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
    }
}