using System;
using Escc.EastSussexGovUK.Views;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// Select the most appropriate MVC layout to apply for a request
    /// </summary>
    public interface IViewSelector
    {
        /// <summary>
        /// Gets the current MVC layout type based on its path
        /// </summary>
        /// <param name="currentView">Path of current MVC layout.</param>
        /// <returns></returns>
        EsccWebsiteView CurrentViewIs(string currentView);

        /// <summary>
        /// Determines whether the currently selected MVC layout is an instance of the given view.
        /// </summary>
        /// <param name="currentView">Path of current MVC layout.</param>
        /// <param name="view">Check whether this view is currently selected.</param>
        /// <returns></returns>
        bool CurrentViewIs(string currentView, EsccWebsiteView view);

        /// <summary>
        /// Selects most appropriate master page or MVC layout based on querystring or URL path
        /// </summary>
        /// <param name="forUrl">The URL of the page to select a view for.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <returns></returns>
        string SelectView(Uri forUrl, string userAgent);
    }
}