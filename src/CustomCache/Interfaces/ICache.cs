using System;

namespace CustomCache.Interfaces
{
    public interface ICache
    {
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class;
        void Remove(string cacheKey);
        void Clear();
    }
}