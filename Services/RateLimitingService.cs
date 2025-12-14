using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace SmartStockAI.Services
{
    public class RateLimitingService
    {
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _config;


        public RateLimitingService(IMemoryCache cache, IConfiguration config)
        {
            _cache = cache;
            _config = config;
        }

        public bool IsRateLimited(string key, int maxRequests, int windowMinutes)
        {
            var cacheKey = $"ratelimit:{key}";
            var requestCount = _cache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(windowMinutes);
                return 0;
            });

            if (requestCount >= maxRequests)
                return true;

            _cache.Set(cacheKey, requestCount + 1, TimeSpan.FromMinutes(windowMinutes));
            return false;
        }

    }
}