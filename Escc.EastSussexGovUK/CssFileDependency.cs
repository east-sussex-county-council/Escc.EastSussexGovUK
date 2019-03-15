using System;

namespace Escc.EastSussexGovUK
{
    /// <summary>
    /// A CSS file on which an <see cref="IClientDependencySet"/> depends
    /// </summary>
    public class CssFileDependency
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CssFileDependency"/> class.
        /// </summary>
        public CssFileDependency()
        {
            Priority = 100;
        }

        /// <summary>
        /// The alias of the CSS file, used with <see cref="Escc.ClientDependencyFramework"/>
        /// </summary>
        public string CssFileAlias { get; set; }

        /// <summary>
        /// The URL of the CSS file, relative to the root of the Escc.EastSussexGovUK.Template source project
        /// </summary>
        public Uri CssRelativeUrl { get; set; }

        /// <summary>
        /// The alias of the media query, used with <see cref="Escc.ClientDependencyFramework"/>
        /// </summary>
        public string MediaQueryAlias { get; set; }

        /// <summary>
        /// Gets or sets the priority, which defaults to 100.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int Priority { get; set; }
    }
}