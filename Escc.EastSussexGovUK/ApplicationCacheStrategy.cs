using System;
using System.Runtime.Caching;

namespace Escc.EastSussexGovUK
{
    /// <summary>
    /// Cache values using the default memory cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Escc.EastSussexGovUK.ICacheStrategy{T}" />
    public class ApplicationCacheStrategy<T> : ICacheStrategy<T>
    {
        private readonly TimeSpan _cacheDuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationCacheStrategy{T}"/> class.
        /// </summary>
        /// <param name="cacheDuration">Duration of the cache.</param>
        public ApplicationCacheStrategy(TimeSpan cacheDuration)
        {
            _cacheDuration = cacheDuration;
        }

        /// <summary>
        /// Adds a value to the cache with a fixed maximum expiration time.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddToCache(string key, T value)
        {
            if (value != null)
            {
                MemoryCache.Default.Set(key, value, (DateTime.Now + _cacheDuration));
            }
        }

        /// <summary>
        /// Reads a value from the cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T ReadFromCache(string key)
        {
            if (MemoryCache.Default.Contains(key))
            {
                return (T)MemoryCache.Default[key];
            }
            return default(T);
        }

        /// <summary>
        /// Removes a value from the cache.
        /// </summary>
        /// <param name="key">The key.</param>
        public void RemoveFromCache(string key)
        {
            if (MemoryCache.Default.Contains(key))
            {
                MemoryCache.Default.Remove(key);
            }
        }
    }
}
