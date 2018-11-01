using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.HealthChecks;

namespace CustomCache.Utils
{
    public static class HealthCheckBuilderRedisExtensions  
    {  
  
        public static HealthCheckBuilder AddRedisCheck(this HealthCheckBuilder builder, RedisCacheOptions options)
        {
            if (options == null)
            {
                Console.WriteLine("REDIS cache is disabled (RedisCacheOptions options is null)");
                return builder;
            }
            
            var key = $"redisHealthCheck_{Guid.NewGuid()}";
            var duration = TimeSpan.FromSeconds(1);
            builder.AddCheck($"RedisCheck({options.InstanceName})", async () =>  
            {  
                try  
                {  
                    using (var client = new RedisCache(options))
                    {
                        string data = Guid.NewGuid().ToString();
                        
                        await client.SetAsync(key, data.ToByteArray(), new DistributedCacheEntryOptions()
                        {
                            AbsoluteExpirationRelativeToNow = duration
                        });

                        var response = client.Get(key);
  
                        if (response != null && response.FromByteArray<string>() == data)  
                        {  
                            return HealthCheckResult.Healthy($"RedisCheck({options.InstanceName}): Healthy");  
                        }  
                        return HealthCheckResult.Unhealthy($"RedisCheck({options.InstanceName}): Unhealthy");  
  
                    }  
                }  
                catch (Exception ex)  
                {  
                    return HealthCheckResult.Unhealthy($"RedisCheck({options.InstanceName}): Exception during check: {ex.GetType().FullName}");  
                }  
            }, duration);  
  
            return builder;  
        }  
    }
}