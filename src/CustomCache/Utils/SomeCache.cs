using System;
using CustomCache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace CustomCache.Utils
{
    public abstract class SomeCache : ICache
    {
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _options;
        private readonly string _prefix;

        public SomeCache(IDistributedCache cache, DistributedCacheEntryOptions options, string prefix)
        {
            _cache = cache;
            _options = options;
            _prefix = prefix;
        }

        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            T item = _cache.Get(_prefix + cacheKey).FromByteArray<T>();

            if (item == null)
            {
                item = getItemCallback();
                _cache.Set(_prefix + cacheKey, item.ToByteArray(), _options);
            }

            return item;
        }

        public void Remove(string cacheKey)
        {
            _cache.Remove(_prefix + cacheKey);
        }

        public void Clear()
        {
            //not implemented https://github.com/aspnet/Caching/issues/96
        }
    }
}