﻿
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.UI;
using EsccWebTeam.Cms;
using EsccWebTeam.EastSussexGovUK.MasterPages;
using Microsoft.ContentManagement.Publishing;
namespace EsccWebTeam.EastSussexGovUK
{
    /// <summary>
    /// Information about the state of the current request
    /// </summary>
    /// <remarks>
    /// <para>The following web.config setting is used to identify robot user agents:</para>
    /// <example>
    /// &lt;configuration&gt;
    ///     &lt;configSections&gt;
    ///         &lt;sectionGroup name=&quot;EsccWebTeam.EastSussexGovUK&quot;&gt;
    ///             &lt;section name=&quot;GeneralSettings&quot; type=&quot;System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089&quot; /&gt;
    ///         &lt;/sectionGroup&gt;
    ///     &lt;/configSections&gt;
    ///
    ///     &lt;EsccWebTeam.EastSussexGovUK&gt;
    ///         &lt;GeneralSettings&gt;
    ///             &lt;add key=&quot;RobotUserAgents&quot; value=&quot;Agent name;Agent name;Agent name&quot; /&gt;
    ///         &lt;/GeneralSettings&gt;
    ///     &lt;/EsccWebTeam.EastSussexGovUK&gt;
    /// &lt;/configuration&gt;
    /// </example>
    /// <para>See <seealso cref="MasterPages.ViewSelector"/> for settings used to identify desktop and mobile master pages.</para>
    /// </remarks>
    public class EastSussexGovUKContext
    {
        #region Shortcuts to configuration settings
        private NameValueCollection generalSettings;

        /// <summary>
        /// Gets the settings for the site and its template.
        /// </summary>
        /// <value>The settings.</value>
        public NameValueCollection Settings
        {
            get
            {
                if (this.generalSettings == null)
                {
                    this.generalSettings = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/GeneralSettings") as NameValueCollection;
                    if (this.generalSettings == null) this.generalSettings = new NameValueCollection();
                }
                return this.generalSettings;
            }
        }


        #endregion // Shortcuts to configuration settings

        #region Shortcuts to information about the current request

        private Page currentPage;
        private Uri requestUrl;
        private Uri baseUrl;
        private int? textSize;

        /// <summary>
        /// Gets the current page.
        /// </summary>
        /// <value>The page.</value>
        public Page Page
        {
            get
            {
                this.currentPage = HttpContext.Current.Handler as Page;
                return this.currentPage;
            }
        }

        /// <summary>
        /// Gets the current page URL.
        /// </summary>
        /// <value>The current page URL.</value>
        public Uri RequestUrl
        {
            get
            {
                // Re-use the result rather than working it out every time
                if (this.requestUrl != null) return this.requestUrl;

                // Default works unless you're in CMS
                this.requestUrl = HttpContext.Current.Request.Url;

                if (CmsUtilities.IsCmsEnabled())
                {
                    // hide API calls in a method to support machines without CMS installed
                    this.requestUrl = GetCmsRequestUrl();
                }

                return this.requestUrl;
            }
        }

        /// <summary>
        /// Gets the URL of the current request, automatically hiding CMS template URLs in published mode
        /// </summary>
        /// <returns></returns>
        private Uri GetCmsRequestUrl()
        {
            if (CmsHttpContext.Current.Posting == null) return this.requestUrl;
            return new Uri(this.requestUrl.Scheme + "://" + this.requestUrl.Host + CmsUtilities.CorrectUnpublishedUrl(CmsUtilities.CorrectPublishedUrl(CmsHttpContext.Current.Posting.UrlModePublished)));
        }

        /// <summary>
        /// Gets the base URL, if any, to prefix links to the website with. Intended for sub-domains to use when linking back to the main site.
        /// </summary>
        /// <value>The base URL.</value>
        public Uri BaseUrl
        {
            get
            {
                if (this.baseUrl != null) return this.baseUrl;

                if (!ViewIsLegacy)
                {
                    if (this.Settings == null) return null;

                    if (!String.IsNullOrEmpty(this.Settings["BaseUrl"]))
                    {
                        this.baseUrl = new Uri(this.Settings["BaseUrl"]);
                        return this.baseUrl;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets whether the current request is for a publicly available URL.
        /// </summary>
        /// <value><c>true</c> if URL is public; otherwise, <c>false</c>.</value>
        public bool IsPublicUrl
        {
            get
            {
                return (HttpContext.Current.Request.Url.Host.IndexOf('.') > -1 && HttpContext.Current.Request.Url.Host.IndexOf("escc.gov") == -1);
            }
        }


        /// <summary>
        /// Gets the current user-selected text size 
        /// </summary>
        /// <returns></returns>
        public int TextSize
        {
            get
            {
                // Return cached value if already worked out
                if (this.textSize.HasValue) return this.textSize.Value;

                // Look for text size value
                this.textSize = 1;
                if (HttpContext.Current.Request.Cookies["textsize"] != null)
                {
                    try
                    {
                        this.textSize = Convert.ToInt32(HttpContext.Current.Request.Cookies["textsize"].Value);
                    }
                    catch (FormatException)
                    {
                        // if value wrong, just ignore it
                    }
                    catch (OverflowException)
                    {
                        // if value wrong, just ignore it
                    }
                }

                // On the text size page itself, it could be in the querystring
                if (HttpContext.Current.Request.QueryString["textsize"] != null)
                {
                    try
                    {
                        this.textSize = Convert.ToInt32(HttpContext.Current.Request.QueryString["textsize"]);
                    }
                    catch (FormatException)
                    {
                        // if value wrong, just ignore it
                    }
                    catch (OverflowException)
                    {
                        // if value wrong, just ignore it
                    }
                }

                // Only accept expected values
                if (this.textSize < 1 || this.textSize > 3) this.textSize = 1;
                return this.textSize.Value;
            }
        }

        /// <summary>
        /// Gets whether the request includes a "do not track" header.
        /// </summary>
        /// <value><c>true</c> if "do not track" is enabled; <c>false</c> if disabled or <c>null</c> if not specified.</value>
        public bool? DoNotTrack
        {
            get
            {
                // The spec defines 3 values: OPT-OUT, OPT-IN and NO-EXPRESSED-PREFERENCE.
                // However the permitted actions are the same for OPT-IN and NO-EXPRESSED-PREFERENCE, 
                // but the header should be echoed back if present so need to distinguish between the two.
                //
                // The setting for "DNT" should be "1", but Firefox 11 is sending things like DNT: 1, 1 and DNT: 1, 1, 1, 1
                // so use StartsWith to support their broken implementation
                var dntHeader = HttpContext.Current.Request.Headers.Get("DNT");
                if (!String.IsNullOrEmpty(dntHeader) && dntHeader.StartsWith("1", StringComparison.Ordinal)) return true;
                if (dntHeader == "0") return false;
                return null;
            }
        }


        #endregion // Shortcuts to information about the current request

        #region Information about current master page

        private bool? viewIsDesktop;
        private bool? viewIsMobile;
        private bool? viewIsLegacy;
        private bool? viewIsPlain;
        private bool? viewIsFullScreen;

        /// <summary>
        /// Gets a value indicating whether the current master page is designed primarily for desktop browsers.
        /// </summary>
        /// <value><c>true</c> if view is for desktop browsers; otherwise, <c>false</c>.</value>
        public bool ViewIsDesktop
        {
            get
            {
                // If we've already checked, return the previous result
                if (this.viewIsDesktop != null) return (bool)this.viewIsDesktop;

                // Otherwise check whether the current master page is listed as a desktop master page
                this.viewIsDesktop = ViewSelector.CurrentViewIs(Page.MasterPageFile, EsccWebsiteView.Desktop);
                if ((bool)this.viewIsDesktop)
                {
                    this.viewIsMobile = false;
                    this.viewIsPlain = false;
                    this.viewIsLegacy = false;
                    this.viewIsFullScreen = false;
                }
                return (bool)this.viewIsDesktop;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current master page is designed primarily for mobiles.
        /// </summary>
        /// <value><c>true</c> if view is for mobiles; otherwise, <c>false</c>.</value>
        public bool ViewIsMobile
        {
            get
            {
                // If we've already checked, return the previous result
                if (this.viewIsMobile != null) return (bool)this.viewIsMobile;

                // Otherwise check whether the current master page is listed as a mobile master page
                this.viewIsMobile = ViewSelector.CurrentViewIs(Page.MasterPageFile, EsccWebsiteView.Mobile);
                if ((bool)this.viewIsMobile)
                {
                    this.viewIsDesktop = false;
                    this.viewIsPlain = false;
                    this.viewIsLegacy = false;
                    this.viewIsFullScreen = false;
                }
                return (bool)this.viewIsMobile;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current master page is designed primarily for API access.
        /// </summary>
        /// <value><c>true</c> if view is for API access; otherwise, <c>false</c>.</value>
        public bool ViewIsPlain
        {
            get
            {
                // If we've already checked, return the previous result
                if (this.viewIsPlain != null) return (bool)this.viewIsPlain;

                // Otherwise check whether the current master page is listed as a plain master page
                this.viewIsPlain = ViewSelector.CurrentViewIs(Page.MasterPageFile, EsccWebsiteView.Plain);
                if ((bool)this.viewIsPlain)
                {
                    this.viewIsDesktop = false;
                    this.viewIsMobile = false;
                    this.viewIsLegacy = false;
                    this.viewIsFullScreen = false;
                }
                return (bool)this.viewIsPlain;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current master page is designed for full-screen applications.
        /// </summary>
        /// <value><c>true</c> if view is for full-screen applications; otherwise, <c>false</c>.</value>
        public bool ViewIsFullScreen
        {
            get
            {
                // If we've already checked, return the previous result
                if (this.viewIsFullScreen != null) return (bool)this.viewIsFullScreen;

                // Otherwise check whether the current master page is listed as a full-screen master page
                this.viewIsFullScreen = ViewSelector.CurrentViewIs(Page.MasterPageFile, EsccWebsiteView.FullScreen);
                if ((bool)this.viewIsFullScreen)
                {
                    this.viewIsDesktop = false;
                    this.viewIsMobile = false;
                    this.viewIsLegacy = false;
                    this.viewIsPlain = false;
                }
                return (bool)this.viewIsFullScreen;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the pre-2011-refresh master page is currently being used
        /// </summary>
        /// <value><c>true</c> if view is legacy; otherwise, <c>false</c>.</value>
        public bool ViewIsLegacy
        {
            get
            {
                // If we've already checked, return the previous result
                if (this.viewIsLegacy != null) return (bool)this.viewIsLegacy;

                // Otherwise check whether the current master page is the legacy master page  
                if (Page == null) return false;
                if (String.IsNullOrEmpty(Page.MasterPageFile))
                {
                    // The old home page is also part of the legacy view
                    if (CmsUtilities.IsCmsEnabled())
                    {
                        this.viewIsLegacy = IsLegacyHomePage();
                        return (bool)this.viewIsLegacy;
                    }
                    else return false;
                }

                // Get the legacy master page from web.config. If it's not there, the legacy view has been retired.
                if (this.Settings != null && !String.IsNullOrEmpty(this.Settings["LegacyMasterPage"]))
                {
                    this.viewIsLegacy = (Page.MasterPageFile.TrimStart('/').ToUpperInvariant() == this.Settings["LegacyMasterPage"].Replace("~", HttpRuntime.AppDomainAppVirtualPath).TrimStart('/').ToUpperInvariant());
                }
                else this.viewIsLegacy = false;

                if ((bool)this.viewIsLegacy)
                {
                    this.viewIsDesktop = false;
                    this.viewIsMobile = false;
                    this.viewIsPlain = false;
                    this.viewIsFullScreen = false;
                }
                return (bool)this.viewIsLegacy;
            }
        }

        /// <summary>
        /// Determines whether the current page is the pre-refresh home page.
        /// </summary>
        /// <returns></returns>
        private bool? IsLegacyHomePage()
        {
            var posting = CmsUtilities.Posting;
            return (posting != null && posting.Template.Guid == "{AE1E8507-8136-481F-8EBF-73CB244CFEA4}");
        }

        #endregion // Information about current master page

        #region Information about the current user

        private bool? userIsLibraryCatalogue;

        /// <summary>
        /// Gets whether the user is on a library catalogue PC in a library.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if user is on a library catalogue PC; otherwise, <c>false</c>.
        /// </value>
        public bool UserIsLibraryCatalogue
        {
            get
            {
                if (this.userIsLibraryCatalogue == null)
                {
                    var userAgent = HttpContext.Current.Request.UserAgent;
                    this.userIsLibraryCatalogue = (userAgent != null && userAgent.IndexOf("ESCC Libraries") > -1);
                }

                return (bool)this.userIsLibraryCatalogue;
            }
        }

        /// <summary>
        /// Determines whether the current user agent is a robot such as a search engine, based on a list of search engines in web.config
        /// </summary>
        /// <returns><c>true</c> if recognised as a search engine, otherwise <c>false</c></returns>
        /// <remarks>I wasn't sure whether all the IndexOf commands would be too slow with a long list of user agents, 
        /// but I timed several requests and it usually comes in at under a millisecond - RM</remarks>
        public bool UserIsRobot
        {
            get
            {
                // Use session rather than a field to stash this value, because it shouldn't change for the duration of a session
                var userAgent = HttpContext.Current.Request.UserAgent;
                if (userAgent == null || userAgent.Length == 0)
                {
                    return false;
                }
                else
                {
                    var sess = HttpContext.Current.Session;
                    if (sess != null && sess["IsRobot"] != null)
                    {
                        return (bool)sess["IsRobot"];
                    }

                    using (var deviceDetection = new DeviceDetection.Service())
                    {
                        deviceDetection.UseDefaultCredentials = true;
                        bool isRobot = deviceDetection.IsRobot(userAgent);
                        if (sess != null) sess["IsRobot"] = isRobot;
                        return isRobot;
                    }
                }
            }
        }

        #endregion

        #region Shortcut methods to load common content

        /// <summary>
        /// Return an HTTP 310 error code and display a standard message
        /// </summary>
        /// <param name="displayControl">The container control for the message</param>
        /// <param name="hideControls">Any other controls to hide when in an error state</param>
        public static void HttpStatus310Gone(Control displayControl, params Control[] hideControls)
        {
            HttpStatusMessage(310, displayControl, hideControls);
        }

        /// <summary>
        /// Return an HTTP 400 error code and display a standard message
        /// </summary>
        /// <param name="displayControl">The container control for the message</param>
        /// <param name="hideControls">Any other controls to hide when in an error state</param>
        public static void HttpStatus400BadRequest(Control displayControl, params Control[] hideControls)
        {
            HttpStatusMessage(400, displayControl, hideControls);
        }

        /// <summary>
        /// Return an HTTP 404 error code and display a standard message
        /// </summary>
        /// <param name="displayControl">The container control for the message</param>
        /// <param name="hideControls">Any other controls to hide when in an error state</param>
        public static void HttpStatus404NotFound(Control displayControl, params Control[] hideControls)
        {
            HttpStatusMessage(404, displayControl, hideControls);
        }

        /// <summary>
        /// Return an HTTP error code and display a standard message
        /// </summary>
        /// <param name="statusCode">The HTTP status code</param>
        /// <param name="displayControl">The container control for the message</param>
        /// <param name="hideControls">Any other controls to hide when in an error state</param>
        private static void HttpStatusMessage(int statusCode, Control displayControl, params Control[] hideControls)
        {
            // Check parameters
            if (statusCode != 310 && statusCode != 400 && statusCode != 404)
            {
                throw new ArgumentException("Only the 310, 400 and 404 status codes have a message to display");
            }
            if (displayControl == null) throw new ArgumentNullException("displayControl");

            // Hide any elements of the page not needed in an error state
            foreach (Control control in displayControl.Controls)
            {
                control.Visible = false;
            }
            if (hideControls != null)
            {
                foreach (Control control in hideControls)
                {
                    control.Visible = false;
                }
            }

            // Display the error message and return its status code
            displayControl.Controls.Add(displayControl.Page.LoadControl("~/masterpages/controls/error" + statusCode + ".ascx"));
        }

        #endregion // Shortcut methods to load common content
    }
}