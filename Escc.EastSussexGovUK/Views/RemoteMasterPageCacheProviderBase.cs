﻿using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.Views
{
    /// <summary>
    /// A base class to define methods of caching remote template elements
    /// </summary>
    public abstract class RemoteMasterPageCacheProviderBase : IRemoteMasterPageCacheProvider
    {
        /// <summary>
        /// Gets or sets the duration to cache the master page elements for
        /// </summary>
        public TimeSpan CacheDuration { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="RemoteMasterPageCacheProviderBase"/> from a child class
        /// </summary>
        protected RemoteMasterPageCacheProviderBase()
        {
        }

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
        public abstract bool CachedVersionExists(string applicationId, string hostName, string controlId, string selectedSection, int textSize, bool isLibraryCatalogueRequest);

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
        public abstract bool CachedVersionIsFresh(string applicationId, string hostName, string controlId, string selectedSection, int textSize, bool isLibraryCatalogueRequest);

        /// <summary>
        /// Gets the time before which cached responses are not fresh enough.
        /// </summary>
        /// <returns></returns>
        protected DateTime GetCacheThreshold()
        {
            return DateTime.UtcNow.Subtract(CacheDuration);
        }

        /// <summary>
        /// Gets a token which identifies the unique fragment of HTML to be stored in the cache.
        /// </summary>
        /// <param name="applicationId">A string which identifies the application making the request</param>
        /// <param name="hostName">The host name of the requesting application</param>
        /// <param name="controlId">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        /// <returns></returns>
        protected string GetCacheToken(string applicationId, string hostName, string controlId, string selectedSection, int textSize, bool isLibraryCatalogueRequest)
        {
            // Sanitise selected section and use as a token, so we get a different cached version for each section if appropriate
            var sanitisedSection = String.IsNullOrEmpty(selectedSection) ? String.Empty : "." + Regex.Replace(selectedSection.ToLower(CultureInfo.CurrentCulture), "[^a-z]", String.Empty);

            // Add the user's text size to the token, because it affects the HTML of the header (bigger / smaller links are added / removed)
            var textSizeToken = (textSize > 1) ? ".textsize" + textSize : String.Empty;

            // If user is on library catalogue PC, add that to token so that they get a separate cache
            var libraryUser = isLibraryCatalogueRequest ? ".librarycatalogue" : String.Empty;

            // Add application path to the token, because it affects the path to /masterpages
            var sanitisedPath = "." + Regex.Replace(applicationId.ToLower(CultureInfo.CurrentCulture), "[^a-z]", String.Empty);

            return hostName + controlId + sanitisedSection + textSizeToken + libraryUser + sanitisedPath;
        }

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
        public abstract void SaveRemoteHtmlToCache(string applicationId, string hostName, string controlId, string selectedSection, int textSize, bool isLibraryCatalogueRequest, string html);

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
        public abstract string ReadHtmlFromCache(string applicationId, string hostName, string controlId, string selectedSection, int textSize, bool isLibraryCatalogueRequest);
    }
}