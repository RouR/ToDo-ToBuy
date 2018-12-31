using System;
using System.Linq;
using AccountService.DAL;
using AccountService.Services;
using AutoMapper;
using Domain.DBEntities;
using DTO;
using DTO.Internal.Account;
using DTO.Internal.TOBUY;
using Microsoft.EntityFrameworkCore;
using Moq;
using OpenTracing;
using Xunit;

namespace AccountMicroserviceTests
{
    public class AccountServiceTests
    {
        [Fact]
        public void AccountService_Should_RegisterNewUser()
        {
            var userId = Guid.NewGuid();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingDTOProfile()); });
            var mapper = mockMapper.CreateMapper();

            var tracer = new Mock<ITracer>();

            using (var context = new ApplicationDbContext(options))
            {
                var service = new UserService(tracer.Object, context);

                Assert.Equal(0, context.Users.Count());
                Assert.Equal(0, context.LoginAttempts.Count());
                var newUser = new CreateUserRequest()
                {
                    Email = "test@tst.test",
                    Password = Guid.NewGuid().ToString(),
                    Name = "test",
                    PublicId = Guid.NewGuid()
                };
                var ret1 = service.Register(newUser);
                Assert.Equal(false, ret1.HasError);
                Assert.Equal(newUser.Email, ret1.Data.Email);
                Assert.NotNull(ret1.Data.Salt);
                Assert.NotNull(ret1.Data.SaltedPwd);
                Assert.NotEqual(newUser.Password, ret1.Data.SaltedPwd);
                

                var ret2 = service.Login(new TryLoginRequest()
                {
                    UserName = newUser.Email,
                    Password = newUser.Password
                });
                Assert.Equal(false, ret2.HasError);
                Assert.Equal(newUser.Email, ret2.Data.Email);
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(1, context.Users.Count());
                Assert.Equal(1, context.LoginAttempts.Count());
            }
        }


        [Fact]
        public void AccountService_Should_LoginAutoBan()
        {
            const int AttemptCount = 5;
            var AttemptTime = TimeSpan.FromMinutes(5);
            
            var userId = Guid.NewGuid();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingDTOProfile()); });
            var mapper = mockMapper.CreateMapper();

            var tracer = new Mock<ITracer>();

            using (var context = new ApplicationDbContext(options))
            {
                var service = new UserService(tracer.Object, context);

                Assert.Equal(0, context.Users.Count());
                Assert.Equal(0, context.LoginAttempts.Count());
                var newUser = new CreateUserRequest()
                {
                    Email = "test@tst.test",
                    Password = Guid.NewGuid().ToString(),
                    Name = "test",
                    PublicId = Guid.NewGuid()
                };
                var ret1 = service.Register(newUser);
                Assert.Equal(false, ret1.HasError);
                Assert.Equal(1, context.Users.Count());
                Assert.Equal(0, context.LoginAttempts.Count());

                var expectedAttempts = 0;
                for (var i = 0; i < AttemptCount*2; i++)
                {
                    var retBad = service.Login(new TryLoginRequest()
                    {
                        UserName = newUser.Email,
                        Password = Guid.NewGuid().ToString()
                    });
                    Assert.Equal(true, retBad.HasError);
                    Assert.Equal(null, retBad.Data);
                    Assert.Equal(++expectedAttempts, context.LoginAttempts.Count());
                    
                    if (i > AttemptCount)
                    {
                        var retGood = service.Login(new TryLoginRequest()
                        {
                            UserName = newUser.Email,
                            Password = newUser.Password
                        });
                        Assert.Equal(true, retGood.HasError);
                        Assert.Equal(null, retGood.Data);
                        Assert.Equal(++expectedAttempts, context.LoginAttempts.Count());
                    }
                }
                
                //emulate time shift
                foreach (var attempt in context.LoginAttempts)
                {
                    attempt.AtUtc -= AttemptTime.Add(TimeSpan.FromMinutes(1));
                }

                context.SaveChanges();
                
                var retGood2 = service.Login(new TryLoginRequest()
                {
                    UserName = newUser.Email,
                    Password = newUser.Password
                });
                Assert.Equal(false, retGood2.HasError);
                Assert.Equal(newUser.Email, retGood2.Data.Email);
                Assert.Equal(++expectedAttempts, context.LoginAttempts.Count());
            }
        }
    }
}