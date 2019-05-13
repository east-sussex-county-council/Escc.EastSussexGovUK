using System;

namespace Escc.EastSussexGovUK.Views
{
    /// <summary>
    /// A method of caching remote template elements
    /// </summary>

    public interface IRemoteMasterPageCacheProvider
    {
        /// <summary>
        /// Gets or sets the duration to cache the master page elements for
        /// </summary>
        TimeSpan CacheDuration { get; set; }

        /// <summary>
        /// Gets whether a cached version exists.
        /// </summary>
        /// <param name="applicationId">A string which identifies the application making the request</param>
        /// <param name="hostName">The host name of the requesting application</param>
        /// <param name="controlId">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        /// <value><c>true</c> if a cached version exists; otherwise, <c>false</c>.</value>
        bool CachedVersionExists(string applicationId, string hostName, string controlId, string selectedSection, int textSize, bool isLibraryCatalogueRequest);

        /// <summary>
        /// Gets a value indicating whether cached version is newer than the cache threshold.
        /// </summary>
        /// <param name="applicationId">A string which identifies the application making the request</param>
        /// <param name="hostName">The host name of the requesting application</param>
        /// <param name="controlId">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        /// <value>
        /// 	<c>true</c> if cached version is fresh; otherwise, <c>false</c>.
        /// </value>
        bool CachedVersionIsFresh(string applicationId, string hostName, string controlId, string selectedSection, int textSize, bool isLibraryCatalogueRequest);

        /// <summary>
        /// Gets the best available cached response (up-to-date or not)
        /// </summary>
        /// <param name="applicationId">A string which identifies the application making the request</param>
        /// <param name="hostName">The host name of the requesting application</param>
        /// <param name="controlId">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        /// <returns></returns>
        string ReadHtmlFromCache(string applicationId, string hostName, string controlId, string selectedSection, int textSize, bool isLibraryCatalogueRequest);

        /// <summary>
        /// Saves the remote HTML to the cache.
        /// </summary>
        /// <param name="applicationId">A string which identifies the application making the request</param>
        /// <param name="hostName">The host name of the requesting application</param>
        /// <param name="controlId">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        /// <param name="html">The HTML.</param>
        void SaveRemoteHtmlToCache(string applicationId, string hostName, string controlId, string selectedSection, int textSize, bool isLibraryCatalogueRequest, string html);
    }
}