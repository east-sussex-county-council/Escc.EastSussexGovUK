using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Exceptionless;
using System.Web;
using Escc.EastSussexGovUK.Features;
using Escc.Net;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Escc.EastSussexGovUK.Views
{
    /// <summary>
    /// Loads sections of the master page remotely from a URL.
    /// </summary>
    public class RemoteMasterPageHtmlProvider : IHtmlControlProvider
    {
        private readonly IRemoteMasterPageCacheProvider _cacheProvider;
        private readonly Uri _masterPageControlUrl;
        private readonly string _userAgent;
        private readonly bool _forceCacheRefresh;
        private static IHttpClientProvider _httpClientProvider;
        private static HttpClient _httpClient;

        /// <summary>
        /// Creates a new instance of <see cref="RemoteMasterPageHtmlProvider"/>
        /// </summary>
        /// <param name="masterPageControlUrl">The URL from which to download the remote HTML, with {0} where the <c>controlId</c> should be inserted</param>
        /// <param name="httpClientProvider">Strategy to get the HttpClient instance used for requests.</param>
        /// <param name="userAgent">The user agent to use when requesting the remote HTML (usually the user agent for the consuming request)</param>
        /// <param name="cacheProvider">Strategy for caching the remote HTML.</param>
        /// <param name="forceCacheRefresh"><c>true</c> to ensure the HTML is requested from the remote URL, not from a local cache</param>
        public RemoteMasterPageHtmlProvider(Uri masterPageControlUrl, IHttpClientProvider httpClientProvider, string userAgent, IRemoteMasterPageCacheProvider cacheProvider, bool forceCacheRefresh = false)
        {
            _masterPageControlUrl = masterPageControlUrl ?? throw new ArgumentNullException(nameof(masterPageControlUrl));
            _httpClientProvider = httpClientProvider ?? throw new ArgumentNullException(nameof(httpClientProvider));
            _userAgent = userAgent;
            _cacheProvider = cacheProvider;
            _forceCacheRefresh = forceCacheRefresh;
        }

        /// <summary>
        /// Creates a new instance of <see cref="RemoteMasterPageHtmlProvider"/>
        /// </summary>
        /// <param name="remoteMasterPageSettings">Settings for fetching and caching the remote master page controls</param>
        /// <param name="httpContextAccessor">A method of getting the web request which requires a response with the template applied.</param>
        /// <param name="httpClientProvider">A method of getting an <c>HttpClient</c> to connect to remote resources.</param>
        /// <param name="cacheProvider">Strategy for caching the remote HTML.</param>
        public RemoteMasterPageHtmlProvider(IOptions<RemoteMasterPageSettings> remoteMasterPageSettings, IHttpContextAccessor httpContextAccessor, IHttpClientProvider httpClientProvider, IRemoteMasterPageCacheProvider cacheProvider)
        {
            if (remoteMasterPageSettings == null || remoteMasterPageSettings.Value == null) { throw new ArgumentNullException(nameof(remoteMasterPageSettings)); }
            var request = httpContextAccessor?.HttpContext?.Request ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            var queryString = HttpUtility.ParseQueryString(request.QueryString.Value);

            _masterPageControlUrl = remoteMasterPageSettings.Value.PartialViewUrl;
            _httpClientProvider = httpClientProvider;
            _userAgent = request.Headers["User-Agent"].ToString();

            _cacheProvider = cacheProvider;
            _cacheProvider.CacheDuration = TimeSpan.FromMinutes(remoteMasterPageSettings.Value.CacheMinutes);

            _forceCacheRefresh = (queryString["ForceCacheRefresh"] == "1"); // Provide a way to force an immediate update of the cache;
        }

        /// <summary>
        /// Fetches the master page control from a remote URL.
        /// </summary>
        /// <param name="applicationId">A string which identifies the application making the request</param>
        /// <param name="forUrl">The page to request the control for (usually the current page)</param>
        /// <param name="controlId">A key identifying the control to cache.</param>
        /// <param name="breadcrumbProvider">The provider for working out the current context within the site's information architecture.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        public async Task<string> FetchHtmlForControl(string applicationId, Uri forUrl, string controlId, IBreadcrumbProvider breadcrumbProvider, int textSize, bool isLibraryCatalogueRequest)
        {
            // Check parameters
            if (string.IsNullOrEmpty(applicationId))
            {
                throw new ArgumentException("applicationId should be be a string uniquely identifying the application", nameof(applicationId));
            }

            if (forUrl == null)
            {
                throw new ArgumentNullException(nameof(forUrl));
            }

            if (string.IsNullOrEmpty(controlId))
            {
                throw new ArgumentException("controlId must be specified to load the control", nameof(controlId));
            }

            if (breadcrumbProvider == null)
            {
                throw new ArgumentNullException(nameof(breadcrumbProvider));
            }

            // Add the current section parsed from the breadcrumb trail
            var selectedSection = String.Empty;
            var trail = breadcrumbProvider.BuildTrail();
            if (trail != null && trail.Count > 1)
            {
                selectedSection = new List<string>(trail.Keys)[1].ToUpperInvariant();
            }

            // Update the cached control if it's missing or too old
            if (_cacheProvider == null || !_cacheProvider.CachedVersionExists(applicationId, controlId, selectedSection, textSize, isLibraryCatalogueRequest) || !_cacheProvider.CachedVersionIsFresh(applicationId, controlId, selectedSection, textSize, isLibraryCatalogueRequest) || _forceCacheRefresh)
            {
                return await RequestRemoteHtml(applicationId, forUrl, controlId, selectedSection, textSize, isLibraryCatalogueRequest).ConfigureAwait(false);
            }

            // Return the HTML
            return _cacheProvider.ReadHtmlFromCache(applicationId, controlId, selectedSection, textSize, isLibraryCatalogueRequest);
        }

        /// <summary>
        /// Requests the remote HTML.
        /// </summary>
        /// <param name="applicationId">A string which identifies the application making the request</param>
        /// <param name="forUrl">The page to request the control for (usually the current page)</param>
        /// <param name="controlId">A key identifying the control to cache.</param>
        /// <param name="selectedSection">The selected section.</param>
        /// <param name="textSize">The current setting for the site's text size feature.</param>
        /// <param name="isLibraryCatalogueRequest"><c>true</c> if the request is from a public catalogue machine in a library</param>
        private async Task<string> RequestRemoteHtml(string applicationId, Uri forUrl, string controlId, string selectedSection, int textSize, bool isLibraryCatalogueRequest)
        {
            string html = string.Empty;
            try
            {
                // Get the URL to request the cached control from.
                // Include text size so that header knows which links to apply
                Uri urlToRequest = new Uri(forUrl, String.Format(CultureInfo.CurrentCulture, _masterPageControlUrl.ToString(), controlId));
                applicationId = HttpUtility.UrlEncode(applicationId.ToLower(CultureInfo.CurrentCulture).TrimEnd('/'));
                var query = HttpUtility.ParseQueryString(urlToRequest.Query);
                query.Add("section", selectedSection);
                query.Add("host", forUrl.Host);
                query.Add("textsize", textSize.ToString(CultureInfo.InvariantCulture));
                query.Add("path", applicationId);
                urlToRequest = new Uri(urlToRequest.Scheme + "://" + urlToRequest.Authority + urlToRequest.AbsolutePath + "?" + query);

                try
                {
                    // Create the request. Pass current user-agent so that library catalogue PCs can be detected by the remote script.
                    if (_httpClient == null)
                    {
                        _httpClient = _httpClientProvider.GetHttpClient();
                    }
                    using (var request = new HttpRequestMessage(HttpMethod.Get, urlToRequest))
                    {
                        if (!String.IsNullOrEmpty(_userAgent))
                        {
                            request.Headers.UserAgent.ParseAdd(_userAgent);
                        }

                        using (var response = await _httpClient.SendAsync(request).ConfigureAwait(false))
                        {
                            if (!response.IsSuccessStatusCode)
                            {
                                // Report failure code as if it was an exception
                                new HttpRequestException($"Request to URL {urlToRequest} returned {response.StatusCode} {response.ReasonPhrase}")
                                    .ToExceptionless().Submit();
                            }

                            html = await response.Content.ReadAsStringAsync();
                            if (_cacheProvider != null)
                            {
                                _cacheProvider.SaveRemoteHtmlToCache(applicationId, controlId, selectedSection, textSize, isLibraryCatalogueRequest, html);
                            }
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Publish exception, otherwise it just disappears as async method has no calling code to throw to.
                    ex.Data.Add("URL which failed", urlToRequest.ToString());
                    ex.ToExceptionless().Submit();
                }
            }
            catch (UriFormatException ex)
            {
                ex.Data.Add("URL which failed", String.Format(CultureInfo.CurrentCulture, _masterPageControlUrl.ToString(), controlId));
                ex.ToExceptionless().Submit();
            }
            return html;
        }
    }
}