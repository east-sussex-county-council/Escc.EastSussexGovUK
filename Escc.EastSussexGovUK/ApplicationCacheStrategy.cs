using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Escc.EastSussexGovUK
{
    /// <summary>
    /// Cache values using the ASP.NET application cache
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
            HttpContext.Current.Cache.Insert(key, value, null, (DateTime.Now + _cacheDuration), Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// Reads a value from the cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T ReadFromCache(string key)
        {
            if (HttpContext.Current.Cache[key] != null)
            {
                return (T)HttpContext.Current.Cache[key];
            }
            return default(T);
        }

        /// <summary>
        /// Removes a value from the cache.
        /// </summary>
        /// <param name="key">The key.</param>
        public void RemoveFromCache(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }
    }
}
