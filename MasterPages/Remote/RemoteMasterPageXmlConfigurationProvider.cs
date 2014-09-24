using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Remote
{
    /// <summary>
    /// Gets configuration for the remote master page from web.config
    /// </summary>
    public class RemoteMasterPageXmlConfigurationProvider : IRemoteMasterPageConfigurationProvider
    {
        /// <summary>
        /// Gets the allowed origins for CORS requests.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> CorsAllowedOrigins()
        {
            var config = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/RemoteMasterPage") as NameValueCollection;
            if (config != null && !String.IsNullOrEmpty(config["CorsAllowedOrigins"]))
            {
                return new List<string>(config["CorsAllowedOrigins"].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
            }
            return new string[0];
        }
    }
}