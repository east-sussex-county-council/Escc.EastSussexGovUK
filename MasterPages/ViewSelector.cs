using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using EsccWebTeam.Data.Web;
using EsccWebTeam.Data.Xml;
using EsccWebTeam.EastSussexGovUK.DeviceDetection;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// Selects most appropriate master page or MVC layout based on device capabilities, cookie, querystring or URL path
    /// </summary>
    public static class ViewSelector
    {
        /// <summary>
        /// Selects most appropriate master page or MVC layout based on device capabilities, cookie, querystring or URL path
        /// </summary>
        /// <param name="cookies">The cookies.</param>
        /// <param name="session">The session.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <returns></returns>
        /// <remarks>
        /// 	<para>It requires the EsccWebTeam.DeviceDetection web service, and master pages or MVC layouts to be set up in web.config similar to the following:</para>
        /// 	<example>
        /// 		<code>
        /// &lt;configuration&gt;
        ///     &lt;configSections&gt;
        ///            &lt;sectionGroup name="EsccWebTeam.Cms"&gt;
        ///                &lt;section name="GeneralSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" /&gt;
        ///            &lt;/sectionGroup&gt;
        ///            &lt;sectionGroup name="EsccWebTeam.EastSussexGovUK"&gt;
        ///                &lt;section name="GeneralSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" /&gt;
        ///                &lt;section name="DesktopMasterPages" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" /&gt;
        ///                &lt;section name="MobileMasterPages" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" /&gt;
        ///            &lt;/sectionGroup&gt;
        ///        &lt;/configSections&gt;
        ///        &lt;EsccWebTeam.Cms&gt;
        ///            &lt;GeneralSettings&gt;
        ///                &lt;add key="CmsEnabled" value="machinename;machinename;" /&gt;
        ///            &lt;/GeneralSettings&gt;
        ///        &lt;/EsccWebTeam.Cms&gt;
        ///        &lt;EsccWebTeam.EastSussexGovUK&gt;
        ///            &lt;GeneralSettings&gt;
        ///                &lt;add key="MasterPageParameterName" value="template" /&gt;
        ///            &lt;/GeneralSettings&gt;
        ///            &lt;DesktopMasterPages&gt;
        ///                &lt;add key="/somefolder/somefolder/somepage.htm" value="~/master/CustomPage.master" /&gt;
        ///                &lt;add key="/somefolder" value="~/master/CustomFolder.master" /&gt;
        ///                &lt;add key="/" value="~/master/Desktop.master" /&gt;
        ///            &lt;/DesktopMasterPages&gt;
        ///            &lt;MobileMasterPages&gt;
        ///                &lt;add key="/somefolder/somefolder/somepage.htm" value="~/master/CustomMobilePage.master" /&gt;
        ///                &lt;add key="/somefolder" value="~/master/CustomMobileFolder.master" /&gt;
        ///                &lt;add key="/" value="~/master/Mobile.master" /&gt;
        ///            &lt;/MobileMasterPages&gt;
        ///        &lt;/EsccWebTeam.EastSussexGovUK&gt;
        ///    &lt;system.web&gt;
        ///        &lt;httpModules&gt;
        ///            &lt;add name="MasterPageModule" type="EsccWebTeam.EastSussexGovUK.MasterPageModule, EsccWebTeam.EastSussexGovUK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=06fad7304560ae6f"/&gt;
        ///        &lt;/httpModules&gt;
        ///    &lt;/system.web&gt;
        ///  &lt;/configuration&gt;    
        /// </code>
        /// 	</example>
        /// 	<para>For MVC sites using Razor views, specify *.cshtml files wherever *.master is used above. The HTTP module is not needed for MVC.
        /// Instead add similar code to _ViewStart.cshtml.</para>
        /// 	<para>WURFL decides whether a device sees the desktop or mobile template. So how does a mobile device view the desktop site?
        /// It follows the link to choose.ashx within the /masterpages virtual directory of the current application domain (/masterpages must
        /// have script execute permission in IIS for this to work), which sets the session value to desktop for the current application domain.
        /// This is then used instead of the WURFL setting to keep the mobile device on the desktop site.</para>
        /// 	<para>The problem is that on a site with several application domains, this setting is per-application-domain. As soon as the
        /// user strays into a new application domain, the session value will be missing, WURFL will detect a mobile device and go back to
        /// the mobile site.</para>
        /// 	<para>This will be a problem for mobile devices which don't support cookies and want to visit the desktop site. For those
        /// which do support cookies, they store the same setting as session but do it sitewide and are given preference over the session value.</para>
        /// </remarks>
        public static string SelectView(HttpCookieCollection cookies, HttpSessionState session, NameValueCollection queryString, string userAgent)
        {
            // Grab settings from config and set up some defaults
            var generalSettings = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/GeneralSettings") as NameValueCollection;
            var configSettings = new Dictionary<PreferredView, NameValueCollection>();
            configSettings[PreferredView.Mobile] = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/MobileMasterPages") as NameValueCollection;
            configSettings[PreferredView.Desktop] = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/DesktopMasterPages") as NameValueCollection;
            configSettings[PreferredView.Plain] = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/PlainMasterPages") as NameValueCollection;

            string preferredMasterPage = String.Empty;
            var preferredView = PreferredView.Unknown;

            // Are we set up to accept user requests for a master page?
            var acceptUserRequest = (generalSettings != null && !String.IsNullOrEmpty(generalSettings["MasterPageParameterName"]));

            // A cookie is the best indicator because it works across the site, and is set when the user swops from one version 
            // to the other. However we only need to set a cookie if the user isn't happy with the default, and anyway we can't 
            // rely on cookie support on the device, so session and device detection are there as a backup plan.

            // Has user expressed a valid preference, saved in a cookie?
            // Add a "1" because the original cookie used for the preview needed to be cleared due to IE6 problems.
            if (acceptUserRequest)
            {
                string cookieName = generalSettings["MasterPageParameterName"] + "1";
                if (cookies[cookieName] != null)
                {
                    preferredView = AssignMasterPageFromUserRequest(generalSettings, configSettings, preferredView, cookies[cookieName].Value);
                }
            }

            // Next check session for a preferred view. If it's an existing session we want to stick with the expressed preference rather than
            // the default. The idea of putting it in session is that it's only tested on the first request, allowing a user to be directed
            // on that first request to the right site, but then move to the other version if they want to. Trouble is, our website has many
            // separate sessions and the setting may not be in sync, so for us this can't be the best option.
            if (preferredView == PreferredView.Unknown)
            {
                if (session != null && session["EastSussexGovUK.PreferredView"] != null) preferredView = (PreferredView)session["EastSussexGovUK.PreferredView"];
            }

            // As a last resort, depend on the device. This guarantees we will end up with a decision of some kind.
            // Code has been structured to make this the last option to avoid unnecessary calls to the web service.
            // Important for the device detection to be in a web service though, so one copy of the detection code can be cached in one application domain
            // rather than in every calling application.
            if (preferredView == PreferredView.Unknown)
            {
                if (String.IsNullOrEmpty(userAgent))
                {
                    preferredView = PreferredView.Desktop;
                }
                else
                {
                    using (var deviceDetection = new Service())
                    {
                        try
                        {
                            deviceDetection.UseDefaultCredentials = true;
                            deviceDetection.Proxy = XmlHttpRequest.CreateProxy();
                            preferredView = (deviceDetection.IsMobile(userAgent)) ? PreferredView.Mobile : PreferredView.Desktop;
                        }
                        catch (WebException)
                        {
                            // If there's a problem reaching the web service, default to the desktop design which works on all devices
                            preferredView = PreferredView.Desktop;
                        }
                    }
                }
            }

            // We've made a decision, save it in session to help us maintain it consistently
            if (session != null) session["EastSussexGovUK.PreferredView"] = preferredView;

            // Finally, if a master page is requested in the querystring that trumps everything *for this request only*
            // Use the same parameter for the querystring and the cookie so that the connection is obvious
            if (acceptUserRequest && !String.IsNullOrEmpty(queryString[generalSettings["MasterPageParameterName"]]))
            {
                preferredView = AssignMasterPageFromUserRequest(generalSettings, configSettings, preferredView, queryString[generalSettings["MasterPageParameterName"]]);
            }

            // We now know which type of master page we want to use, so now get the path to the master page file.
            // Get master page from config based on folder if available, otherwise based on a single setting.
            if (configSettings.ContainsKey(preferredView) && configSettings[preferredView] != null)
            {
                // Microsoft CMS changes the URL requested by the user to that of the template, so use the corrected URL provided by EastSussexGovUKContext 
                var siteContext = new EastSussexGovUKContext();
                preferredMasterPage = AssignMasterPageByFolder(siteContext.RequestUrl, configSettings[preferredView], preferredMasterPage);
            }
            else if (generalSettings != null)
            {
                var configKey = preferredView.ToString() + "MasterPage";
                if (!String.IsNullOrEmpty(generalSettings[configKey]))
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
        /// <param name="preferredView">The preferred view before this test.</param>
        /// <param name="requestedKey">The key supplied by the user to request the master page.</param>
        /// <returns></returns>
        private static PreferredView AssignMasterPageFromUserRequest(NameValueCollection generalSettings, Dictionary<PreferredView, NameValueCollection> configSettings, PreferredView preferredView, string requestedKey)
        {
            try
            {
                // Get the view requested by the user, and check that we have a master page registered to match it
                var requestedView = (PreferredView)Enum.Parse(typeof(PreferredView), requestedKey, true);

                if ((configSettings.ContainsKey(requestedView) && configSettings[requestedView] != null) ||
                    !String.IsNullOrEmpty(generalSettings[requestedView.ToString() + "MasterPage"]))
                {
                    preferredView = requestedView;
                }
            }
            catch (ArgumentException)
            {
                // Requested key was invalid, ignore it
            }
            return preferredView;
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
            var folderList = Iri.ListFilesAndFoldersInPath(requestUrl);

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
        /// Select a particular master page or MVC layout, then redirect to a new page to apply the selection
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="response">The response</param>
        /// <param name="session">The session</param>
        public static void SwitchView(HttpRequest request, HttpResponse response, HttpSessionState session)
        {
            if (request == null) throw new ArgumentNullException("request");
            if (response == null) throw new ArgumentNullException("response");
            if (session == null) throw new ArgumentNullException("session");

            try
            {
                // Get master page settings from config
                var generalSettings =
                    ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/GeneralSettings") as NameValueCollection;

                // Check relevant bits of config data are available
                if (generalSettings == null || String.IsNullOrEmpty(generalSettings["MasterPageParameterName"]))
                {
                    throw new ConfigurationErrorsException(
                        "<EsccWebTeam.EastSussexGovUK/GeneralSettings><add key=\"MasterPageParameterName\" value=\"???\" /></EsccWebTeam.EastSussexGovUK/GeneralSettings> setting not found in web.config");
                }

                // Note we're only using some master pages here. Other master page groups are designed to be used for a single request only.
                var hasMobileMaster = (ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/MobileMasterPages") != null ||
                                       !String.IsNullOrEmpty(generalSettings["MobileMasterPage"]));
                var hasDesktopMaster = (ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/DesktopMasterPages") != null ||
                                        !String.IsNullOrEmpty(generalSettings["DesktopMasterPage"]));

                // Set cookie to mobile or desktop if specified, or clear to use default.
                var cookieName = generalSettings["MasterPageParameterName"];
                var cookieValue = String.Empty;
                if (request.QueryString[cookieName] == "desktop" && hasDesktopMaster) cookieValue = "desktop";
                if (request.QueryString[cookieName] == "mobile" && hasMobileMaster) cookieValue = "mobile";

                // Add a "1" because the original cookie used for the preview needed to be cleared due to IE6 problems.
                var cookie = new HttpCookie(cookieName + "1", cookieValue);
                cookie.Expires = DateTime.Now.AddYears(cookieValue.Length > 0 ? 50 : -1);
                if (request.Url.Host.IndexOf("eastsussex.gov.uk", StringComparison.InvariantCulture) > -1) cookie.Domain = ".eastsussex.gov.uk";
                response.Cookies.Add(cookie);

                // Put the value in session too. This is a backup in case permanent cookies aren't supported. Cookies are better because they apply sitewide,
                // whereas we have quite a few separate application domains with separate sessions which may not be kept in sync. This will update
                // the session for the current application domain only. Note that it is the current application domain rather than the root one, 
                // because this user control is run within the "masterpages" virtual directory.
                if (cookieValue == "mobile")
                {
                    session["EastSussexGovUK.PreferredView"] = PreferredView.Mobile;
                }
                else if (cookieValue == "desktop")
                {
                    session["EastSussexGovUK.PreferredView"] = PreferredView.Desktop;
                }

                // Redirect to the best available page - either the referrer or the root of the current host
                var redirectUri = new Uri(request.Url.Scheme + "://" + request.Url.Host + "/");
                if (request.QueryString["for"] != "survey" && !String.IsNullOrEmpty(request.QueryString["return"]))
                {
                    var returnUrl = new Uri(request.QueryString["return"], UriKind.RelativeOrAbsolute);
                    returnUrl = Iri.MakeAbsolute(returnUrl);

                    if (returnUrl.Host.EndsWith("eastsussex.gov.uk", StringComparison.OrdinalIgnoreCase) ||
                     !returnUrl.Host.Contains("."))
                    {
                        redirectUri = returnUrl;
                    }
                }

                if (redirectUri != request.Url)
                {
                    // Add a cache-busting parameter to ensure the new version is loaded
                    redirectUri = Iri.RemoveQueryStringParameter(redirectUri, "nocache");
                    redirectUri = new Uri(Iri.PrepareUrlForNewQueryStringParameter(redirectUri) + "nocache=" + Guid.NewGuid().ToString());

                    Http.Status303SeeOther(redirectUri);
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
            }
        }
    }
}