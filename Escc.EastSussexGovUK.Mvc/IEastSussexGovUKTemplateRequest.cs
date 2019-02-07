using System.Threading.Tasks;
using Escc.EastSussexGovUK.Features;

namespace Escc.EastSussexGovUK.Mvc
{
    public interface IEastSussexGovUKTemplateRequest
    {
        Task<TemplateHtml> RequestTemplateHtmlAsync();
        Task<WebChatSettings> RequestWebChatSettingsAsync();
    }
}