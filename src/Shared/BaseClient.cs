using System;
using System.Net.Http;
using DTO;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Shared
{
    public abstract class BaseClient
    {
        protected readonly HttpClient _client;
        
        protected BaseClient(Service clientId, IHttpClientFactory factory)
        {
            var url = ServiceClients.Url(clientId);
            Console.WriteLine($"{clientId.ToString()}Client Url {url}");
            var uri = new Uri(url);
            Console.WriteLine($"{clientId.ToString()}Client Uri {uri}");

            _client = factory.CreateClient(clientId.ToString());
//            _client = new HttpClient
//            {
//                BaseAddress = uri
//            };
//            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static void DefaultStrategy(IHttpClientBuilder builder)
        {
            builder.AddTransientHttpErrorPolicy(p => 
                p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(100)));
        }
    }
}