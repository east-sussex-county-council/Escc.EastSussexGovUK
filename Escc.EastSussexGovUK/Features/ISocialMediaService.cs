namespace Escc.EastSussexGovUK.Features
{
    /// <summary>
    /// Service for determining which social media widgets to display
    /// </summary>
    public interface ISocialMediaService
    {
        /// <summary>
        /// Gets settings for which social media widgets to display
        /// </summary>
        /// <returns></returns>
        SocialMediaSettings ReadSocialMediaSettings();
    }
}