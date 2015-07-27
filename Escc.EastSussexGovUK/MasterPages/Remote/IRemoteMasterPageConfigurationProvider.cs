using System.Collections.Generic;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Remote
{
    /// <summary>
    /// Gets configuration for the remote master page
    /// </summary>
    public interface IRemoteMasterPageConfigurationProvider
    {
        /// <summary>
        /// Gets the allowed origins for CORS requests.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> CorsAllowedOrigins();
    }
}