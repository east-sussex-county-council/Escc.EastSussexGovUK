using System;

namespace Escc.EastSussexGovUK
{
    /// <summary>
    /// An interface for reading and writing a value to a simple cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICacheStrategy<T>
    {
        /// <summary>
        /// Adds a value to the cache with a fixed maximum expiration time.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void AddToCache(string key, T value);

        /// <summary>
        /// Reads a value from the cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        T ReadFromCache(string key);

        /// <summary>
        /// Removes a value from the cache.
        /// </summary>
        /// <param name="key">The key.</param>
        void RemoveFromCache(string key);
    }
}