using Microsoft.Extensions.Caching.Memory;

namespace API.Helpers
{
    /// <summary>
    /// In memory cache implementation of Microsoft.Extensions.Caching.Memory
    /// </summary>
    public class Cache
    {
        protected MemoryCache MemoryCaching { get; }
        protected long SizeLimit { get; }
        protected CacheItemPriority CachePriority { get; }
        protected TimeSpan CacheLifeSpan { get; }
        protected TimeSpan CacheExpire { get; }

        /// <summary>
        /// Initialize a new instance of cache manager with options
        /// </summary>
        /// <param name="sizeLimit">Number of items the cache can contain</param>
        /// <param name="cachePriority">Priority for keeping an item when reaching size limit (memory pressure)</param>
        /// <param name="cacheLifeSpan">How long an item stay in cache before removed (in minutes)</param>
        /// <param name="cacheExpireDays">Days before the cache is absolutely expired</param>
        public Cache(long sizeLimit, CacheItemPriority cachePriority,
            double cacheLifeSpan,
            double cacheExpireDays
        )
        {
            MemoryCaching = new MemoryCache(new MemoryCacheOptions());
            SizeLimit = sizeLimit;
            CachePriority = cachePriority;
            CacheLifeSpan = TimeSpan.FromMinutes(cacheLifeSpan);
            CacheExpire = TimeSpan.FromMinutes(cacheExpireDays);
        }

        public void Set<T>(string key, T value)
        {
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(SizeLimit)
                .SetPriority(CachePriority)
                .SetSlidingExpiration(CacheLifeSpan)
                .SetAbsoluteExpiration(CacheExpire);
            MemoryCaching.Set(key, value, memoryCacheEntryOptions);
        }
        public T Get<T>(string key)
        {
            return MemoryCaching.Get<T>(key);
        }

        public T GetOrCreate<T>(object key, T value)
        {
            if (MemoryCaching.TryGetValue(key, out var item)) return (T)Convert.ChangeType(item, typeof(T));
            item = value;
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(SizeLimit)
                .SetPriority(CachePriority)
                .SetSlidingExpiration(CacheLifeSpan)
                .SetAbsoluteExpiration(CacheExpire);
            MemoryCaching.Set(key, item, memoryCacheEntryOptions);
            return (T)Convert.ChangeType(item, typeof(T));
        }

        public void Remove(object key)
        {
            if (MemoryCaching.TryGetValue(key, out _))
                MemoryCaching.Remove(key);
        }
        public async Task<T> GetOrCreate<T>(object key, Func<Task<T>> function)
        {
            if (MemoryCaching.TryGetValue(key, out var item)) return (T)Convert.ChangeType(item, typeof(T));
            item = await function();
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(SizeLimit)
                .SetPriority(CachePriority)
                .SetSlidingExpiration(CacheLifeSpan)
                .SetAbsoluteExpiration(CacheExpire);
            MemoryCaching.Set(key, item, memoryCacheEntryOptions);
            return (T)Convert.ChangeType(item, typeof(T));
        }
    }
}
