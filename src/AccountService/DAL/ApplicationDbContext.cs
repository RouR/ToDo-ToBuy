using System;
using System.IO;
using AccountService.Interfaces;
using Domain.DBEntities;
using DTO.Internal.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json.Linq;

namespace AccountService.DAL
{
    public class ApplicationDbContext : DbContext 
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserLoginAttemptEntity> LoginAttempts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            
            //don`t call SaveChanges() - it will throw exception:
            // A DbContext instance cannot be used inside OnModelCreating

            builder.Entity<UserEntity>().HasIndex(c => new { c.IsDeleted });
            builder.Entity<UserEntity>().HasIndex(c => new { c.Email }).IsUnique();

            builder.Entity<UserLoginAttemptEntity>().HasIndex(c => new { c.Email, c.AtUtc });
        }

        /// <summary>
        /// Used for console commands - dotnet ef migrations add MigrationName
        /// </summary>
        public ApplicationDbContext()
        {
            //don`t delete
        }

        /// <summary>
        /// Used in runtime for EF Migrations
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //don`t delete
        }

        /// <summary>
        /// Used for console commands - dotnet ef migrations add MigrationName
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <exception cref="Exception"></exception>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder);
                return;
            }
            
            var connection = Environment.GetEnvironmentVariable($"sqlCon");
            var path = Path.Combine(Environment.CurrentDirectory, "Properties", "launchSettings.json");
            if (string.IsNullOrEmpty(connection) && File.Exists(path))
            {
                var data = File.ReadAllText(path);
                dynamic json = JToken.Parse(data);
                connection = json.profiles.localProfile.environmentVariables.sqlCon;
            }
            else
                throw new Exception("Database connection string required 'sqlCon' or check json file");
            
            optionsBuilder.UseNpgsql(connection);           
        }


        /// <summary>
        /// Run every time at startup
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userService"></param>
        public static void CustomSeed(ApplicationDbContext context, IUserService userService)
        {
            context.Database.EnsureCreated();


            if (!context.Users.Any())
            {
                userService.Register(new CreateUserRequest()
                {
                    PublicId = Guid.NewGuid(),
                    Email = "demo",
                    Name = "all",
                    Password = "demo123",
                    IP = null,
                });
            }

        }
    }
}