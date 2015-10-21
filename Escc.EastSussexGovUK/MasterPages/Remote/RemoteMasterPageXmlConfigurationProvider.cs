using EsccWebTeam.Data.Web;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Remote
{
    /// <summary>
    /// Gets configuration for the remote master page from web.config
    /// </summary>
    [Obsolete("Use EsccWebTeam.Data.Web.ConfigurationCorsAllowedOriginsProvider")]
    public class RemoteMasterPageXmlConfigurationProvider : ConfigurationCorsAllowedOriginsProvider
    {
    }
}