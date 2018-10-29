using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

            if(!int.TryParse(Environment.GetEnvironmentVariable($"apiTimeout"), out var timeout))
                timeout = 1000;
            
            _client = factory.CreateClient(clientId.ToString());
            _client.BaseAddress = uri;
            _client.Timeout = TimeSpan.FromMilliseconds(timeout);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static void DefaultStrategy(IHttpClientBuilder builder)
        {
            if(!int.TryParse(Environment.GetEnvironmentVariable($"apiRetriesCount"), out var retries))
                retries = 3;
            
            if(!int.TryParse(Environment.GetEnvironmentVariable($"apiRetriesDelayMin"), out var delayMin))
                delayMin = 100;
            
            if(!int.TryParse(Environment.GetEnvironmentVariable($"apiRetriesDelayStep"), out var delayStep))
                delayStep = 50;
            
            if(!int.TryParse(Environment.GetEnvironmentVariable($"apiRetriesJitterMin"), out var jitterMin))
                jitterMin = 0;
            
            if(!int.TryParse(Environment.GetEnvironmentVariable($"apiRetriesJitterMax"), out var jitterMax))
                jitterMax = 100;
            
            if(!int.TryParse(Environment.GetEnvironmentVariable($"apiCircuitBreakerCount"), out var circuitCount))
                circuitCount = 5;
            
            if(!int.TryParse(Environment.GetEnvironmentVariable($"apiCircuitBreakerDelaySeconds"), out var circuitDelay))
                circuitDelay = 30;
            
            if(!int.TryParse(Environment.GetEnvironmentVariable($"apiHandlerLifetimeSeconds"), out var lifetime))
                lifetime = 300;
            
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-2.1
            //https://github.com/App-vNext/Polly
            //https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory
            //HttpRequestException, 5XX and 408
            var jitter = new Random(); 
            builder
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retries, retryAttempt  => 
                        TimeSpan.FromMilliseconds(delayMin + retryAttempt * delayStep) 
                        + TimeSpan.FromMilliseconds(jitter.Next(jitterMin, jitterMax)))
                )
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(circuitCount, TimeSpan.FromSeconds(circuitDelay)))
                .SetHandlerLifetime(TimeSpan.FromSeconds(lifetime)) // can prevent the handler from reacting to DNS changes
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
                {
                    AllowAutoRedirect = true,
                    MaxAutomaticRedirections = 3,
                    UseDefaultCredentials = false,
                    AutomaticDecompression = DecompressionMethods.GZip,
                });
        }
    }
}