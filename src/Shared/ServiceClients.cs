using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Shared
{
    /// <summary>
    /// Microservices
    /// </summary>
    public enum Service
    {
        Account = 1,
    }

    public static class ServiceClients
    {
        public const string HealthCheck = "/healthz";

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<AccountServiceClient>();
        }


        private static readonly ConcurrentDictionary<Service, Uri> CacheUrl = new ConcurrentDictionary<Service, Uri>();

        private static Uri GetUri(Service srv)
        {
            //https://docs.microsoft.com/en-gb/dotnet/api/system.uri.getleftpart?view=netcore-2.1
            if (CacheUrl.ContainsKey(srv))
            {
                return CacheUrl[srv];
            }
            else
            {
                var url = Environment.GetEnvironmentVariable($"api{srv.ToString()}");
                var uri = new Uri(url);
                CacheUrl.TryAdd(srv, uri);
                return uri;
            }
        }

        public static string Url(Service srv)
        {
            return GetUri(srv).Authority;
        }

        public static string HealthUrl(Service srv)
        {
            var uri = new Uri(GetUri(srv), HealthCheck);
            return uri.AbsolutePath;
        }
    }
}