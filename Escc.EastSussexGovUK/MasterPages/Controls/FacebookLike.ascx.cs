using System;
using System.Web;
using Escc.Html;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Displays a Facebook Like box when the <see cref="FacebookPage"/> property is not empty
    /// </summary>
    public partial class FacebookLike : System.Web.UI.UserControl
    {
        /// <summary>
        /// Gets or sets the URL of the Facebook page
        /// </summary>
        /// <value>The Facebook page URL.</value>
        public string FacebookPage { get; set; }

        /// <summary>
        /// Gets or sets whether to show faces.
        /// </summary>
        /// <value><c>true</c> to show faces; otherwise, <c>false</c>.</value>
        public bool ShowFaces { get; set; }

        /// <summary>
        /// Gets or sets whether to show a feed from the page.
        /// </summary>
        /// <value><c>true</c> to show feed; otherwise, <c>false</c>.</value>
        public bool ShowFeed { get; set; }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(FacebookPage))
            {
                this.facebook.Attributes["data-href"] = this.FacebookPage;
                this.facebook.Attributes["data-show-faces"] = this.ShowFaces ? "true" : "false";
                this.facebook.Attributes["data-stream"] = this.ShowFeed ? "true" : "false";

                this.fb.HRef = HttpUtility.HtmlEncode(this.FacebookPage.ToString());
                this.fb.InnerHtml = HttpUtility.HtmlEncode(new HtmlLinkFormatter().AbbreviateUrl(new Uri(this.FacebookPage)));

                this.Visible = true;
            }
            else
            {
                this.Visible = false;
            }
        }
    }
}