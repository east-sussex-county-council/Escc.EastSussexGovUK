using System;
using System.Web.UI.HtmlControls;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Petitions list in the site footer
    /// </summary>
    public partial class Petitions : System.Web.UI.UserControl
    {
        private EastSussexGovUKContext context = new EastSussexGovUKContext();

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // If there's a base URL set, we need to change the "start a petition" link
            if (context.BaseUrl != null)
            {
                this.feed.PreRender += new EventHandler(feed_PreRender);
            }
        }

        /// <summary>
        /// Handles the PreRender event of the feed control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void feed_PreRender(object sender, EventArgs e)
        {
            // Get the "start a petition" link inside the footer template
            var footer = feed.FindControl("footer");
            if (footer == null) return;
            var start = footer.FindControl("start") as HtmlAnchor;
            if (start == null) return;

            // Prepend the base URL from web.config
            var urlPrefix = context.BaseUrl.ToString().TrimEnd('/');
            start.HRef = urlPrefix + start.HRef;
        }
    }
}