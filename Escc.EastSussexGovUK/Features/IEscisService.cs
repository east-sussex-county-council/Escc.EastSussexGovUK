namespace Escc.EastSussexGovUK.Features
{
    /// <summary>
    /// Service to control use of the ESCIS search widget
    /// </summary>
    public interface IEscisService
    {
        /// <summary>
        /// Determines whether to show the ESCIS search widget
        /// </summary>
        /// <returns></returns>
        bool ShowSearch();
    }
}