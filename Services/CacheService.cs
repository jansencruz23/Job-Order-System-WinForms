using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Job_Order_System.Services
{
    public static class CacheService
    {
        private static MemoryCache cache = MemoryCache.Default;

        public static T Get<T>(string key)
        {
            if (cache.Contains(key))
            {
                return (T)cache[key];
            }

            return default;
        }

        public static void Add<T>(string key, T value, DateTimeOffset absoluteExpiration)
        {
            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration };
            cache.Set(key, value, policy);
        }

        public static void Remove(string key)
        {
            if (cache.Contains(key))
            {
                cache.Remove(key);
            }
        }
    }
}