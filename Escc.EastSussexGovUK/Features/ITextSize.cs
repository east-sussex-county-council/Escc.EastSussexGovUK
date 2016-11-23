namespace Escc.EastSussexGovUK.Features
{
    /// <summary>
    /// Manage the sitewide feature which changes the text size independently of browser zoom
    /// </summary>
    public interface ITextSize
    {
        /// <summary>
        /// Gets the current user-selected text size 
        /// </summary>
        /// <returns></returns>
        int CurrentTextSize();
    }
}