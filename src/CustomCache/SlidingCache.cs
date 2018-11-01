using System;
using CustomCache.Interfaces;
using CustomCache.Utils;
using Microsoft.Extensions.Caching.Distributed;

namespace CustomCache
{
    public class SlidingCache : SlidingExpirationCache, ISlidingCache
    {
        public const int CacheMinutes = 10;

        public SlidingCache(IDistributedCache cache) : base(cache, TimeSpan.FromMinutes(CacheMinutes), "r_")
        {
        }
    }
}