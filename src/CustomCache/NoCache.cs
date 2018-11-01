using System;
using CustomCache.Interfaces;

namespace CustomCache
{
    public class NoCache : ICache, IShortCache, ILongCache, ISlidingCache
    {
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            return getItemCallback();
        }

        public void Remove(string cacheKey)
        {
            
        }

        public void Clear()
        {
            
        }
    }
}