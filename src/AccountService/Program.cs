using System;
using AccountService.DAL;
using AccountService.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shared;

//Install-Package Microsoft.AspNet.HealthChecks
namespace AccountService
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                var bindingConfig = new ConfigurationBuilder()
                    .AddCommandLine(args)
                    .Build();
                var serverport = bindingConfig.GetValue<int?>("port") ?? 5555;

                var host = BuildWebHost(args, serverport);
                
                // cd ./src/AccountService/ && dotnet ef migrations add Initial && cd ../..
                using (var scope = host.Services.CreateScope())
                {
                    using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
                    {
                        context.Database.Migrate();

                        var userService = scope.ServiceProvider.GetService<IUserService>();
                        ApplicationDbContext.CustomSeed(context, userService);
                    }
                }
                
                host.Run();
                Log.Information("Shutdown...");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Host terminated unexpectedly " + ex.Message);
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.Information("Shutdown finally");
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildWebHost(string[] args, int serverport) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>()
                .UseHealthChecks(ServiceClients.HealthCheck) //nginx-ingress require this path
                .UseUrls("http://0.0.0.0:"+serverport) // Take that 0.0.0.0 instead of localhost, Docker port forwarding!!!
                .Build();
    }
}
