using System;
using CustomCache.Interfaces;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace CustomCache
{
    public static class SetupCustomCache
    {
        public static void ConfigureServices(IServiceCollection services, out RedisCacheOptions redisCacheOptions)
        {
            var ipAddr = Environment.GetEnvironmentVariable("REDIS_HOST");

            if (string.IsNullOrEmpty(ipAddr))
            {
                Console.WriteLine("REDIS cache is disabled (environment REDIS_HOST)");
                redisCacheOptions = null;
                
                services.AddSingleton<ICache, NoCache>();
                services.AddSingleton<IShortCache, NoCache>();
                services.AddSingleton<ILongCache, NoCache>();
                services.AddSingleton<ISlidingCache, NoCache>();
                
                return;
            }
            
            services.AddSingleton<ICache, ShortCache>();
            services.AddSingleton<IShortCache, ShortCache>();
            services.AddSingleton<ILongCache, LongCache>();
            services.AddSingleton<ISlidingCache, SlidingCache>();

            var instanceName = "iName";
            //InstanceName used as key prefix inside Microsoft.Extensions.Caching.Redis 
            
            redisCacheOptions = new RedisCacheOptions()
            {
                Configuration = ipAddr,
                InstanceName = instanceName,
            };

            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = ipAddr;
                option.InstanceName = instanceName;
            });

            
        }
    }
}