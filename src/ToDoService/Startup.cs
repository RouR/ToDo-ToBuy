using System;
using CustomCache;
using CustomCache.Utils;
using CustomMetrics;
using CustomTracing;
using DTO;
using HealthChecks.PostgreSQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared;
using ToDoService.DAL;

namespace ToDoService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //Configuration = configuration;

            CustomLogs.SetupCustomLogs.ConfigureStartup();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var instanceInfo = new InstanceInfo();
            services.AddSingleton(instanceInfo);

            CustomLogs.SetupCustomLogs.ConfigureServices(instanceInfo);
            SetupDefaultWebMetrics.ConfigureServices(instanceInfo, services);
            SetupTracing.ConfigureServices(instanceInfo, services, false);
            ServiceClients.ConfigureServices(services, CustomLogs.SetupCustomLogs.Logger());
            SetupCustomCache.ConfigureServices(services, out var redisCacheOptions);
            
            CustomLogs.SetupCustomLogs.PrintAllEnv();

            var connection = Environment.GetEnvironmentVariable($"sqlCon") ?? throw new Exception("Database connection string required 'sqlCon'");
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));

            services.AddMvc();

            services.AddHealthChecks(checks =>
            {
                //However, the MVC web application has multiple dependencies on the rest of the microservices. Therefore, it calls one AddUrlCheck method for each microservice
                checks.AddPostgreSqlCheck("any_text", connection, TimeSpan.FromSeconds(10)); // https://github.com/sindrunas/aspnetcore.healthcheck.postgresqlextension
                //checks.AddSqlCheck("CatalogDb", connection);
                //checks.AddUrlCheck(Configuration["CatalogUrl"]);

                checks.AddRedisCheck(redisCacheOptions);
                
                //If the microservice does not have a dependency on a service or on SQL Server, you should just add a Healthy("Ok") check.
//                checks.AddValueTaskCheck("HTTP Endpoint",
//                    () => new ValueTask<IHealthCheckResult>(HealthCheckResult.Healthy("Ok")));
            });
        }


        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime,
            ILoggerFactory loggerFactory)
        {
            //var configuration = app.ApplicationServices.GetService<TelemetryConfiguration>();
            //configuration.DisableTelemetry = true;

            CustomLogs.SetupCustomLogs.Configure(loggerFactory, applicationLifetime);
            SetupDefaultWebMetrics.Configure(app);

            app.ApiKeyMiddleware();
            KeyHeaderChecker.SetApiKey(ServiceClients.GetApiKey(Service.ToDo));
            
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "areas",
                //    template: "{area:exists}/{controller=Home}/{action=Index}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}"
                    //defaults: new {area = "home", controller = "Hello", action = "Index"}
                    );
            });
        }
    }
}