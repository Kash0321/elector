using Cache.Examples.Infrasctructure.Cache;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Kash.CrossCutting.Cache.InMemory
{
    public class InMemoryCacheManager : ICacheManager
    {
        IMemoryCache MemoryCache { get; set; }

        public InMemoryCacheManager(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        public async Task<T> Get<T>(string key)
        {
            var value = (T)MemoryCache.Get(key);

            if (value == null)
                return default;

            return await Task.FromResult(value);
        }

        public async Task<bool> Remove(string key)
        {
            MemoryCache.Remove(key);

            return await Task.FromResult(true);
        }

        public async Task<bool> Set<T>(string key, T value)
        {
            MemoryCache.Set(key, value);

            return await Task.FromResult(true);
        }

        public async Task<bool> Set<T>(string key, T value, int expirationInSeconds)
        {
            throw new NotImplementedException();
        }

        public bool TryGet<T>(string key, out T result)
        {
            if (MemoryCache.TryGetValue(key, out result))
            {
                return true;
            }

            return false;
        }
    }
}
