using System;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Header for the mobile template
    /// </summary>
    public partial class HeaderMobile : System.Web.UI.UserControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Preprend the base URL if specified (which it should be if this is a subdomain of eastsussex.gov.uk)
            var siteContext = new EastSussexGovUKContext();
            if (siteContext.BaseUrl != null)
            {
                var urlPrefix = siteContext.BaseUrl.ToString().TrimEnd('/');
                this.logoSmallLink.Text = urlPrefix + this.logoSmallLink.Text;
                this.logoSmall.Text = urlPrefix + this.logoSmall.Text;
                this.contact.HRef = urlPrefix + this.contact.HRef;
                this.searchUrl.Text = urlPrefix + this.searchUrl.Text;
                this.mobileHome.HRef = urlPrefix + this.mobileHome.HRef;
                this.mobileMenu.HRef = urlPrefix + this.mobileMenu.HRef;
            }
            else
            {
                // If no base URL we're on the main server. Are we on the home page, and therefore need to de-link the logo?
                var isHomePage = (siteContext.RequestUrl.AbsolutePath.StartsWith("/DEFAULT.", StringComparison.OrdinalIgnoreCase));
                this.logoSmallLinkOpen.Visible = !isHomePage;
                this.logoSmallLinkClose.Visible = !isHomePage;
                this.logoSmallLink.Visible = !isHomePage;
                this.logoSmallLinkEnd.Visible = !isHomePage;
                this.logoSmallDivOpen.Visible = isHomePage;
                this.logoSmallDivEnd.Visible = isHomePage;

                // Ensure small logo is an absolute URL because WorldPay loads and redisplays a confirmation page
                var urlPrefix = Request.Url.Scheme + "://" + Request.Url.Host;
                this.logoSmall.Text = urlPrefix + this.logoSmall.Text;
                if (this.logoSmallLink.Visible) this.logoSmallLink.Text = urlPrefix + this.logoSmallLink.Text;

            }
        }
    }
}