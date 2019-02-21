using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Views;
using Escc.Net;
using Escc.Net.Configuration;

namespace Escc.EastSussexGovUK.Mvc
{
    /// <summary>
    /// Helper class to compose the elements required to apply the EastSussexGovUK site template
    /// </summary>
    public class EastSussexGovUKTemplateRequest : IEastSussexGovUKTemplateRequest
    {
        private readonly HttpRequestBase _request;
        private readonly EsccWebsiteView _esccWebsiteView;
        private readonly IWebChatSettingsService _webChatSettingsService;
        private readonly ITextSize _textSize;
        private readonly ILibraryCatalogueContext _libraryContext;
        private readonly IBreadcrumbProvider _breadcrumbProvider;
        private readonly IHtmlControlProvider _htmlProvider;
        private WebChatSettings _webChatSettings;
        private TemplateHtml _templateHtml;

        /// <summary>
        /// Initialises a new instance of <see cref="EastSussexGovUKTemplateRequest"/>
        /// </summary>
        /// <param name="request">The web request which requires a response with the template applied.</param>
        /// <param name="esccWebsiteView">The layout to be applied. If not set or set to <see cref="EsccWebsiteView.Unknown"/>, the documented rules for selecting a layout using <see cref="MvcViewSelector"/> will be used.</param>
        /// <param name="htmlProvider">A method of getting HTML to make up the template. If not set, the remote template will be loaded based on settings in <c>web.config</c>.</param>
        /// <param name="webChatSettingsService">A method of getting web chat configuration. If not set, they will be read from a URL specified in <c>web.config</c>.</param>
        /// <param name="breadcrumbProvider">A method of getting the breadcrumb trail, which provides the context of the page requested within the site. If not set, the breadcrumb trail will be read from <c>web.config</c></param>
        /// <param name="textSize">A method of getting the current text size. If not set, this will be read from a cookie.</param>
        /// <param name="libraryContext">A method of determining whether this request comes from a catalogue machine situated in a library. If not set, this will be based on the user agent.</param>
        /// <param name="proxyProvider">A method of getting an optional proxy server that may be required to connect to remote resources. If not set, this will be read from <c>web.config</c> according to the rules documented for <c>Escc.Net.Configuration</c></param>
        /// <exception cref="ArgumentNullException">request</exception>
        public EastSussexGovUKTemplateRequest(HttpRequestBase request,
            EsccWebsiteView esccWebsiteView = EsccWebsiteView.Unknown,
            IHtmlControlProvider htmlProvider = null,
            IWebChatSettingsService webChatSettingsService = null,
            IBreadcrumbProvider breadcrumbProvider = null,
            ITextSize textSize = null,
            ILibraryCatalogueContext libraryContext = null,
            IProxyProvider proxyProvider = null)
        {
            _request = request ?? throw new ArgumentNullException(nameof(request));

            _esccWebsiteView = esccWebsiteView;
            if (esccWebsiteView == EsccWebsiteView.Unknown) {
                var viewSelector = new MvcViewSelector();
                _esccWebsiteView = viewSelector.CurrentViewIs(viewSelector.SelectView(request.Url, request.UserAgent));
            }

            _textSize = textSize ?? new TextSize(request.Cookies?["textsize"]?.Value, request.QueryString);
            _libraryContext = libraryContext ?? new LibraryCatalogueContext(request.UserAgent);
            _breadcrumbProvider = breadcrumbProvider ?? new BreadcrumbTrailFromConfig(request.Url);

            if (proxyProvider == null) proxyProvider = new ConfigurationProxyProvider();
            if (htmlProvider == null)
            {
                var masterPageSettings = new RemoteMasterPageSettingsFromConfig();
                var forceCacheRefresh = (request.QueryString["ForceCacheRefresh"] == "1"); // Provide a way to force an immediate update of the cache
                _htmlProvider = new RemoteMasterPageHtmlProvider(masterPageSettings.MasterPageControlUrl(), proxyProvider, request.UserAgent, masterPageSettings.RequestTimeout(), new RemoteMasterPageMemoryCacheProvider(masterPageSettings.CacheTimeout()), forceCacheRefresh);
            }
            else
            {
                _htmlProvider = htmlProvider;
            }
            
            _webChatSettingsService = webChatSettingsService;
            if (_webChatSettingsService == null)
            {
                var webChatRequestSettings = new HostingEnvironmentContext(request.Url);
                if (webChatRequestSettings.WebChatSettingsUrl != null)
                {
                    _webChatSettingsService = new WebChatSettingsFromApi(webChatRequestSettings.WebChatSettingsUrl, proxyProvider, new ApplicationCacheStrategy<WebChatSettings>(TimeSpan.FromMinutes(webChatRequestSettings.WebChatSettingsCacheDuration)));
                }
            }
        }

        /// <summary>
        /// Reads web chat configuration from the provided or default <see cref="IWebChatSettingsService"/>. The result is cached for the lifetime of this instance.
        /// </summary>
        /// <returns>Web chat settings, or <c>null</c> if the <see cref="IWebChatSettingsService"/> was not configured</returns>
        public async Task<WebChatSettings> RequestWebChatSettingsAsync()
        {
            if (_webChatSettings != null) return _webChatSettings;

            if (_webChatSettingsService != null)
            {
                _webChatSettings = await _webChatSettingsService.ReadWebChatSettings().ConfigureAwait(false);
                _webChatSettings.PageUrl = new Uri(_request.Url.AbsolutePath, UriKind.Relative);
                return _webChatSettings;
            }
            return null;
        }

        /// <summary>
        /// Reads the HTML controls required for the website template. The result is cached for the lifetime of this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<TemplateHtml> RequestTemplateHtmlAsync()
        {
            if (_templateHtml != null) return _templateHtml;

            var textSize = _textSize.CurrentTextSize();
            var isLibraryCatalogueRequest = _libraryContext.RequestIsFromLibraryCatalogueMachine();

            _templateHtml = new TemplateHtml();
            if (_esccWebsiteView == EsccWebsiteView.Desktop)
            {
                var htmlTagTask = _htmlProvider.FetchHtmlForControl(HttpRuntime.AppDomainAppVirtualPath, _request.Url, "HtmlTag", _breadcrumbProvider, textSize, isLibraryCatalogueRequest);
                var metadataTask = _htmlProvider.FetchHtmlForControl(HttpRuntime.AppDomainAppVirtualPath, _request.Url, "MetadataDesktop", _breadcrumbProvider, textSize, isLibraryCatalogueRequest);
                var aboveHeaderTask = _htmlProvider.FetchHtmlForControl(HttpRuntime.AppDomainAppVirtualPath, _request.Url, "AboveHeaderDesktop", _breadcrumbProvider, textSize, isLibraryCatalogueRequest);
                var headerTask = _htmlProvider.FetchHtmlForControl(HttpRuntime.AppDomainAppVirtualPath, _request.Url, "HeaderDesktop", _breadcrumbProvider, textSize, isLibraryCatalogueRequest);
                var footerTask = _htmlProvider.FetchHtmlForControl(HttpRuntime.AppDomainAppVirtualPath, _request.Url, "FooterDesktop", _breadcrumbProvider, textSize, isLibraryCatalogueRequest);
                var scriptsTask = _htmlProvider.FetchHtmlForControl(HttpRuntime.AppDomainAppVirtualPath, _request.Url, "ScriptsDesktop", _breadcrumbProvider, textSize, isLibraryCatalogueRequest);

                var results = await Task.WhenAll(htmlTagTask, metadataTask, aboveHeaderTask, headerTask, footerTask, scriptsTask).ConfigureAwait(false);

                _templateHtml.HtmlTag = new HtmlString(results[0]);
                _templateHtml.Metadata = new HtmlString(results[1]);
                _templateHtml.AboveHeader = new HtmlString(results[2]);
                _templateHtml.Header = new HtmlString(results[3]);
                _templateHtml.Footer = new HtmlString(results[4]);
                _templateHtml.Scripts = new HtmlString(results[5]);
            }
            else if (_esccWebsiteView == EsccWebsiteView.FullScreen)
            {
                var htmlTagTask = _htmlProvider.FetchHtmlForControl(HttpRuntime.AppDomainAppVirtualPath, _request.Url, "HtmlTag", _breadcrumbProvider, textSize, isLibraryCatalogueRequest);
                var metadataTask = _htmlProvider.FetchHtmlForControl(HttpRuntime.AppDomainAppVirtualPath, _request.Url, "MetadataFullScreen", _breadcrumbProvider, textSize, isLibraryCatalogueRequest);
                var headerTask = _htmlProvider.FetchHtmlForControl(HttpRuntime.AppDomainAppVirtualPath, _request.Url, "HeaderFullScreen", _breadcrumbProvider, textSize, isLibraryCatalogueRequest);
                var scriptsTask = _htmlProvider.FetchHtmlForControl(HttpRuntime.AppDomainAppVirtualPath, _request.Url, "ScriptsFullScreen", _breadcrumbProvider, textSize, isLibraryCatalogueRequest);

                var results = await Task.WhenAll(htmlTagTask, metadataTask, headerTask, scriptsTask).ConfigureAwait(false);

                _templateHtml.HtmlTag = new HtmlString(results[0]);
                _templateHtml.Metadata = new HtmlString(results[1]);
                _templateHtml.Header = new HtmlString(results[2]);
                _templateHtml.Scripts = new HtmlString(results[3]);
            }
            return _templateHtml;
        }
    }
}