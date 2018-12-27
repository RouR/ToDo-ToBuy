using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using Shared;
using Swashbuckle.AspNetCore.Swagger;

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

                var host = BuildWebHost(args, serverport);
                
                if(args!= null && args.Any(x=> x.Equals("swagger", StringComparison.InvariantCultureIgnoreCase)))
                    GenerateSwaggerFiles(host);
                else
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

        private static void GenerateSwaggerFiles(IWebHost host)
        {
            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "../../swagger");
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);
            
            Console.WriteLine("save swagger files to {0}", outputPath);
            
            var sw = (ISwaggerProvider) host.Services.GetService(typeof(ISwaggerProvider));
            // ... and document here. Which throws exception when it tries to add duplicate "x-purpose" in
            // the AssignOperationVendorExtensions.Apply()
            var provider = (IApiVersionDescriptionProvider) host.Services.GetService(typeof(IApiVersionDescriptionProvider));

            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in provider.ApiVersionDescriptions)
            {
                var swaggerDoc = sw.GetSwagger(description.GroupName, null, "/"); //  
                var swaggerString = JsonConvert.SerializeObject(
                    swaggerDoc,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = new SwaggerContractResolver(new JsonSerializerSettings())
                    });
                File.WriteAllText($"../../swagger/swagger-Web-{description.GroupName}.json", swaggerString);
            }

//                var apiVersion = "v0.1";
//                var swaggerDoc = sw.GetSwagger(apiVersion, null, "/");
//                
//                var swaggerString = JsonConvert.SerializeObject(
//                    swaggerDoc,
//                    Formatting.Indented,
//                    new JsonSerializerSettings
//                    {
//                        NullValueHandling = NullValueHandling.Ignore,
//                        ContractResolver = new SwaggerContractResolver(new JsonSerializerSettings())
//                    });
//                File.WriteAllText("swagger.json", swaggerString);
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