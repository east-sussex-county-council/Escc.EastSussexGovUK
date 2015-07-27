
namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// The default skin on www.eastsussex.gov.uk
    /// </summary>
    public class DefaultSkin : IEsccWebsiteSkin
    {
        /// <summary>
        /// Class or classes applied to supporting content with standard text formatting
        /// </summary>
        public string SupportingTextContentClass { get { return "supporting-text"; } }
    }
}