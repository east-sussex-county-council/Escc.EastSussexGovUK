namespace Escc.EastSussexGovUK.MasterPages.Features
{
    /// <summary>
    /// Service to control use of the EastSussex1Space search
    /// </summary>
    public interface IEastSussex1SpaceService
    {
        /// <summary>
        /// Determines whether to show the EastSussex1Space search 
        /// </summary>
        /// <returns></returns>
        bool ShowSearch();
    }
}