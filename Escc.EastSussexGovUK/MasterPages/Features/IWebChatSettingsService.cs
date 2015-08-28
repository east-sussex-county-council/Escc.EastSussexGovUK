namespace Escc.EastSussexGovUK.MasterPages.Features
{
    /// <summary>
    /// Gets the settings for where to display the web chat feature
    /// </summary>
    public interface IWebChatSettingsService
    {
        /// <summary>
        /// Gets the settings for where to display the web chat feature
        /// </summary>
        /// <returns></returns>
        WebChatSettings ReadWebChatSettings();
    }
}