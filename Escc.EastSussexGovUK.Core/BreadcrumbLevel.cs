using System;
using System.Diagnostics.CodeAnalysis;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// A level within the the hierarchy of the site
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BreadcrumbLevel
    {
        /// <summary>
        /// The display name of this section of the site
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The base URL for this section of the site
        /// </summary>
        public Uri Url { get; set; }
    }
}
