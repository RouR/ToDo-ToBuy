using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Shared;

#pragma warning disable 1591

//Install-Package Microsoft.AspNet.HealthChecks
namespace ToDoService
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                BuildWebHost(args).Run();
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

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>()
                .UseHealthChecks(ServiceClients.HealthCheck) //nginx-ingress require this path
                .UseUrls("http://0.0.0.0:5555") // Take that 0.0.0.0 instead of localhost, Docker port forwarding!!!
                .Build();
    }
}
