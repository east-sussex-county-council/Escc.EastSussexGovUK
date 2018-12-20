using System;
using System.Runtime.Caching;

namespace Escc.EastSussexGovUK.Views
{
    /// <summary>
    /// Cache remote template elements using the default memory cache
    /// </summary>
    public class RemoteMasterPageMemoryCacheProvider : RemoteMasterPageCacheProviderBase
    {
        private ObjectCache _cache = MemoryCache.Default;

        /// <summary>
        /// Creates a new instance of a <see cref="RemoteMasterPageMemoryCacheProvider"/>
        /// </summary>
        /// <param name="cacheMinutes">The number of minutes to cache the master page elements for</param>
        public RemoteMasterPageMemoryCacheProvider(int cacheMinutes) : base(cacheMinutes)
        {
        }

        /// <summary>
        /// Saves the remote HTML to the cache.
        /// </summary>
        /// <param name="applicationId">A string which identifies the application making the request</param>
        /// <param name="control">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        /// <param name="html">The HTML.</param>
        public override void SaveRemoteHtmlToCache(string applicationId, string control, string selectedSection, int textSize, bool isLibraryCatalogueRequest, string html)
        {
            var cacheToken = GetCacheToken(applicationId, control, selectedSection, textSize, isLibraryCatalogueRequest);
            if (_cache.Contains(cacheToken))
            {
                _cache.Remove(cacheToken);
            }
            _cache.Set(cacheToken, html, DateTime.Now.AddMinutes(GetCacheMinutes()));
        }

        /// <summary>
        /// Gets the best available cached response (up-to-date or not)
        /// </summary>
        /// <param name="applicationId">A string which identifies the application making the request</param>
        /// <param name="control">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        /// <returns></returns>
        public override string ReadHtmlFromCache(string applicationId, string control, string selectedSection, int textSize, bool isLibraryCatalogueRequest)
        {
            var cacheToken = GetCacheToken(applicationId, control, selectedSection, textSize, isLibraryCatalogueRequest);
            return (_cache.Contains(cacheToken)) ? _cache[cacheToken].ToString() : String.Empty;
        }

        /// <summary>
        /// Gets whether a cached version exists.
        /// </summary>
        /// <param name="applicationId">A string which identifies the application making the request</param>
        /// <param name="control">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        /// <value><c>true</c> if a cached version exists; otherwise, <c>false</c>.</value>
        public override bool CachedVersionExists(string applicationId, string control, string selectedSection, int textSize, bool isLibraryCatalogueRequest)
        {
            return (_cache.Contains(GetCacheToken(applicationId, control, selectedSection, textSize, isLibraryCatalogueRequest)));
        }

        /// <summary>
        /// Gets a value indicating whether cached version is newer than the cache threshold.
        /// </summary>
        /// <param name="applicationId">A string which identifies the application making the request</param>
        /// <param name="control">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        /// <value>
        /// 	<c>true</c> if cached version is fresh; otherwise, <c>false</c>.
        /// </value>
        public override bool CachedVersionIsFresh(string applicationId, string control, string selectedSection, int textSize, bool isLibraryCatalogueRequest)
        {
            return CachedVersionExists(applicationId, control, selectedSection, textSize, isLibraryCatalogueRequest);
        }
    }
}