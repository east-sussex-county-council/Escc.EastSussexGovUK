using Escc.EastSussexGovUK.Features;

namespace Escc.EastSussexGovUK.Mvc.Views.EastSussexGovUK
{
    /// <summary>
    /// Data model to configure an instance of <see cref="MasterPageControl"/>
    /// </summary>
    public class MasterPageControlData
    {
        /// <summary>
        /// Gets or sets the key identifying the control to load.
        /// </summary>
        public string Control { get; set; }

        /// <summary>
        /// Gets or sets the provider for working out the current context within the site's information architecture.
        /// </summary>
        public IBreadcrumbProvider BreadcrumbProvider { get; set; }
    }
}