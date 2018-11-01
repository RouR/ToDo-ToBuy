using System;
using CustomCache.Interfaces;
using CustomCache.Utils;
using Microsoft.Extensions.Caching.Distributed;

namespace CustomCache
{
    public class ShortCache : AbsoluteExpirationCache, IShortCache
    {
        public const int CacheMinutes = 10;

        public ShortCache(IDistributedCache cache) : base(cache, TimeSpan.FromMinutes(CacheMinutes), "s_")
        {
        }
    }
}