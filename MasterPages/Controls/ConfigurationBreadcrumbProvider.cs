using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Get breadcrumb trail data from the web.config file
    /// </summary>
    public class ConfigurationBreadcrumbProvider : IBreadcrumbProvider
    {
        /// <summary>
        /// Gets the data for a breadcrumb trail, indexed by the display text with the URL to link to as the value
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> BuildTrail()
        {
            var breadcrumbTrail = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/BreadcrumbTrail") as NameValueCollection;
            if (breadcrumbTrail == null) breadcrumbTrail = new NameValueCollection();

            // Try to remove the link to the current page
            if (breadcrumbTrail.Count > 0)
            {
                var lastKey = breadcrumbTrail.AllKeys[breadcrumbTrail.AllKeys.Length - 1];
                if ((breadcrumbTrail[lastKey] == HttpContext.Current.Request.Url.AbsolutePath && String.IsNullOrEmpty(HttpContext.Current.Request.QueryString.ToString()))
                    || HttpContext.Current.Request.Url.ToString().EndsWith("/", StringComparison.Ordinal))
                {
                    // Start with a new copy of the NameValueCollection as the one from web.config is read only
                    breadcrumbTrail = new NameValueCollection(breadcrumbTrail);
                    breadcrumbTrail[lastKey] = String.Empty;
                }
            }

            // Convert to type which can be returned as an interface
            var dictionary = new Dictionary<string, string>();
            foreach (string key in breadcrumbTrail.Keys)
            {
                dictionary.Add(key, breadcrumbTrail[key]);
            }

            return dictionary;
        }
    }
}
