using System;
using System.Net.Http;
using System.Threading.Tasks;
using Escc.Net;
using Exceptionless;
using Newtonsoft.Json;

namespace Escc.EastSussexGovUK.Features
{
    /// <summary>
    /// Downloads web chat settings from a URL, expected (but not required) to be published as a Web API in Umbraco
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Features.IWebChatSettingsService" />
    public class WebChatSettingsFromApi : IWebChatSettingsService
    {
        private readonly Uri _apiUrl;
        private readonly ICacheStrategy<WebChatSettings> _cache;
        private static IHttpClientProvider _httpClientProvider;
        private static HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebChatSettingsFromApi" /> class.
        /// </summary>
        /// <param name="apiUrl">The API URL.</param>
        /// <param name="httpClientProvider">Strategy to get the HttpClient instance used for requests.</param>
        /// <param name="cache">The cache.</param>
        /// <exception cref="System.ArgumentNullException">apiUrl</exception>
        public WebChatSettingsFromApi(Uri apiUrl, IHttpClientProvider httpClientProvider, ICacheStrategy<WebChatSettings> cache)
        {
            _apiUrl = apiUrl ?? throw new ArgumentNullException(nameof(apiUrl));
            _httpClientProvider = httpClientProvider ?? throw new ArgumentNullException(nameof(httpClientProvider));
            _cache = cache;
        }

        /// <summary>
        /// Gets the settings for where to display the web chat feature
        /// </summary>
        /// <returns></returns>
        public async Task<WebChatSettings> ReadWebChatSettings()
        {
            try
            {
                var cachedSettings =_cache?.ReadFromCache("WebChatSettingsUrl");
                if (cachedSettings != null) return cachedSettings;

                if (_httpClient == null)
                {
                    _httpClient = _httpClientProvider.GetHttpClient();
                }
                var json = await _httpClient.GetStringAsync(_apiUrl).ConfigureAwait(false);
                var settings = JsonConvert.DeserializeObject<WebChatSettings>(json);

                _cache?.AddToCache("WebChatSettingsUrl", settings);    

                return settings;
            }
            catch (Exception exception)
            {
                // catch, report and suppress errors because we never want a check for web chat support to stop a page from loading
                exception.Data.Add("URL", _apiUrl.ToString());
                exception.ToExceptionless().Submit();
                return new WebChatSettings();
            }
        }
    }
}
