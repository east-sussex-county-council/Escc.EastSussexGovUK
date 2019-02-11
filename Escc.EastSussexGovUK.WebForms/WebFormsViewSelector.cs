using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Escc.EastSussexGovUK.Views;

namespace Escc.EastSussexGovUK.WebForms
{
    /// <summary>
    /// Selects most appropriate master page based on querystring or URL path
    /// </summary>
    public class WebFormsViewSelector : ViewSelector
    {
        /// <summary>
        /// Selects most appropriate master page based on querystring or URL path
        /// </summary>
        /// <param name="forUrl">The URL of the page to select a view for.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <returns></returns>
        public string SelectView(Uri forUrl, string userAgent)
        {
            var generalSettings = ConfigurationManager.GetSection("Escc.EastSussexGovUK/GeneralSettings") as NameValueCollection;
            if (generalSettings == null) generalSettings = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/GeneralSettings") as NameValueCollection;

            return base.SelectView(forUrl, userAgent, generalSettings?.AllKeys.ToDictionary(k => k, k => generalSettings[k]), ViewEngine.WebForms);
        }


        /// <summary>
        /// Gets the current master page based on its path
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

            return IsMasterPageInGroup(currentView, view + "MasterPage", generalSettings?.AllKeys.ToDictionary(k => k, k => generalSettings[k]));
        }
    }
}
