using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cache.Examples.Infrasctructure.Cache
{
    public interface ICacheManager
    {
        Task<T> Get<T>(string key);

        bool TryGet<T>(string key, out T result);

        Task<bool> Set<T>(string key, T value);

        Task<bool> Set<T>(string key, T value, int expirationInSeconds);

        Task<bool> Remove(string key);
    }
}
