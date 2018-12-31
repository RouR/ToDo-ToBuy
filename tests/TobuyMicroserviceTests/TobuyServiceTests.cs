using System;
using System.Linq;
using AutoMapper;
using Domain.DBEntities;
using DTO;
using DTO.Internal.TOBUY;
using Microsoft.EntityFrameworkCore;
using ToBuyService.DAL;
using ToBuyService.Services;
using Xunit;

namespace TobuyMicroserviceTests
{
    public class TobuyServiceTests
    {
        [Fact]
        public void TobuyService_Should_CRUD()
        {
            var userId = Guid.NewGuid();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingDTOProfile()); });
            var mapper = mockMapper.CreateMapper();

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TobuyService(context, mapper);

                Assert.Equal(0, context.Tobuy.Count());
                var list1 = service.List(new ListTOBUY() {Page = 1, PageSize = 10, UserId = userId});
                Assert.Equal(0, list1.TotalItems);
                Assert.Equal(0, list1.Items.Count());

                for (var i = 0; i < 50; i++)
                {
                    var ret1 = service.Create(new CreateTOBUY()
                    {
                        Name = "item " + i, PublicId = Guid.NewGuid(), UserId = userId
                    });
                    Assert.Equal(false, ret1.HasError);
                }

                Assert.Equal(50, context.Tobuy.Count());

                var list2 = service.List(new ListTOBUY() {Page = 1, PageSize = 10, UserId = userId});
                Assert.Equal(50, list2.TotalItems);
                Assert.Equal(10, list2.Items.Count());

                var editId = context.Tobuy.Skip(3).First().PublicId;

                var edit1 = service.Find(new FindToBuyRequest() {PublicId = editId, UserId = userId});
                Assert.Equal(false, edit1.HasError);
                Assert.NotNull(edit1.Data);
                var origField = edit1.Data.Name;

                const string newValue = "new value";
                service.Update(new UpdateTOBUY() {Name = newValue, PublicId = editId, UserId = userId});

                var edit2 = service.Find(new FindToBuyRequest() {PublicId = editId, UserId = userId});
                Assert.Equal(false, edit1.HasError);
                Assert.NotNull(edit1.Data);
                Assert.Equal(newValue, edit1.Data.Name);
                Assert.Equal(newValue, edit1.Data.Name);
                Assert.Equal(editId, edit1.Data.PublicId);

                service.Delete(new DeleteTOBUY() {PublicId = editId, UserId = userId});
                Assert.Equal(50, context.Tobuy.Count());
                var list3 = service.List(new ListTOBUY() {Page = 1, PageSize = 10, UserId = userId});
                Assert.Equal(49, list3.TotalItems);

                var newId = Guid.NewGuid();
                var cr1 = service.Create(new CreateTOBUY() {Name = newValue, PublicId = newId, UserId = userId});
                Assert.Equal(false, cr1.HasError);
                Assert.NotNull(cr1.Data);
                Assert.Equal(newValue, cr1.Data.Name);
                Assert.Equal(newId, cr1.Data.PublicId);

                Assert.Equal(51, context.Tobuy.Count());
                var list4 = service.List(new ListTOBUY() {Page = 1, PageSize = 10, UserId = userId});
                Assert.Equal(50, list4.TotalItems);
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(50, context.Tobuy.Count(x => !x.IsDeleted));
                Assert.Equal(51, context.Tobuy.Count());
            }
        }


        [Fact]
        public void TobuyService_Should_Sort()
        {
            var userId = Guid.NewGuid();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingDTOProfile()); });
            var mapper = mockMapper.CreateMapper();

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TobuyService(context, mapper);
                Assert.Equal(0, context.Tobuy.Count());
                for (var i = 0; i < 50; i++)
                {
                    var ret1 = service.Create(new CreateTOBUY()
                    {
                        PublicId = Guid.NewGuid(), UserId = userId,
                        Created = new DateTime(2000, 1, 1).AddDays(i)
                    });
                }

                Assert.Equal(50, context.Tobuy.Count());

                var list1 = service.List(new ListTOBUY()
                    {Page = 1, PageSize = 10, UserId = userId, OrderBy = nameof(TobuyEntity.PublicId), Asc = true});
                var list2 = service.List(new ListTOBUY()
                    {Page = 1, PageSize = 10, UserId = userId, OrderBy = nameof(TobuyEntity.PublicId), Asc = false});
                var list3 = service.List(new ListTOBUY()
                    {Page = 1, PageSize = 10, UserId = userId, OrderBy = nameof(TobuyEntity.Created), Asc = true});
                var list4 = service.List(new ListTOBUY()
                    {Page = 1, PageSize = 10, UserId = userId, OrderBy = nameof(TobuyEntity.Created), Asc = false});

                var first1 = list1.Items.First().PublicId;
                var first2 = list2.Items.First().PublicId;
                var first3 = list3.Items.First().PublicId;
                var first4 = list4.Items.First().PublicId;

                Assert.Equal(4, new[] {first1, first2, first3, first4}.Distinct().Count());
            }
        }
    }
}