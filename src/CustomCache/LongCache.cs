using System;
using CustomCache.Interfaces;
using CustomCache.Utils;
using Microsoft.Extensions.Caching.Distributed;

namespace CustomCache
{
    public class LongCache : AbsoluteExpirationCache, ILongCache
    {
        public const int CacheMinutes = 600;

        public LongCache(IDistributedCache cache) : base(cache, TimeSpan.FromMinutes(CacheMinutes), "l_")
        {
        }
    }
}