using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterBackend.Services
{
    public class AppMemoryCache
    {
        public MemoryCache Cache { get; set; }
        public AppMemoryCache()
        {
            Cache = new MemoryCache(new MemoryCacheOptions());
        }
    }
}
