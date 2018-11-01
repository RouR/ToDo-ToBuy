using System;
using Microsoft.Extensions.Caching.Distributed;

namespace CustomCache.Utils
{
    public abstract class AbsoluteExpirationCache : SomeCache
    {
        public AbsoluteExpirationCache(IDistributedCache cache, TimeSpan timeSpan, string prefix) : base(cache,
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = timeSpan
            }, prefix)
        {
        }
    }
}