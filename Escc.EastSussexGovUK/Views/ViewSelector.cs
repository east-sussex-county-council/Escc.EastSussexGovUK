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
        /// <param name="groupedByViewSettings">The settings section for a specific <see cref="EsccWebsiteView"/>, which overrides <paramref name="generalSettings"/></param>
        /// <param name="viewEngine">The view engine to select the view for.</param>
        /// <returns></returns>
        protected string SelectView(Uri forUrl, string userAgent, Dictionary<string,string> generalSettings, Dictionary<EsccWebsiteView, Dictionary<string,string>> groupedByViewSettings, ViewEngine viewEngine)
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
                preferredView = AssignMasterPageFromUserRequest(generalSettings, groupedByViewSettings, configSettingsGroup, preferredView, queryString[generalSettings["MasterPageParameterName"]]);
            }

            // We now know which type of master page we want to use, so now get the path to the master page file.
            // Get master page from config based on folder if available, otherwise based on a single setting.
            if (groupedByViewSettings.ContainsKey(preferredView) && groupedByViewSettings[preferredView] != null)
            {
                preferredMasterPage = AssignMasterPageByFolder(forUrl, groupedByViewSettings[preferredView]);
            }
            else if (generalSettings != null)
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
        /// <param name="configSettings">The config settings for specific master pages.</param>
        /// <param name="configSettingsGroup">The configuration settings group.</param>
        /// <param name="esccWebsiteView">The preferred view before this test.</param>
        /// <param name="requestedKey">The key supplied by the user to request the master page.</param>
        /// <returns></returns>
        private static EsccWebsiteView AssignMasterPageFromUserRequest(Dictionary<string,string> generalSettings, Dictionary<EsccWebsiteView, Dictionary<string,string>> configSettings, string configSettingsGroup, EsccWebsiteView esccWebsiteView, string requestedKey)
        {
            try
            {
                // Get the view requested by the user, and check that we have a master page registered to match it
                var requestedView = (EsccWebsiteView)Enum.Parse(typeof(EsccWebsiteView), requestedKey, true);
                var configKey = requestedView + configSettingsGroup;

                if ((configSettings.ContainsKey(requestedView) && configSettings[requestedView] != null) ||
                    (generalSettings.ContainsKey(configKey) && !String.IsNullOrEmpty(generalSettings[configKey])))
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
        /// <returns></returns>
        private static string AssignMasterPageByFolder(Uri requestUrl, Dictionary<string,string> masterPageSettings)
        {
            // Break the URL into folders. Work backwards starting from the page, its folder, its subfolder 
            // and so on back to the root until we find a setting. If we don't find a setting, stick with the existing default.
            var folderList = ListFilesAndFoldersInPath(requestUrl);

            var i = 0;
            var folders = folderList.Count;
            string preferredMasterPage = string.Empty;

            while (i < folders)
            {
                string lowercaseUrl = folderList[i].ToLowerInvariant();
                if (masterPageSettings.ContainsKey(lowercaseUrl) && !String.IsNullOrEmpty(masterPageSettings[lowercaseUrl]))
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
        /// Determines whether the current master page is in the group identified by the specified key.
        /// </summary>
        /// <param name="applicationPath">The virtual path to the application, starting and ending with a /</param>
        /// <param name="currentMasterPage">The current master page.</param>
        /// <param name="generalSettingsKey">The key for the relevant setting in the 'generalSettings' dictionary.</param>
        /// <param name="generalSettings">The general settings section from web.config.</param>
        /// <param name="groupedByViewSettings">The settings section for a specific <see cref="EsccWebsiteView"/>, which overrides <paramref name="generalSettings"/></param>
        /// <returns>
        ///   <c>true</c> if the master page is from the specified configuration group; otherwise, <c>false</c>.
        /// </returns>
        protected static bool IsMasterPageInGroup(string applicationPath, string currentMasterPage, string generalSettingsKey, Dictionary<string,string> generalSettings, Dictionary<string, string> groupedByViewSettings)
        {
            if (String.IsNullOrEmpty(currentMasterPage)) return false;

            currentMasterPage = currentMasterPage.Substring(applicationPath.Length);
            if (!currentMasterPage.StartsWith("/", StringComparison.Ordinal)) currentMasterPage = "/" + currentMasterPage;
            currentMasterPage = "~" + currentMasterPage.ToUpperInvariant();

            // Check if there's a single setting for the master page
            if (generalSettings != null && generalSettings.ContainsKey(generalSettingsKey) && !String.IsNullOrEmpty(generalSettings[generalSettingsKey]))
            {
                if (generalSettings[generalSettingsKey].ToUpperInvariant() == currentMasterPage)
                {
                    return true;
                }
            }

            // If not, check if there's a group of settings
            if (groupedByViewSettings != null)
            {
                foreach (string key in groupedByViewSettings.Keys)
                {
                    if (groupedByViewSettings[key].ToUpperInvariant() == currentMasterPage)
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