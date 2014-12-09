using System;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Page header bar for the full screen master page
    /// </summary>
    public partial class HeaderFullScreen : System.Web.UI.UserControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Preprend the base URL if specified (which it should be if this is a subdomain of eastsussex.gov.uk)
            var context = new EastSussexGovUKContext();
            if (context.BaseUrl != null)
            {
                var urlPrefix = context.BaseUrl.ToString().TrimEnd('/');
                this.homeSmall.HRef = urlPrefix + this.homeSmall.HRef;
                this.contact.HRef = urlPrefix + this.contact.HRef;

                // Because these are resources loaded by the page, rather than linking off to another page, 
                // ensure the URL is protocol relative
                var colon = urlPrefix.IndexOf(":", StringComparison.OrdinalIgnoreCase);
                if (colon > -1)
                {
                    urlPrefix = urlPrefix.Substring(colon + 1);
                }
                this.logoSmall.Text = urlPrefix + this.logoSmall.Text;
            }
        }
    }
}