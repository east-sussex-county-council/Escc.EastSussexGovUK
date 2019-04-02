using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Views;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// Helper class to compose the elements required to apply the EastSussexGovUK site template
    /// </summary>
    public class EastSussexGovUKTemplateRequest : IEastSussexGovUKTemplateRequest
    {
        private readonly HttpRequest _request;
        private readonly Uri _requestUrl;
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
        /// <param name="httpContextAccessor">A method of getting the web request which requires a response with the template applied.</param>
        /// <param name="breadcrumbProvider">A method of getting the breadcrumb trail, which provides the context of the page requested within the site.</param>
        /// <param name="viewSelector">A method of selecting the layout view to be applied.</param>
        /// <param name="htmlProvider">A method of getting HTML to make up the template. If not set, the remote template will be loaded based on settings in <c>remoteMasterPageSettings</c>.</param>
        /// <param name="webChatSettingsService">A method of getting web chat configuration. If not set, they will be read from a URL specified in <c>webChatRequestSettings</c>.</param>
        /// <param name="textSize">A method of getting the current text size. If not set, this will be read from a cookie accessed via <c>httpContextAccessor</c>.</param>
        /// <param name="libraryContext">A method of determining whether this request comes from a catalogue machine situated in a library. If not set, this will be based on the user agent accessed via <c>httpContextAccessor</c>.</param>
        /// <exception cref="ArgumentNullException">httpContextAccessor or breadcrumbProvider or httpClientProvider or viewSelector</exception>
        public EastSussexGovUKTemplateRequest(
            IHttpContextAccessor httpContextAccessor,
            IViewSelector viewSelector,
            IHtmlControlProvider htmlProvider
,
            IBreadcrumbProvider breadcrumbProvider,
            ILibraryCatalogueContext libraryContext,
            ITextSize textSize,
            IWebChatSettingsService webChatSettingsService)
        {
            _request = httpContextAccessor?.HttpContext?.Request ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _requestUrl = new Uri(_request.GetDisplayUrl());

            if (viewSelector == null) { throw new ArgumentNullException(nameof(viewSelector)); }
            _esccWebsiteView = viewSelector.CurrentViewIs(viewSelector.SelectView(_requestUrl, _request.Headers["User-Agent"].ToString()));

            _htmlProvider = htmlProvider ?? throw new ArgumentNullException(nameof(htmlProvider));
            _breadcrumbProvider = breadcrumbProvider ?? throw new ArgumentNullException(nameof(breadcrumbProvider));

            _libraryContext = libraryContext;
            _textSize = textSize;
            _webChatSettingsService = webChatSettingsService;
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
                if (_webChatSettings != null)
                {
                    _webChatSettings.PageUrl = new Uri(_requestUrl.AbsolutePath, UriKind.Relative);
                }
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

            var textSize = _textSize?.CurrentTextSize();
            if (textSize == null) { textSize = 1; }
            var isLibraryCatalogueRequest = _libraryContext?.RequestIsFromLibraryCatalogueMachine() ?? false;

            _templateHtml = new TemplateHtml();
            var applicationId = String.IsNullOrEmpty(_request.PathBase.ToString()) ? "/" : _request.PathBase.ToString();
            if (_esccWebsiteView == EsccWebsiteView.Desktop)
            {
                var htmlTagTask = _htmlProvider.FetchHtmlForControl(applicationId, _requestUrl, "HtmlTag", _breadcrumbProvider, textSize.Value, isLibraryCatalogueRequest);
                var metadataTask = _htmlProvider.FetchHtmlForControl(applicationId, _requestUrl, "MetadataDesktop", _breadcrumbProvider, textSize.Value, isLibraryCatalogueRequest);
                var aboveHeaderTask = _htmlProvider.FetchHtmlForControl(applicationId, _requestUrl, "AboveHeaderDesktop", _breadcrumbProvider, textSize.Value, isLibraryCatalogueRequest);
                var headerTask = _htmlProvider.FetchHtmlForControl(applicationId, _requestUrl, "HeaderDesktop", _breadcrumbProvider, textSize.Value, isLibraryCatalogueRequest);
                var footerTask = _htmlProvider.FetchHtmlForControl(applicationId, _requestUrl, "FooterDesktop", _breadcrumbProvider, textSize.Value, isLibraryCatalogueRequest);
                var scriptsTask = _htmlProvider.FetchHtmlForControl(applicationId, _requestUrl, "ScriptsDesktop", _breadcrumbProvider, textSize.Value, isLibraryCatalogueRequest);

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
                var htmlTagTask = _htmlProvider.FetchHtmlForControl(applicationId, _requestUrl, "HtmlTag", _breadcrumbProvider, textSize.Value, isLibraryCatalogueRequest);
                var metadataTask = _htmlProvider.FetchHtmlForControl(applicationId, _requestUrl, "MetadataFullScreen", _breadcrumbProvider, textSize.Value, isLibraryCatalogueRequest);
                var headerTask = _htmlProvider.FetchHtmlForControl(applicationId, _requestUrl, "HeaderFullScreen", _breadcrumbProvider, textSize.Value, isLibraryCatalogueRequest);
                var scriptsTask = _htmlProvider.FetchHtmlForControl(applicationId, _requestUrl, "ScriptsFullScreen", _breadcrumbProvider, textSize.Value, isLibraryCatalogueRequest);

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