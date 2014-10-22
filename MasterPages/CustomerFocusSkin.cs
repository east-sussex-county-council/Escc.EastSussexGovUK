namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// Skin applied to pages on www.eastsussex.gov.uk based on the work of the Government Digital Service
    /// </summary>
    public class CustomerFocusSkin : IEsccWebsiteSkin
    {
        /// <summary>
        /// Class or classes applied to supporting content with standard text formatting
        /// </summary>
        public string SupportingTextContentClass
        {
            get { return "text-content content-small content-medium"; }
        }
    }
}