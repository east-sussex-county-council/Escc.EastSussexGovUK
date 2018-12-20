using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using Escc.EastSussexGovUK.Features;

namespace Escc.EastSussexGovUK.Mvc
{
    /// <summary>
    /// Get breadcrumb trail data from the web.config file
    /// </summary>
    public class BreadcrumbTrailFromConfig : IBreadcrumbProvider
    {
        private readonly Uri _requestUrl;

        /// <summary>
        /// Creates a new instance of <see cref="BreadcrumbTrailFromConfig"/>
        /// </summary>
        /// <param name="requestUrl">The URL of the request the breadcrumb trail should be generated for</param>
        public BreadcrumbTrailFromConfig(Uri requestUrl)
        {
            _requestUrl = requestUrl ?? throw new ArgumentNullException(nameof(requestUrl));
        }

        /// <summary>
        /// Gets the data for a breadcrumb trail, indexed by the display text with the URL to link to as the value
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> BuildTrail()
        {
            var breadcrumbTrail = ConfigurationManager.GetSection("Escc.EastSussexGovUK/BreadcrumbTrail") as NameValueCollection;
            if (breadcrumbTrail == null) breadcrumbTrail = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/BreadcrumbTrail") as NameValueCollection;
            if (breadcrumbTrail == null) breadcrumbTrail = new NameValueCollection();

            // Try to remove the link to the current page
            if (breadcrumbTrail.Count > 0)
            {
                var lastKey = breadcrumbTrail.AllKeys[breadcrumbTrail.AllKeys.Length - 1];
                if ((breadcrumbTrail[lastKey] == _requestUrl.AbsolutePath && String.IsNullOrEmpty(_requestUrl.Query))
                    || _requestUrl.ToString().EndsWith("/", StringComparison.Ordinal))
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
