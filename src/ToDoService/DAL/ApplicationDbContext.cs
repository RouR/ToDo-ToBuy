using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace ToDoService.DAL
{
    public class ApplicationDbContext : DbContext 
    {

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
        
        /*
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            
            //don`t call SaveChanges() - it will throw exception:
            // A DbContext instance cannot be used inside OnModelCreating
          
        }
        */

        /// <summary>
        /// Run every time at startup
        /// </summary>
        /// <param name="context"></param>
        public static void CustomSeed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            
            /*
            if (!context.Accounts.Any())
            {
                // Seeding the Database
                // ...

                context.Accounts.Add(new AccountEntity()
                {
                    Name = "auto"
                });
                context.SaveChanges();
            }
            */
        }
    }
}