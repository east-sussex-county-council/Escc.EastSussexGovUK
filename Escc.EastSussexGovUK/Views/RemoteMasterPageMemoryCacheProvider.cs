using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using Escc.EastSussexGovUK.Features;

namespace Escc.EastSussexGovUK.Views
{
    /// <summary>
    /// Cache remote template elements using the ASP.NET application cache
    /// </summary>
    public class RemoteMasterPageMemoryCacheProvider : RemoteMasterPageCacheProviderBase
    {
        private readonly string _cacheToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteMasterPageMemoryCacheProvider" /> class.
        /// </summary>
        /// <param name="control">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        /// <param name="libraryCatalogueContext">The library catalogue context.</param>
        /// <param name="textSize">The site's text size feature.</param>
        public RemoteMasterPageMemoryCacheProvider(string control, string selectedSection, ILibraryCatalogueContext libraryCatalogueContext, ITextSize textSize)
            : base(control, selectedSection, libraryCatalogueContext, textSize)
        {
            _cacheToken = GetCacheToken();
            CachedVersionExists = (HttpContext.Current.Cache[_cacheToken] != null);
            CachedVersionIsFresh = CachedVersionExists;
        }

        /// <summary>
        /// Saves the remote HTML to the cache.
        /// </summary>
        /// <param name="stream">The HTML stream.</param>
        public override void SaveRemoteHtmlToCache(Stream stream)
        {
            if (HttpContext.Current.Cache[_cacheToken] != null)
            {
                HttpContext.Current.Cache.Remove(_cacheToken);
            }
            using (var reader = new StreamReader(stream))
            {
                HttpContext.Current.Cache.Insert(_cacheToken, reader.ReadToEnd(), null, DateTime.Now.AddMinutes(GetCacheMinutes()), Cache.NoSlidingExpiration);
            }
        }

        /// <summary>
        /// Gets the best available cached response (up-to-date or not)
        /// </summary>
        /// <returns></returns>
        public override string ReadHtmlFromCache()
        {
            return (HttpContext.Current.Cache[_cacheToken] != null) ? HttpContext.Current.Cache[_cacheToken].ToString() : String.Empty;
        }
    }
}