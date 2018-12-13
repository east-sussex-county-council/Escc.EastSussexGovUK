using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;

namespace Escc.EastSussexGovUK.Views
{
    /// <summary>
    /// Selects most appropriate master page or MVC layout based on querystring or URL path
    /// </summary>
    public static class ViewSelector
    {
        /// <summary>
        /// Selects most appropriate master page or MVC layout based on querystring or URL path
        /// </summary>
        /// <param name="forUrl">The URL of the page to select a view for.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <param name="viewEngine">The view engine to select the view for.</param>
        /// <returns></returns>
        public static string SelectView(Uri forUrl, string userAgent, ViewEngine viewEngine=ViewEngine.WebForms)
        {
            if (forUrl == null)
            {
                throw new ArgumentNullException(nameof(forUrl));
            }
            var queryString = HttpUtility.ParseQueryString(forUrl.Query);

            // Grab settings from config and set up some defaults
            var generalSettings = ConfigurationManager.GetSection("Escc.EastSussexGovUK/GeneralSettings") as NameValueCollection;
            if (generalSettings == null) generalSettings = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/GeneralSettings") as NameValueCollection;
            var configSettings = new Dictionary<EsccWebsiteView, NameValueCollection>();

            var configSettingsGroup = viewEngine == ViewEngine.WebForms ? "MasterPage" : "MvcLayout";
            configSettings[EsccWebsiteView.Desktop] = ConfigurationManager.GetSection("Escc.EastSussexGovUK/Desktop" + configSettingsGroup + "s") as NameValueCollection;
            if (configSettings[EsccWebsiteView.Desktop] == null) configSettings[EsccWebsiteView.Desktop] = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/Desktop" + configSettingsGroup + "s") as NameValueCollection;

            configSettings[EsccWebsiteView.Plain] = ConfigurationManager.GetSection("Escc.EastSussexGovUK/Plain" + configSettingsGroup + "s") as NameValueCollection;
            if (configSettings[EsccWebsiteView.Plain] == null) configSettings[EsccWebsiteView.Plain] = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/Plain" + configSettingsGroup + "s") as NameValueCollection;

            string preferredMasterPage = String.Empty;
            var preferredView = EsccWebsiteView.Desktop;

            // Are we set up to accept user requests for a master page?
            var acceptUserRequest = (generalSettings != null && !String.IsNullOrEmpty(generalSettings["MasterPageParameterName"]));

            // If a master page is requested in the querystring that trumps everything *for this request only*
            if (acceptUserRequest && !String.IsNullOrEmpty(queryString[generalSettings["MasterPageParameterName"]]))
            {
                preferredView = AssignMasterPageFromUserRequest(generalSettings, configSettings, configSettingsGroup, preferredView, queryString[generalSettings["MasterPageParameterName"]]);
            }

            // We now know which type of master page we want to use, so now get the path to the master page file.
            // Get master page from config based on folder if available, otherwise based on a single setting.
            if (configSettings.ContainsKey(preferredView) && configSettings[preferredView] != null)
            {
                // A CMS may change the URL requested by the user to that of the template, so use the corrected URL provided by HostingEnvironmentContext 
                var siteContext = new HostingEnvironmentContext(forUrl);
                preferredMasterPage = AssignMasterPageByFolder(forUrl, configSettings[preferredView], preferredMasterPage);
            }
            else if (generalSettings != null)
            {
                var configKey = preferredView + configSettingsGroup;
                if (!String.IsNullOrEmpty(generalSettings[configKey]))
                {
                    preferredMasterPage = generalSettings[configKey];
                }
            }

            if (viewEngine == ViewEngine.Mvc && String.IsNullOrEmpty(preferredMasterPage))
            {
                throw new ConfigurationErrorsException("The path to the selected MVC layout was not specified. Set the path in the Escc.EastSussexGovUK/GeneralSettings/add[@key='" + preferredView + configSettingsGroup + "'] element in web.config.");
            }

            return preferredMasterPage;
        }

        /// <summary>
        /// Assigns the master page based on a user request
        /// </summary>
        /// <param name="generalSettings">The general settings.</param>
        /// <param name="configSettings">The config settings for specific master pages.</param>
        /// <param name="configSettingsGroup">The configuration settings group.</param>
        /// <param name="esccWebsiteView">The preferred view before this test.</param>
        /// <param name="requestedKey">The key supplied by the user to request the master page.</param>
        /// <returns></returns>
        private static EsccWebsiteView AssignMasterPageFromUserRequest(NameValueCollection generalSettings, Dictionary<EsccWebsiteView, NameValueCollection> configSettings, string configSettingsGroup, EsccWebsiteView esccWebsiteView, string requestedKey)
        {
            try
            {
                // Get the view requested by the user, and check that we have a master page registered to match it
                var requestedView = (EsccWebsiteView)Enum.Parse(typeof(EsccWebsiteView), requestedKey, true);

                if ((configSettings.ContainsKey(requestedView) && configSettings[requestedView] != null) ||
                    !String.IsNullOrEmpty(generalSettings[requestedView + configSettingsGroup]))
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
        /// Assigns the master page by folder.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="masterPageSettings">The master page settings.</param>
        /// <param name="preferredMasterPage">The preferred master page.</param>
        /// <returns></returns>
        private static string AssignMasterPageByFolder(Uri requestUrl, NameValueCollection masterPageSettings, string preferredMasterPage)
        {
            // Break the URL into folders. Work backwards starting from the page, its folder, its subfolder 
            // and so on back to the root until we find a setting. If we don't find a setting, stick with the existing default.
            var folderList = ListFilesAndFoldersInPath(requestUrl);

            var i = 0;
            var folders = folderList.Count;

            while (i < folders)
            {
                string lowercaseUrl = folderList[i].ToLowerInvariant();
                if (!String.IsNullOrEmpty(masterPageSettings[lowercaseUrl]))
                {
                    // found a setting for this file/folder
                    preferredMasterPage = masterPageSettings[lowercaseUrl];
                    break;
                }
                i++;
            }
            return preferredMasterPage;
        }


        /// <summary>
        /// Lists files and folders in the path of an absolute URL or a URL relative to the site root. Trailing slashes are trimmed.
        /// </summary>
        /// <param name="urlToParse">The URL to parse.</param>
        /// <returns>List of paths, starting with the most specific and ending with only the root folder or filename</returns>
        /// <exception cref="ArgumentException"></exception>
        private static IList<string> ListFilesAndFoldersInPath(Uri urlToParse)
        {
            if (urlToParse == null) throw new ArgumentNullException("urlToParse");

            // get path to process
            var path = String.Empty;
            if (urlToParse.IsAbsoluteUri)
            {
                path = urlToParse.AbsolutePath;
            }
            else if (urlToParse.ToString().StartsWith("/", StringComparison.Ordinal))
            {
                path = urlToParse.ToString();
                var querystring = path.IndexOf("?", StringComparison.Ordinal);
                if (querystring > -1) path = path.Substring(0, querystring);
            }
            else
            {
                throw new ArgumentException("urlToParse must be an absolute URL or a relative URL which begins with /", "urlToParse");
            }

            // Build up a list of paths, knocking off one segment at a time
            var paths = new List<string>();

            while (path.Length > 0)
            {
                paths.Add(path);
                var slashIndex = path.LastIndexOf("/", StringComparison.Ordinal);
                if (slashIndex == -1) break;
                path = path.Substring(0, slashIndex);
            }
            paths.Add("/"); // because we knock off the trailing slash, need to hard-code adding the root which is only a trailing slash

            return paths;
        }

        /// <summary>
        /// Gets the current master page or MVC layout type based on its path
        /// </summary>
        /// <param name="applicationPath">The virtual path to the application, starting and ending with a /</param>
        /// <param name="currentView">Path of current master page or MVC layout.</param>
        /// <param name="viewEngine">The view engine.</param>
        /// <returns></returns>
        public static EsccWebsiteView CurrentViewIs(string applicationPath, string currentView, ViewEngine viewEngine = ViewEngine.WebForms)
        {
            if (CurrentViewIs(applicationPath, currentView, EsccWebsiteView.Desktop, viewEngine)) return EsccWebsiteView.Desktop;
            if (CurrentViewIs(applicationPath, currentView, EsccWebsiteView.FullScreen, viewEngine)) return EsccWebsiteView.FullScreen;
            if (CurrentViewIs(applicationPath, currentView, EsccWebsiteView.Plain, viewEngine)) return EsccWebsiteView.Plain;
            return EsccWebsiteView.Unknown;
        }

        /// <summary>
        /// Determines whether the currently selected master page or MVC layout is an instance of the given view.
        /// </summary>
        /// <param name="applicationPath">The virtual path to the application, starting and ending with a /</param>
        /// <param name="currentView">Path of current master page or MVC layout.</param>
        /// <param name="view">Check whether this view is currently selected.</param>
        /// <param name="viewEngine">The view engine.</param>
        /// <returns></returns>
        public static bool CurrentViewIs(string applicationPath, string currentView, EsccWebsiteView view, ViewEngine viewEngine = ViewEngine.WebForms)
        {
            var generalSettings = ConfigurationManager.GetSection("Escc.EastSussexGovUK/GeneralSettings") as NameValueCollection;
            if (generalSettings == null) generalSettings = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/GeneralSettings") as NameValueCollection;
            return IsMasterPageInGroup(applicationPath, currentView, view.ToString(), generalSettings, viewEngine);
        }

        /// <summary>
        /// Determines whether the current master page is in the group identified by the specified key.
        /// </summary>
        /// <param name="applicationPath">The virtual path to the application, starting and ending with a /</param>
        /// <param name="currentMasterPage">The current master page.</param>
        /// <param name="groupName">The key part of a configuration section name or setting listing master pages.</param>
        /// <param name="generalSettings">The general settings section from web.config.</param>
        /// <param name="viewEngine">The view engine.</param>
        /// <returns>
        ///   <c>true</c> if the master page is from the specified configuration group; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsMasterPageInGroup(string applicationPath, string currentMasterPage, string groupName, NameValueCollection generalSettings, ViewEngine viewEngine)
        {
            if (String.IsNullOrEmpty(currentMasterPage)) return false;

            currentMasterPage = currentMasterPage.Substring(applicationPath.Length);
            if (!currentMasterPage.StartsWith("/", StringComparison.Ordinal)) currentMasterPage = "/" + currentMasterPage;
            currentMasterPage = "~" + currentMasterPage.ToUpperInvariant();

            // Check if there's a single setting for the master page
            var configSettingsGroup = viewEngine == ViewEngine.WebForms ? "MasterPage" : "MvcLayout";
            if (generalSettings != null && !String.IsNullOrEmpty(generalSettings[groupName + configSettingsGroup]))
            {
                if (generalSettings[groupName + configSettingsGroup].ToUpperInvariant() == currentMasterPage)
                {
                    return true;
                }
            }

            // If not, check if there's a group of settings
            var masterPageSettings = ConfigurationManager.GetSection("Escc.EastSussexGovUK/" + groupName + configSettingsGroup + "s") as NameValueCollection;
            if (masterPageSettings == null) masterPageSettings = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/" + groupName + configSettingsGroup + "s") as NameValueCollection;
            if (masterPageSettings != null)
            {
                foreach (string key in masterPageSettings.Keys)
                {
                    if (masterPageSettings[key].ToUpperInvariant() == currentMasterPage)
                    {
                        return true;
                    }
                }
            }

            // Otherwise it's not a match
            return false;
        }
    }
}