using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AccountService.DAL;
using AccountService.Interfaces;
using Domain.DBEntities;
using DTO.Internal.Account;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using OpenTracing;
using Utils;

namespace AccountService.Services
{
    public class UserService : IUserService
    {
        private readonly ITracer _tracer;
        private readonly ApplicationDbContext _dbContext;

        public UserService(ITracer tracer, ApplicationDbContext dbContext)
        {
            _tracer = tracer;
            _dbContext = dbContext;
        }
        public CreateUserResponse Register(CreateUserRequest model)
        {
            if(model.PublicId == Guid.Empty)
                return new CreateUserResponse
                {
                    HasError = true,
                    Message = "UserId must be pre-generated"
                };

            if(string.IsNullOrWhiteSpace(model.Email))
                return new CreateUserResponse
                {
                    HasError = true,
                    Message = "Email is required"
                };

            var salt = GenerateSalt(UserEntity.SaltMaxLength);
            var saltedPwd = Salt(salt, model.Password, UserEntity.SaltedPwdMaxLength);

            return DBHelpers.TryDoTransaction<CreateUserResponse, UserEntity>(_dbContext, () =>
            {
                var user  = _dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefault(x => x.PublicId == model.PublicId || x.Email == model.Email.ToLower());

                if (user != null && user.PublicId == model.PublicId)
                {
                    if(user.IsDeleted)
                        return (false, new CreateUserResponse
                        {
                            HasError = true,
                            Message = "This user was deleted"
                        });

                    return (false, new CreateUserResponse
                    {
                        HasError = true,
                        Message = "This user is already exist"
                    });   
                }
                if (user != null)
                {
                    return (false, new CreateUserResponse
                    {
                        HasError = true,
                        Message = "Email must be unique"
                    });   
                }

                var newUser = _dbContext.Users.Add(new UserEntity()
                {
                    PublicId = model.PublicId,
                    Name = model.Name,
                    Email = model.Email.ToLower(),
                    Salt = salt,
                    SaltedPwd =  saltedPwd,
                    IP = model.IP,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    IsDeleted = false,
                });

                return (true, new CreateUserResponse()
                {
                    Data = newUser.Entity
                });
            });
        }

        public TryLoginResponse Login(TryLoginRequest model)
        {
            if(string.IsNullOrWhiteSpace(model.UserName))
                return new TryLoginResponse
                {
                    HasError = true,
                    Message = "Email is required"
                };
            if(string.IsNullOrWhiteSpace(model.Password))
                return new TryLoginResponse
                {
                    HasError = true,
                    Message = "Password is required"
                };

            var email = model.UserName.ToLower();

            var user = _dbContext.Users.SingleOrDefault(x => x.Email == email);
            if(user == null)
                return new TryLoginResponse
                {
                    HasError = true,
                    Message = "Unknown user"
                };

            if(user.IsDeleted)
                return new TryLoginResponse
                {
                    HasError = true,
                    Message = "User was deleted"
                };

            var attempts = _dbContext.LoginAttempts.Count(x => x.Email == email && x.AtUtc > DateTime.UtcNow.AddMinutes(-5));
            _dbContext.LoginAttempts.Add(new UserLoginAttemptEntity()
            {
                PublicId = Guid.NewGuid(),
                Email = email,
                AtUtc = DateTime.UtcNow,
                IP = model.IP,
            });
            _dbContext.SaveChanges();

            if(attempts > 5)
                return new TryLoginResponse
                {
                    HasError = true,
                    Message = "Try login later (5 min)"
                };

            if (user.SaltedPwd == Salt(user.Salt, model.Password, UserEntity.SaltedPwdMaxLength))
            {
                return new TryLoginResponse
                {
                    HasError = false,
                    Data = user
                };
            }
            else
            {
                return new TryLoginResponse
                {
                    HasError = true,
                    Message = "Wrong password"
                };

            }
        }


        private string GenerateSalt(int maxLength)
        {
            var data = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }

            var salt = BitConverter.ToString(data).Replace("-", "").ToLower();  
            return salt.Substring(0, Math.Min(salt.Length, maxLength));
        }

        private string Salt(string salt, string password, int maxLength)
        {
            var encoding = new ASCIIEncoding();
            var saltBytes = encoding.GetBytes(salt);

            var hash =  BitConverter.ToString(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8)).Replace("-", "").ToLower();

            return hash.Substring(0, Math.Min(hash.Length, maxLength));
        }

    }
}
