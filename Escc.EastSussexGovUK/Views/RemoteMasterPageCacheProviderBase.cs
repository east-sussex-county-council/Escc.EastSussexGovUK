using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace Escc.EastSussexGovUK.Views
{
    /// <summary>
    /// A base class to define methods of caching remote template elements
    /// </summary>
    public abstract class RemoteMasterPageCacheProviderBase
    {
        /// <summary>
        /// Gets or sets the remote master page configuration settings.
        /// </summary>
        /// <value>The configuration settings.</value>
        protected NameValueCollection ConfigurationSettings { get; private set; }

        /// <summary>
        /// Loads the configuration settings from web.config
        /// </summary>
        protected void EnsureConfigurationSettings()
        {
            if (ConfigurationSettings == null)
            {
                ConfigurationSettings = ConfigurationManager.GetSection("Escc.EastSussexGovUK/RemoteMasterPage") as NameValueCollection;
                if (ConfigurationSettings == null) ConfigurationSettings = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/RemoteMasterPage") as NameValueCollection;
                if (ConfigurationSettings == null)
                {
                    throw new ConfigurationErrorsException("web.config section not found: <Escc.EastSussexGovUK><RemoteMasterPage /></Escc.EastSussexGovUK>");
                }
            }
        }


        /// <summary>
        /// Gets whether a cached version exists.
        /// </summary>
        /// <param name="controlId">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        /// <value><c>true</c> if a cached version exists; otherwise, <c>false</c>.</value>
        public abstract bool CachedVersionExists(string controlId, string selectedSection, int textSize, bool isLibraryCatalogueRequest);

        /// <summary>
        /// Gets a value indicating whether cached version is newer than the cache threshold.
        /// </summary>
        /// <param name="controlId">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        /// <value>
        /// 	<c>true</c> if cached version is fresh; otherwise, <c>false</c>.
        /// </value>
        public abstract bool CachedVersionIsFresh(string controlId, string selectedSection, int textSize, bool isLibraryCatalogueRequest);

        /// <summary>
        /// Gets how many minutes the remote template elements should be cached for.
        /// </summary>
        /// <returns></returns>
        protected int GetCacheMinutes()
        {
            EnsureConfigurationSettings();

            if (String.IsNullOrEmpty(ConfigurationSettings["CacheMinutes"])) throw new ConfigurationErrorsException("web.config entry not found: <Escc.EastSussexGovUK><RemoteMasterPage><add key=\"CacheMinutes\" value=\"integer\" /></RemoteMasterPage></Escc.EastSussexGovUK>");
            try
            {
                return Int32.Parse(ConfigurationSettings["CacheMinutes"]);
            }
            catch (FormatException ex)
            {
                throw new ConfigurationErrorsException("web.config entry is not an integer: <Escc.EastSussexGovUK><RemoteMasterPage><add key=\"CacheMinutes\" value=\"integer\" /></RemoteMasterPage></Escc.EastSussexGovUK>", ex);
            }
        }

        /// <summary>
        /// Gets the time before which cached responses are not fresh enough.
        /// </summary>
        /// <returns></returns>
        protected DateTime GetCacheThreshold()
        {
            int cacheMinutes = GetCacheMinutes();
            var cacheThreshold = DateTime.UtcNow.Subtract(new TimeSpan(0, cacheMinutes, 0));
            return cacheThreshold;
        }

        /// <summary>
        /// Gets a token which identifies the unique fragment of HTML to be stored in the cache.
        /// </summary>
        /// <param name="controlId">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        /// <returns></returns>
        protected string GetCacheToken(string controlId, string selectedSection, int textSize, bool isLibraryCatalogueRequest)
        {
            // Sanitise selected section and use as a token, so we get a different cached version for each section if appropriate
            var sanitisedSection = String.IsNullOrEmpty(selectedSection) ? String.Empty : "." + Regex.Replace(selectedSection.ToLower(CultureInfo.CurrentCulture), "[^a-z]", String.Empty);

            // Add the user's text size to the token, because it affects the HTML of the header (bigger / smaller links are added / removed)
            var textSizeToken = (textSize > 1) ? ".textsize" + textSize : String.Empty;

            // If user is on library catalogue PC, add that to token so that they get a separate cache
            var libraryUser = isLibraryCatalogueRequest ? ".librarycatalogue" : String.Empty;

            // Add application path to the token, because it affects the path to /masterpages
            var sanitisedPath = "." + Regex.Replace(HttpRuntime.AppDomainAppVirtualPath.ToLower(CultureInfo.CurrentCulture), "[^a-z]", String.Empty);

            return controlId + sanitisedSection + textSizeToken + libraryUser + sanitisedPath;
        }

        /// <summary>
        /// Saves the remote HTML to the cache.
        /// </summary>
        /// <param name="controlId">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        /// <param name="stream">The HTML stream.</param>
        public abstract void SaveRemoteHtmlToCache(string controlId, string selectedSection, int textSize, bool isLibraryCatalogueRequest, Stream stream);

        /// <summary>
        /// Gets the best available cached response (up-to-date or not)
        /// </summary>
        /// <param name="controlId">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        /// <returns></returns>
        public abstract string ReadHtmlFromCache(string controlId, string selectedSection, int textSize, bool isLibraryCatalogueRequest);
    }
}