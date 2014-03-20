using System.Collections.Generic;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// A data source for discovering the current position within the site's information architecture.
    /// </summary>
    public interface IBreadcrumbProvider
    {
        /// <summary>
        /// Gets the data for a breadcrumb trail, indexed by the display text with the URL to link to as the value
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> BuildTrail();
    }
}
