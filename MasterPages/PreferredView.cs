
namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// Represents user's saved preference for view of site
    /// </summary>
    internal enum PreferredView
    {
        /// <summary>
        /// Preference not yet determined
        /// </summary>
        Unknown,

        /// <summary>
        /// Prefers mobile site
        /// </summary>
        Mobile,

        /// <summary>
        /// Prefers desktop site
        /// </summary>
        Desktop,

        /// <summary>
        /// Prefers a version with no formatting
        /// </summary>
        Plain,

        /// <summary>
        /// Prefers a full-screen view
        /// </summary>
        FullScreen
    }
}