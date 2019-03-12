using System;
using System.Collections.Generic;
using Escc.EastSussexGovUK.Views;
using Microsoft.Extensions.Options;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// Selects most appropriate MVC layout based on querystring or URL path
    /// </summary>
    public class MvcViewSelector : ViewSelector, IViewSelector
    {
        private Dictionary<string, string> _views = new Dictionary<string, string>();
        
        /// <summary>
        /// Creates a new <see cref="MvcViewSelector"/>
        /// </summary>
        /// <param name="options">Paths to views read from the configuration system</param>
        public MvcViewSelector(IOptions<MvcSettings> options)
        {
            _views.Add("DesktopMvcLayout", options.Value.DesktopMvcLayout);
            _views.Add("FullScreenMvcLayout", options.Value.FullScreenMvcLayout);
            _views.Add("PlainMvcLayout", options.Value.PlainMvcLayout);
        }

        /// <summary>
        /// Selects most appropriate MVC layout based on querystring or URL path
        /// </summary>
        /// <param name="forUrl">The URL of the page to select a view for.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <returns></returns>
        public string SelectView(Uri forUrl, string userAgent)
        {
            var preferredView = base.SelectView(forUrl, userAgent, _views, ViewEngine.Mvc);
            if (String.IsNullOrEmpty(preferredView))
            {
                throw new Exception("The path to the selected MVC layout was not specified. Set the path in the Escc.EastSussexGovUK:Mvc:'" + preferredView + "MvcLayout' node in configuration.");
            }
            else return preferredView;
        }


        /// <summary>
        /// Gets the current MVC layout type based on its path
        /// </summary>
        /// <param name="currentView">Path of current master page or MVC layout.</param>
        /// <returns></returns>
        public EsccWebsiteView CurrentViewIs(string currentView)
        {
            if (CurrentViewIs(currentView, EsccWebsiteView.Desktop)) return EsccWebsiteView.Desktop;
            if (CurrentViewIs(currentView, EsccWebsiteView.FullScreen)) return EsccWebsiteView.FullScreen;
            if (CurrentViewIs(currentView, EsccWebsiteView.Plain)) return EsccWebsiteView.Plain;
            return EsccWebsiteView.Unknown;
        }

        /// <summary>
        /// Determines whether the currently selected master page or MVC layout is an instance of the given view.
        /// </summary>
        /// <param name="currentView">Path of current master page or MVC layout.</param>
        /// <param name="view">Check whether this view is currently selected.</param>
        /// <returns></returns>
        public bool CurrentViewIs(string currentView, EsccWebsiteView view)
        {
            return IsMasterPageInGroup(currentView, view + "MvcLayout", _views);
        }
    }
}
