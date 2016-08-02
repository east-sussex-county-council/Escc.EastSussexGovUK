
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Remote
{
    /// <summary>
    /// A base class to define methods of caching remote template elements
    /// </summary>
    public abstract class RemoteMasterPageCacheProviderBase
    {
        private readonly string _control;
        private readonly string _selectedSection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteMasterPageCacheProviderBase"/> class.
        /// </summary>
        /// <param name="control">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        protected RemoteMasterPageCacheProviderBase(string control, string selectedSection)
        {
            _control = control;
            _selectedSection = selectedSection;
            SupportsAsync = false;
        }

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
                ConfigurationSettings = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/RemoteMasterPage") as NameValueCollection;
                if (ConfigurationSettings == null)
                {
                    throw new ConfigurationErrorsException("web.config section not found: <EsccWebTeam.EastSussexGovUK><RemoteMasterPage /></EsccWebTeam.EastSussexGovUK>");
                }
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether a cached version exists.
        /// </summary>
        /// <value><c>true</c> if a cached version exists; otherwise, <c>false</c>.</value>
        public bool CachedVersionExists { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether cached version is newer than the cache threshold.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if cached version is fresh; otherwise, <c>false</c>.
        /// </value>
        public bool CachedVersionIsFresh { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this cache provider supports caching on an asynchronous thread.
        /// </summary>
        /// <value><c>true</c> if the provider supports async; otherwise, <c>false</c>.</value>
        public bool SupportsAsync { get; set; }

        /// <summary>
        /// Gets how many minutes the remote template elements should be cached for.
        /// </summary>
        /// <returns></returns>
        protected int GetCacheMinutes()
        {
            EnsureConfigurationSettings();

            if (String.IsNullOrEmpty(ConfigurationSettings["CacheMinutes"])) throw new ConfigurationErrorsException("web.config entry not found: <EsccWebTeam.EastSussexGovUK><RemoteMasterPage><add key=\"CacheMinutes\" value=\"integer\" /></RemoteMasterPage></EsccWebTeam.EastSussexGovUK>");
            try
            {
                return Int32.Parse(ConfigurationSettings["CacheMinutes"]);
            }
            catch (FormatException ex)
            {
                throw new ConfigurationErrorsException("web.config entry is not an integer: <EsccWebTeam.EastSussexGovUK><RemoteMasterPage><add key=\"CacheMinutes\" value=\"integer\" /></RemoteMasterPage></EsccWebTeam.EastSussexGovUK>", ex);
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
        /// <returns></returns>
        protected string GetCacheToken()
        {
            // Sanitise selected section and use as a token, so we get a different cached version for each section if appropriate
            var sanitisedSection = String.IsNullOrEmpty(_selectedSection) ? String.Empty : "." + Regex.Replace(_selectedSection.ToLower(CultureInfo.CurrentCulture), "[^a-z]", String.Empty);

            // Add the user's text size to the token, because it affects the HTML of the header (bigger / smaller links are added / removed)
            var siteContext = new EastSussexGovUKContext();
            var textSize = (siteContext.TextSize > 1) ? ".textsize" + siteContext.TextSize : String.Empty;

            // If user is on library catalogue PC, add that to token so that they get a separate cache
            var libraryUser = siteContext.UserIsLibraryCatalogue ? ".librarycatalogue" : String.Empty;

            // Add application path to the token, because it affects the path to /masterpages
            var sanitisedPath = "." + Regex.Replace(HttpRuntime.AppDomainAppVirtualPath.ToLower(CultureInfo.CurrentCulture), "[^a-z]", String.Empty);

            return _control + sanitisedSection + textSize + libraryUser + sanitisedPath;
        }

        /// <summary>
        /// Saves the remote HTML to the cache.
        /// </summary>
        /// <param name="stream">The HTML stream.</param>
        public abstract void SaveRemoteHtmlToCache(Stream stream);

        /// <summary>
        /// Gets the best available cached response (up-to-date or not)
        /// </summary>
        /// <returns></returns>
        public abstract string ReadHtmlFromCache();
    }
}