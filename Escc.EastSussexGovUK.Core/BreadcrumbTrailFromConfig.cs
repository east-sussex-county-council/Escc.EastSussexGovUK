using System;
using System.Collections.Generic;
using Escc.EastSussexGovUK.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// Get breadcrumb trail data from the configuration system
    /// </summary>
    public class BreadcrumbTrailFromConfig : IBreadcrumbProvider
    {
        private readonly Uri _requestUrl;
        private readonly BreadcrumbSettings _breadcrumbSettings;

        /// <summary>
        /// Creates a new instance of <see cref="BreadcrumbTrailFromConfig"/>
        /// </summary>
        /// <param name="breadcrumbSettings">The raw breadcrumb data read from a configuration provider</param>
        /// <param name="httpContextAccessor">A way to get the HttpContext, used to access the URL of the request the breadcrumb trail should be generated for</param>
        public BreadcrumbTrailFromConfig(IOptions<BreadcrumbSettings> breadcrumbSettings, IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor?.HttpContext?.Request == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            _breadcrumbSettings = breadcrumbSettings?.Value ?? throw new ArgumentNullException(nameof(breadcrumbSettings));
            _requestUrl = new Uri(httpContextAccessor.HttpContext.Request.GetDisplayUrl());
        }

        
        /// <summary>
        /// Gets the data for a breadcrumb trail, indexed by the display text with the URL to link to as the value
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> BuildTrail()
        {
            var result = new Dictionary<string, string>();

            for (var i = 0; i < _breadcrumbSettings.Count; i++)
            {
                // Try to remove the link to the current page
                if (i == _breadcrumbSettings.Count -1 && (
                    (_breadcrumbSettings[i].Url?.ToString() == _requestUrl.AbsolutePath && String.IsNullOrEmpty(_requestUrl.Query)) ||
                    _requestUrl.ToString().EndsWith("/", StringComparison.Ordinal)
                    ))
                {
                    result.Add(_breadcrumbSettings[i].Name, string.Empty);
                }
                else
                {
                    result.Add(_breadcrumbSettings[i].Name, _breadcrumbSettings[i].Url?.ToString());
                }
            }

            return result;
        }
    }
}
