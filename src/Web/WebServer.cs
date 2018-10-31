using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Shared;

//Install-Package Microsoft.AspNet.HealthChecks
namespace Web
{
    public static class WebServer
    {
        public static int Main(string[] args)
        {
            try
            {
                var bindingConfig = new ConfigurationBuilder()
                    .AddCommandLine(args)
                    .Build();
                var serverport = bindingConfig.GetValue<int?>("port") ?? 5555;
                
                BuildWebHost(args, serverport).Run();
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