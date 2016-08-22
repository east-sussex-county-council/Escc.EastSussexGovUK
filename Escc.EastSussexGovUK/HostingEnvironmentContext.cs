using System;
using System.Collections.Specialized;
using System.Configuration;
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
                    this.baseUrl = new Uri(this.Settings["BaseUrl"]);
                    return this.baseUrl;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets whether the current request is for a publicly available URL.
        /// </summary>
        /// <value><c>true</c> if URL is public; otherwise, <c>false</c>.</value>
        public bool IsPublicUrl
        {
            get
            {
                return (HttpContext.Current.Request.Url.Host.IndexOf('.') > -1 && HttpContext.Current.Request.Url.Host.IndexOf("escc.gov") == -1);
            }
        }


        #endregion // Shortcuts to information about the current request
    }
}