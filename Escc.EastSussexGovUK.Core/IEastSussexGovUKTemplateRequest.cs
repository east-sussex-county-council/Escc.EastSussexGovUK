using System.Threading.Tasks;
using Escc.EastSussexGovUK.Features;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// A helper to compose the elements required to apply the EastSussexGovUK site template
    /// </summary>
    public interface IEastSussexGovUKTemplateRequest
    {
        /// <summary>
        /// Reads the HTML controls required for the website template.
        /// </summary>
        /// <returns></returns>
        Task<TemplateHtml> RequestTemplateHtmlAsync();

        /// <summary>
        /// Reads web chat configuration from the provided or default <see cref="IWebChatSettingsService"/>.
        /// </summary>
        /// <returns>Web chat settings, or <c>null</c> if the <see cref="IWebChatSettingsService"/> was not configured</returns>
        Task<WebChatSettings> RequestWebChatSettingsAsync();
    }
}