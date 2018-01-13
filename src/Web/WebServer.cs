using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Web
{
    public static class WebServer
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>()
                .UseHealthChecks("/healthz") //nginx-ingress require this path
                .UseUrls("http://0.0.0.0:5555") // Take that 0.0.0.0 instead of localhost, Docker port forwarding!!!
                .Build();
    }
}
