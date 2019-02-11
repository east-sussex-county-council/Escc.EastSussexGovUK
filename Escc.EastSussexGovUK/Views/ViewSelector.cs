using System;
using System.Collections.Generic;
using System.Web;

namespace Escc.EastSussexGovUK.Views
{
    /// <summary>
    /// Selects most appropriate master page or MVC layout based on querystring or URL path
    /// </summary>
    public abstract class ViewSelector
    {
        /// <summary>
        /// Selects most appropriate master page or MVC layout based on querystring or URL path
        /// </summary>
        /// <param name="forUrl">The URL of the page to select a view for.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <param name="generalSettings">The general settings.</param>
        /// <param name="viewEngine">The view engine to select the view for.</param>
        /// <returns></returns>
        protected string SelectView(Uri forUrl, string userAgent, Dictionary<string,string> generalSettings, ViewEngine viewEngine)
        {
            if (forUrl == null)
            {
                throw new ArgumentNullException(nameof(forUrl));
            }
            var queryString = HttpUtility.ParseQueryString(forUrl.Query);

            // Grab settings from config and set up some defaults
            var configSettingsGroup = viewEngine == ViewEngine.WebForms ? "MasterPage" : "MvcLayout";
            string preferredMasterPage = String.Empty;
            var preferredView = EsccWebsiteView.Desktop;

            // Are we set up to accept user requests for a master page?
            var acceptUserRequest = (generalSettings != null && generalSettings.ContainsKey("MasterPageParameterName") && !String.IsNullOrEmpty(generalSettings["MasterPageParameterName"]));

            // If a master page is requested in the querystring that trumps everything *for this request only*
            if (acceptUserRequest && !String.IsNullOrEmpty(queryString[generalSettings["MasterPageParameterName"]]))
            {
                preferredView = AssignMasterPageFromUserRequest(generalSettings, configSettingsGroup, preferredView, queryString[generalSettings["MasterPageParameterName"]]);
            }

            // We now know which type of master page we want to use, so now get the path to the master page file.
            if (generalSettings != null)
            {
                var configKey = preferredView + configSettingsGroup;
                if (generalSettings.ContainsKey(configKey) && !String.IsNullOrEmpty(generalSettings[configKey]))
                {
                    preferredMasterPage = generalSettings[configKey];
                }
            }

            return preferredMasterPage;
        }

        /// <summary>
        /// Assigns the master page based on a user request
        /// </summary>
        /// <param name="generalSettings">The general settings.</param>
        /// <param name="configSettingsGroup">The configuration settings group.</param>
        /// <param name="esccWebsiteView">The preferred view before this test.</param>
        /// <param name="requestedKey">The key supplied by the user to request the master page.</param>
        /// <returns></returns>
        private static EsccWebsiteView AssignMasterPageFromUserRequest(Dictionary<string,string> generalSettings, string configSettingsGroup, EsccWebsiteView esccWebsiteView, string requestedKey)
        {
            try
            {
                // Get the view requested by the user, and check that we have a master page registered to match it
                var requestedView = (EsccWebsiteView)Enum.Parse(typeof(EsccWebsiteView), requestedKey, true);
                var configKey = requestedView + configSettingsGroup;

                if (generalSettings.ContainsKey(configKey) && !String.IsNullOrEmpty(generalSettings[configKey]))
                {
                    esccWebsiteView = requestedView;
                }
            }
            catch (ArgumentException)
            {
                // Requested key was invalid, ignore it
            }
            return esccWebsiteView;
        }

        /// <summary>
        /// Determines whether the current master page is in the group identified by the specified key.
        /// </summary>
        /// <param name="currentMasterPage">The current master page.</param>
        /// <param name="generalSettingsKey">The key for the relevant setting in the 'generalSettings' dictionary.</param>
        /// <param name="generalSettings">The general settings section from web.config.</param>
        /// <returns>
        ///   <c>true</c> if the master page is from the specified configuration group; otherwise, <c>false</c>.
        /// </returns>
        protected static bool IsMasterPageInGroup(string currentMasterPage, string generalSettingsKey, Dictionary<string,string> generalSettings)
        {
            if (String.IsNullOrEmpty(currentMasterPage)) return false;

            currentMasterPage = currentMasterPage.ToUpperInvariant();

            // Check if there's a single setting for the master page
            if (generalSettings != null && generalSettings.ContainsKey(generalSettingsKey) && !String.IsNullOrEmpty(generalSettings[generalSettingsKey]))
            {
                if (generalSettings[generalSettingsKey].ToUpperInvariant() == currentMasterPage)
                {
                    return true;
                }
            }

            // Otherwise it's not a match
            return false;
        }
    }
}