using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escc.EastSussexGovUK.WebForms
{
    /// <summary>
    /// Gets the settings for fetching HTML for the remote master page from web.config
    /// </summary>
    public class RemoteMasterPageSettingsFromConfig 
    {
        private NameValueCollection _remoteMasterPageSettings;

        /// <summary>
        /// Loads the configuration settings from web.config
        /// </summary>
        private void EnsureRemoteMasterPageSettings(bool isRequired)
        {
            if (_remoteMasterPageSettings == null)
            {
                _remoteMasterPageSettings = ConfigurationManager.GetSection("Escc.EastSussexGovUK/RemoteMasterPage") as NameValueCollection;
                if (_remoteMasterPageSettings == null) _remoteMasterPageSettings = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/RemoteMasterPage") as NameValueCollection;
                if (_remoteMasterPageSettings == null && isRequired)
                {
                    throw new ConfigurationErrorsException("web.config section not found: <Escc.EastSussexGovUK><RemoteMasterPage /></Escc.EastSussexGovUK>");
                }
            }
        }

        /// <summary>
        /// Gets how many minutes the remote template elements should be cached for. Defaults to 60 minutes.
        /// </summary>
        /// <returns></returns>
        public int CacheTimeout()
        {
            EnsureRemoteMasterPageSettings(false);

            int cacheMinutes = 60;
            if (!String.IsNullOrEmpty(_remoteMasterPageSettings["CacheMinutes"]))
            {
                Int32.TryParse(_remoteMasterPageSettings["CacheMinutes"], out cacheMinutes);
            }
            return cacheMinutes;
        }

        /// <summary>
        /// Gets the URL from which to request the remote master page controls, with a {0} token to be replaced by the key for the control
        /// </summary>
        /// <returns></returns>
        public Uri MasterPageControlUrl()
        {
            EnsureRemoteMasterPageSettings(true);

            if (String.IsNullOrEmpty(_remoteMasterPageSettings["MasterPageControlUrl"]))
            {
                throw new ConfigurationErrorsException("<Escc.EastSussexGovUK><RemoteMasterPage><add key=\"MasterPageControlUrl\" /></RemoteMasterPage></Escc.EastSussexGovUK> is not defined in web.config");
            }

            return new Uri(_remoteMasterPageSettings["MasterPageControlUrl"], UriKind.RelativeOrAbsolute);
        }
    }
}
