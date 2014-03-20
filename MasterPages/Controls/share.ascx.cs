using System;
using System.Globalization;
using System.Web.UI;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Links to share the current page on social media or by email
    /// </summary>
    public partial class share : System.Web.UI.UserControl
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            // Get site context
            var context = new EastSussexGovUKContext();

            // Pass parameters to comment and sharing links
            string encodedPageUrl = Server.UrlEncode(context.RequestUrl.ToString());

            // Link to comments form with a reference to this page
            // Do this on PreRender as hopefully Page.Title has been set by then
            string pageTitle = Server.UrlEncode(Server.HtmlDecode(Page.Title));

            if (this.facebook != null) this.facebook.HRef = String.Format(CultureInfo.CurrentCulture, this.facebook.HRef, encodedPageUrl);
            if (this.comment != null) this.comment.HRef = String.Format(this.comment.HRef, encodedPageUrl, pageTitle);
            if (this.email != null) this.email.HRef = String.Format(this.email.HRef, encodedPageUrl);
        }
    }
}