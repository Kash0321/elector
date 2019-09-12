using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kash.CrossCutting.Cache.Redis
{
    public class RedisCacheManager : ICacheManager
    {
        IDistributedCache DistributedCache { get; set; }

        public RedisCacheManager(IDistributedCache distributedCache)
        {
            DistributedCache = distributedCache;
        }

        public async Task<T> Get<T>(string key)
        {
            var value = await DistributedCache.GetStringAsync(key);

            if (string.IsNullOrWhiteSpace(value))
                return default;

            await DistributedCache.RefreshAsync(key);

            return JsonConvert.DeserializeObject<T>(value);
        }

        public bool TryGet<T>(string key, out T result)
        {
            result = default;

            var value = DistributedCache.GetString(key);

            if (string.IsNullOrWhiteSpace(value))
                return false;

            DistributedCache.Refresh(key);

            result = JsonConvert.DeserializeObject<T>(value);
            return true;
        }

        public async Task<bool> Set<T>(string key, T value)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddDays(1)
            };

            await DistributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), options);

            return true;
        }

        public async Task<bool> Set<T>(string key, T value, int expirationInSeconds)
        {
            var options = new DistributedCacheEntryOptions()
            {

                AbsoluteExpiration = DateTime.Now.AddSeconds(expirationInSeconds)
            };

            await DistributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), options);

            return true;
        }

        public async Task<bool> Remove(string key)
        {
            await DistributedCache.RemoveAsync(key);

            return true;
        }
    }
}
