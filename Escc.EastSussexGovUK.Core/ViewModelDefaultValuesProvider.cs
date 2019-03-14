using System;
using System.Collections.Generic;
using System.Text;
using Escc.EastSussexGovUK.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// Provides the default values that should be applied to all view models for pages using the EastSussexGovUK template
    /// </summary>
    public class ViewModelDefaultValuesProvider : IViewModelDefaultValuesProvider
    {
        /// <summary>
        /// Creates a new <see cref="ViewModelDefaultValuesProvider"/>
        /// </summary>
        /// <param name="metadata">The default sitewide metadata, usually loaded from configuration</param>
        /// <param name="breadcrumb">The breadcrumb provider which identifies the context within the site hierarchy</param>
        /// <param name="httpContextAccessor">Access to the current request URL</param>
        public ViewModelDefaultValuesProvider(IOptions<Metadata.Metadata> metadata, IBreadcrumbProvider breadcrumb, IHttpContextAccessor httpContextAccessor)
        {
            Metadata = metadata?.Value;
            Breadcrumb = breadcrumb;

            var request = httpContextAccessor?.HttpContext?.Request ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            Metadata.CanonicalPageUrl = new Uri(request.GetDisplayUrl());
        }

        /// <summary>
        /// Gets the default sitewide metadata, usually loaded from configuration
        /// </summary>
        public Metadata.Metadata Metadata { get; }

        /// <summary>
        /// Gets the default breadcrumb provider which identifies the context within the site hierarchy
        /// </summary>
        public IBreadcrumbProvider Breadcrumb { get; }
    }
}
