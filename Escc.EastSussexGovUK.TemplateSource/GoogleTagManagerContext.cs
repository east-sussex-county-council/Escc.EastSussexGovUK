using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;

namespace Escc.EastSussexGovUK.TemplateSource
{
    /// <summary>
    /// Information about the current instance of Google Tag Manager
    /// </summary>
    public class GoogleTagManagerContext
    {
        /// <summary>
        /// Gets the id for the Google Tag Manager container
        /// </summary>
        /// <value>
        /// The Google Tag Manager container id
        /// </value>
        public string GoogleTagManagerContainerId
        {
            get
            {
                // Get the host name, which might be passed in the query string if we are serving up part of the remote template
                var host = HttpContext.Current.Request.Url.Host;
                if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["host"])) host = HttpContext.Current.Request.QueryString["host"];

                // Get from cache if present, or get from config
                if (HttpContext.Current.Cache["GoogleTagManager." + host] != null)
                {
                    return HttpContext.Current.Cache["GoogleTagManager." + host].ToString();
                }
                else
                {
                    // Compare the host name to rules to select the correct tag manager id
                    var rules = ConfigurationManager.GetSection("Escc.EastSussexGovUK/GoogleTagManagerIdRules") as NameValueCollection;
                    if (rules == null) rules = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/GoogleTagManagerIdRules") as NameValueCollection;
                    if (rules == null) return String.Empty;

                    var googleTagManagerId = new GoogleTagManagerContainerIdSelector();
                    var containerId = googleTagManagerId.SelectContainerId(host, rules);

                    // Add to cache and return
                    HttpContext.Current.Cache.Insert("GoogleTagManager." + host, containerId);
                    return containerId;
                }
            }
        }
    }
}