using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Escc.EastSussexGovUK.Views;

namespace Escc.EastSussexGovUK.Mvc
{
    /// <summary>
    /// Selects most appropriate MVC layout based on querystring or URL path
    /// </summary>
    public class MvcViewSelector : ViewSelector
    {
        /// <summary>
        /// Selects most appropriate MVC layout based on querystring or URL path
        /// </summary>
        /// <param name="forUrl">The URL of the page to select a view for.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <returns></returns>
        public string SelectView(Uri forUrl, string userAgent)
        {
            var generalSettings = ConfigurationManager.GetSection("Escc.EastSussexGovUK/GeneralSettings") as NameValueCollection;
            if (generalSettings == null) generalSettings = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/GeneralSettings") as NameValueCollection;

            var preferredView = base.SelectView(forUrl, userAgent, generalSettings?.AllKeys.ToDictionary(k => k, k => generalSettings[k]), ViewEngine.Mvc);
            if (String.IsNullOrEmpty(preferredView))
            {
                throw new ConfigurationErrorsException("The path to the selected MVC layout was not specified. Set the path in the Escc.EastSussexGovUK/GeneralSettings/add[@key='" + preferredView + "MvcLayout'] element in web.config.");
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
            var generalSettings = ConfigurationManager.GetSection("Escc.EastSussexGovUK/GeneralSettings") as NameValueCollection;
            if (generalSettings == null) generalSettings = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/GeneralSettings") as NameValueCollection;

            return IsMasterPageInGroup(currentView, view + "MvcLayout", generalSettings?.AllKeys.ToDictionary(k => k, k => generalSettings[k]));
        }
    }
}
