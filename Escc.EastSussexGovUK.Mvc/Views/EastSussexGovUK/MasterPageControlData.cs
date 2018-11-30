using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Views;

namespace Escc.EastSussexGovUK.Mvc.Views.EastSussexGovUK
{
    /// <summary>
    /// Data model to configure an instance of <c>_MasterPageControl.cshtml</c>
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

        /// <summary>
        /// Gets or sets the provider for getting the HTML of the requested control
        /// </summary>
        public IHtmlControlProvider HtmlControlProvider { get; set; }

        /// <summary>
        /// Gets or sets the current setting for the site's text size feature.
        /// </summary>
        public int TextSize { get; set; }

        /// <summary>
        /// <c>true</c> if the request is from a public catalogue machine in a library
        /// </summary>
        public bool IsLibraryCatalogueRequest { get; set; }

    }
}