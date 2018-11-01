using System;
using Microsoft.Extensions.Caching.Distributed;

namespace CustomCache.Utils
{
    public abstract class SlidingExpirationCache : SomeCache
    {
        public SlidingExpirationCache(IDistributedCache cache, TimeSpan timeSpan, string prefix) : base(cache,
            new DistributedCacheEntryOptions()
            {
                SlidingExpiration = timeSpan
            }, prefix)
        {
        }
    }
}