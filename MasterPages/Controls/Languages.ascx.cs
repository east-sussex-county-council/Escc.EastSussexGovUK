using System;
using System.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    public partial class Languages : System.Web.UI.UserControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var siteContext = new EastSussexGovUKContext();

            // Preprend the base URL if specified (which it should be if this is a subdomain of eastsussex.gov.uk)
            if (siteContext.BaseUrl != null)
            {
                var urlPrefix = siteContext.BaseUrl.ToString().TrimEnd('/');
                this.chinese.HRef = urlPrefix + this.chinese.HRef;
                this.arabic.HRef = urlPrefix + this.arabic.HRef;
                this.urdu.HRef = urlPrefix + this.urdu.HRef;
                this.kurdish.HRef = urlPrefix + this.kurdish.HRef;
                this.portugese.HRef = urlPrefix + this.portugese.HRef;
                this.polish.HRef = urlPrefix + this.polish.HRef;
                this.slovakian.HRef = urlPrefix + this.slovakian.HRef;
                this.turkish.HRef = urlPrefix + this.turkish.HRef;
            }

            // Append the URL of the page to be translated
            var encodedUrl = "&amp;url=" + HttpUtility.UrlEncode(siteContext.RequestUrl.ToString());

            this.chinese.HRef += encodedUrl;
            this.arabic.HRef += encodedUrl;
            this.urdu.HRef += encodedUrl;
            this.kurdish.HRef += encodedUrl;
            this.portugese.HRef += encodedUrl;
            this.polish.HRef += encodedUrl;
            this.slovakian.HRef += encodedUrl;
            this.turkish.HRef += encodedUrl;
        }
    }
}