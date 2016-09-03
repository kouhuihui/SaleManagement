using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement.Core
{
    public static class CachingHelper
    {
        static ConcurrentDictionary<string, IMemoryCache> _Caches = new ConcurrentDictionary<string, IMemoryCache>();
        public static IMemoryCache GetMemoryCache(string name)
        {
            var cache = _Caches.GetOrAdd(name, n => CreateMemoryCache(n));
            return cache;
        }

        public static IMemoryCache CreateMemoryCache(string name)
        {
            return new MemoryCache(new MemoryCacheOptions());
        }
    }
}
