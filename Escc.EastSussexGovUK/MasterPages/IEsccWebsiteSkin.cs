
namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// A skin is applied to content between the header and footer of www.eastsussex.gov.uk
    /// </summary>
    public interface IEsccWebsiteSkin
    {
        /// <summary>
        /// Class or classes applied to supporting content with standard text formatting
        /// </summary>
        string SupportingTextContentClass { get; }
    }
}
