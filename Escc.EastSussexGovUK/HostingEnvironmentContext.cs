using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.UI;

namespace Escc.EastSussexGovUK
{
    /// <summary>
    /// In development and production environments the site may span several domains. Get information about the current environment.
    /// </summary>
    public class HostingEnvironmentContext : IHostingEnvironmentContext
    {
        #region Shortcuts to configuration settings
        private NameValueCollection _generalSettings;
        private readonly Uri _currentUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="HostingEnvironmentContext"/> class.
        /// </summary>
        /// <param name="currentUrl">The current URL. If <c>null</c>, <c>HttpContext.Current.Request.Url</c> will be used if available.</param>
        public HostingEnvironmentContext(Uri currentUrl = null)
        {
            if (currentUrl != null)
            {
                _currentUrl = currentUrl;
            }
            else
            {
                if (HttpContext.Current?.Request?.Url != null) _currentUrl = HttpContext.Current.Request.Url;
            }
        }

        /// <summary>
        /// Gets the settings for the site and its template.
        /// </summary>
        /// <value>The settings.</value>
        private NameValueCollection Settings
        {
            get
            {
                if (this._generalSettings == null)
                {
                    this._generalSettings = ConfigurationManager.GetSection("Escc.EastSussexGovUK/GeneralSettings") as NameValueCollection;
                    if (this._generalSettings == null) this._generalSettings = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/GeneralSettings") as NameValueCollection;
                    if (this._generalSettings == null) this._generalSettings = new NameValueCollection();
                }
                return this._generalSettings;
            }
        }


        #endregion // Shortcuts to configuration settings

        #region Shortcuts to information about the current request

        private Uri baseUrl;

        /// <summary>
        /// Gets the base URL, if any, to prefix links to the website with. Intended for sub-domains to use when linking back to the main site.
        /// </summary>
        /// <value>The base URL.</value>
        public Uri BaseUrl
        {
            get
            {
                if (this.baseUrl != null) return this.baseUrl;

                if (this.Settings == null) return null;

                if (!String.IsNullOrEmpty(this.Settings["BaseUrl"]))
                {
                    var rawBaseUrl = this.Settings["BaseUrl"];
                    if (_currentUrl != null) rawBaseUrl = rawBaseUrl.Replace("{hostname}", _currentUrl.Authority);
                    this.baseUrl = new Uri(rawBaseUrl);
                    return this.baseUrl;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets whether the current request is for a publicly available URL.
        /// </summary>
        /// <value><c>true</c> if URL is public or unknown; otherwise, <c>false</c>.</value>
        public bool IsPublicUrl
        {
            get
            {
                return (_currentUrl == null || _currentUrl.Host.IndexOf('.') > -1 && _currentUrl.Host.IndexOf("escc.gov", StringComparison.InvariantCulture) == -1);
            }
        }


        #endregion // Shortcuts to information about the current request
    }
}