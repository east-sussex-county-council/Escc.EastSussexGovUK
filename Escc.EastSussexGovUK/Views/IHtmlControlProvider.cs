using System;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Features;

namespace Escc.EastSussexGovUK.Views
{
    /// <summary>
    /// A provider for getting fragments of HTML used to build up the sitewide template
    /// </summary>
    public interface IHtmlControlProvider
    {
        /// <summary>
        /// Fetches the master page control HTML.
        /// </summary>
        /// <param name="applicationPath">The virtual URL to the web server application root, starting and ending with a /</param>
        /// <param name="forUrl">The page to request the control for (usually the current page)</param>
        /// <param name="controlId">A key identifying the control to cache.</param>
        /// <param name="breadcrumbProvider">The provider for working out the current context within the site's information architecture.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        Task<string> FetchHtmlForControl(string applicationPath, Uri forUrl, string controlId, IBreadcrumbProvider breadcrumbProvider, int textSize, bool isLibraryCatalogueRequest);
    }
}