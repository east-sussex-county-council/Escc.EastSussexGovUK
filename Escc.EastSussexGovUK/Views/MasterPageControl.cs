using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Escc.EastSussexGovUK.Features;
using Escc.Net;
using Exceptionless;

namespace Escc.EastSussexGovUK.Views
{
    /// <summary>
    /// Loads a section of the master page, either from a local usercontrol or remotely from the public website.
    /// </summary>
    public class MasterPageControl : PlaceHolder
    {
        /// <summary>
        /// Gets or sets an identifier for the control to load.
        /// </summary>
        /// <value>The control.</value>
        public string Control { get; set; }

        /// <summary>
        /// Gets or sets the provider for working out the current context within the site's information architecture.
        /// </summary>
        /// <value>
        /// The breadcrumb provider.
        /// </value>
        public IBreadcrumbProvider BreadcrumbProvider { get; set; }
        private static Dictionary<string, ManualResetEvent> waitFor;
        private NameValueCollection config = null;


        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if cached file was not written</exception>
        protected override void CreateChildControls()
        {
            // Check that the Control property is set
            if (String.IsNullOrEmpty(this.Control)) throw new ArgumentNullException("Control", "Property 'Control' must be set for class MasterPageControl");

            // Get the configuration settings for remote master pages. Is this control in there?
            this.config = ConfigurationManager.GetSection("Escc.EastSussexGovUK/RemoteMasterPage") as NameValueCollection;
            if (this.config == null) this.config = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/RemoteMasterPage") as NameValueCollection;
            if (this.config != null && !String.IsNullOrEmpty(this.config["MasterPageControlUrl"]))
            {
                LoadRemoteControl();
            }
            else
            {
                LoadLocalControl();
            }
        }

        /// <summary>
        /// Loads a local usercontrol.
        /// </summary>
        /// <exception cref="System.Web.HttpException">Thrown if usercontrol does not exist</exception>
        private void LoadLocalControl()
        {
            // Default to the path that used to be hard-coded
            var localControlUrl = "~/masterpages/controls/{0}.ascx";

            // Allow override to load controls from anywhere
            this.config = ConfigurationManager.GetSection("Escc.EastSussexGovUK/GeneralSettings") as NameValueCollection;
            if (this.config == null) this.config = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/GeneralSettings") as NameValueCollection;
            if (this.config != null && !String.IsNullOrEmpty(this.config["MasterPageControlUrl"]))
            {
                localControlUrl = this.config["MasterPageControlUrl"];
            }

            this.Controls.Add(Page.LoadControl(String.Format(CultureInfo.InvariantCulture, localControlUrl, this.Control)));
        }

        /// <summary>
        /// Fetches the master page control from a remote URL.
        /// </summary>
        private void LoadRemoteControl()
        {
            // Add the current section parsed from the breadcrumb trail
            var selectedSection = String.Empty;
            if (BreadcrumbProvider == null)
            {
                BreadcrumbProvider = new BreadcrumbTrailFromConfig();
            }
            var trail = BreadcrumbProvider.BuildTrail();
            if (trail != null && trail.Count > 1)
            {
                selectedSection = new List<string>(trail.Keys)[1].ToUpperInvariant();
            }

            // Cache remote template elements using the application cache
            RemoteMasterPageCacheProviderBase cacheProvider = new RemoteMasterPageMemoryCacheProvider(Control, selectedSection, new LibraryCatalogueContext(Page.Request.UserAgent), new TextSize(Page.Request.Cookies, Page.Request.QueryString));

            // Provide a way to force an immediate update of the cache
            var forceCacheRefresh = (Page.Request.QueryString["ForceCacheRefresh"] == "1");

            // Update the cached control if it's missing or too old
            if (!cacheProvider.CachedVersionExists || !cacheProvider.CachedVersionIsFresh || forceCacheRefresh)
            {
                RequestRemoteHtml(cacheProvider, selectedSection);
            }

            // Output the HTML
            this.Controls.Add(new LiteralControl(cacheProvider.ReadHtmlFromCache()));
        }

        /// <summary>
        /// Requests the remote HTML.
        /// </summary>
        /// <param name="cacheProvider">Strategy for caching the remote HTML.</param>
        /// <param name="selectedSection">The selected section.</param>
        private void RequestRemoteHtml(RemoteMasterPageCacheProviderBase cacheProvider, string selectedSection)
        {
            try
            {
                // Get the URL to request the cached control from.
                // Include text size so that header knows which links to apply
                var textSize = new TextSize(HttpContext.Current.Request.Cookies, HttpContext.Current.Request.QueryString);
                Uri urlToRequest = new Uri(String.Format(CultureInfo.CurrentCulture, config["MasterPageControlUrl"], this.Control));
                var applicationPath = HttpUtility.UrlEncode(HttpRuntime.AppDomainAppVirtualPath.ToLower(CultureInfo.CurrentCulture).TrimEnd('/'));
                var query = HttpUtility.ParseQueryString(urlToRequest.Query);
                query.Add("section", selectedSection);
                query.Add("host", Page.Request.Url.Host);
                query.Add("textsize", textSize.CurrentTextSize().ToString(CultureInfo.InvariantCulture));
                query.Add("path", applicationPath);
                urlToRequest = new Uri(urlToRequest.Scheme + "://" + urlToRequest.Authority + urlToRequest.AbsolutePath + "?" + query);

                // Create the request. Pass current user-agent so that library catalogue PCs can be detected by the remote script.
                var webRequest = (HttpWebRequest)WebRequest.Create(urlToRequest);
                webRequest.UseDefaultCredentials = true;
                webRequest.UserAgent = Page.Request.UserAgent;
                webRequest.Proxy = new ConfigurationProxyProvider().CreateProxy();
#if DEBUG
                // Turn off SSL check in debug mode as it will always fail against a self-signed certificate used for development
                webRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
#endif

                // Prepare the information we'll need when the response comes back
                var state = new RequestState();
                state.Request = webRequest;
                state.CacheProvider = cacheProvider;

                // Kick off the request and, only if there's nothing already cached which we can use, wait for it to come back
                if (cacheProvider.SupportsAsync)
                {
                    RequestRemoteHtmlAsynchronous(cacheProvider, webRequest, state);
                }
                else
                {
                    RequestRemoteHtmlSynchronous(cacheProvider, webRequest);
                }
            }
            catch (UriFormatException ex)
            {
                throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, config["MasterPageControlUrl"], this.Control) + " is not a valid absolute URL", ex);
            }

        }

        private static void RequestRemoteHtmlSynchronous(RemoteMasterPageCacheProviderBase cacheProvider, HttpWebRequest webRequest)
        {
            try
            {
                using (var response = webRequest.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        cacheProvider.SaveRemoteHtmlToCache(responseStream);
                    }
                }
            }
            catch (WebException ex)
            {
                // Publish exception, otherwise it just disappears as async method has no calling code to throw to.
                ex.Data.Add("URL which failed", ex.Response.ResponseUri);
                ex.ToExceptionless().Submit();
            }
        }

        private void RequestRemoteHtmlAsynchronous(RemoteMasterPageCacheProviderBase cacheProvider, HttpWebRequest webRequest, RequestState state)
        {
            // Only wait for the response if there's nothing there at all.
            // If there is a cached version use that, but set the update running in the background.
            if (!cacheProvider.CachedVersionExists)
            {
                if (waitFor == null) waitFor = new Dictionary<string, ManualResetEvent>();
                waitFor[this.Control] = new ManualResetEvent(false);
                state.WaitForResponse = waitFor[this.Control];
            }

            webRequest.BeginGetResponse(new AsyncCallback(Response_Callback), state);
            if (!cacheProvider.CachedVersionExists) waitFor[this.Control].WaitOne(new TimeSpan(0, 0, 10));
        }


        /// <summary>
        /// Saves a remote template control as a local file
        /// </summary>
        /// <param name="result">The result.</param>
        /// <exception cref="UnauthorizedAccessException">Thrown if cache file cannot be written</exception>
        private static void Response_Callback(IAsyncResult result)
        {
            try
            {
                // Get the data we need from when the request was fired off
                var state = (RequestState)result.AsyncState;

                // Get the HTML from the response and save it to a temporary file
                using (var response = state.Request.EndGetResponse(result))
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        state.CacheProvider.SaveRemoteHtmlToCache(responseStream);
                    }
                }

                // Let the calling thread continue, if it was waiting for a file to be available
                if (state.WaitForResponse != null) state.WaitForResponse.Set();
            }
            catch (WebException ex)
            {
                // Publish exception, otherwise it just disappears as async method has no calling code to throw to.
                ex.Data.Add("URL which failed", ex.Response.ResponseUri);
                ex.ToExceptionless().Submit();
            }
        }

        /// <summary>
        /// Container object for state which needs to be passed to asynchronous callback
        /// </summary>
        private class RequestState
        {
            /// <summary>
            /// Gets or sets the web request which was fired off asynchronously.
            /// </summary>
            /// <value>The request.</value>
            public WebRequest Request { get; set; }

            /// <summary>
            /// If the calling thread is waiting for the response, gets or sets the object to release it
            /// </summary>
            /// <value>The wait for response.</value>
            public ManualResetEvent WaitForResponse { get; set; }

            /// <summary>
            /// Gets or sets the cache provider.
            /// </summary>
            /// <value>The cache provider.</value>
            public RemoteMasterPageCacheProviderBase CacheProvider { get; set; }
        }
    }
}