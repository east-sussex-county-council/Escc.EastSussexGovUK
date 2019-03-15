using System;
using Escc.EastSussexGovUK.Features;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// Provides the default values that should be applied to all view models for pages using the EastSussexGovUK template
    /// </summary>
    public interface IViewModelDefaultValuesProvider
    {
        /// <summary>
        /// Gets the default breadcrumb provider which identifies the context within the site hierarchy
        /// </summary>
        IBreadcrumbProvider Breadcrumb { get; }

        /// <summary>
        /// Gets the default sitewide metadata, usually loaded from configuration
        /// </summary>
        Metadata.Metadata Metadata { get; }

        /// <summary>
        /// Gets or sets the base URL to use for sitewide client-side files such as CSS and JavaScript that are not part of the current application
        /// </summary>
        Uri ClientFileBaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the version string to append to client-side files such as CSS and JavaScript to ensure that previously cached versions are not returned
        /// </summary>
        string ClientFileVersion { get; set; }
    }
}