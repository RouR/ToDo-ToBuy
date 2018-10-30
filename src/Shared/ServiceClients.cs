using System;
using System.Collections.Concurrent;
using DTO;
using Microsoft.Extensions.DependencyInjection;

namespace Shared
{
    public static class ServiceClients
    {
        public const string HealthCheck = "/healthz";
        
        public static void ConfigureServices(IServiceCollection services, Serilog.ILogger logger)
        {
            services.AddSingleton<AccountServiceClient>();
            services.AddSingleton<ToDoServiceClient>();
            services.AddSingleton<ToBuyServiceClient>();
            
            Register(Service.Account);
            Register(Service.ToDo);
            Register(Service.ToBuy);

            void Register(Service clientId)
            {
                BaseClient.DefaultStrategy(logger, services.AddHttpClient(clientId.ToString()));
            }
        }


        private static readonly ConcurrentDictionary<Service, Uri> CacheUrl = new ConcurrentDictionary<Service, Uri>();

        private static Uri GetUri(Service srv)
        {
            //https://docs.microsoft.com/en-gb/dotnet/api/system.uri.getleftpart?view=netcore-2.1
            if (CacheUrl.ContainsKey(srv))
            {
                return CacheUrl[srv];
            }

            var url = Environment.GetEnvironmentVariable($"api{srv.ToString()}");
            var uri = new Uri(url);
            CacheUrl.TryAdd(srv, uri);
            return uri;
        }
        
        public static string GetApiKey(Service srv)
        {
            return Environment.GetEnvironmentVariable($"apiKey{srv.ToString()}");
        }

        public static string Url(Service srv)
        {
            return GetUri(srv).GetLeftPart(System.UriPartial.Authority);
        }

        public static string HealthUrl(Service srv)
        {
            var uri = new Uri(GetUri(srv), HealthCheck);
            return uri.AbsoluteUri;
        }
    }
}