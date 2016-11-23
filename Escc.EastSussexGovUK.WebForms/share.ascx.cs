using System;
using System.Web;

namespace Escc.EastSussexGovUK.WebForms
{
    /// <summary>
    /// Links to share the current page on social media or by email
    /// </summary>
    public partial class share : System.Web.UI.UserControl
    {
        public string EncodedPageUrl { get; private set; }
        public string EncodedTitle { get; private set; }
        public string CssClass { get; set; }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            // Pass parameters to comment and sharing links
            EncodedPageUrl = Server.UrlEncode(HttpContext.Current.Request.Url.ToString());

            // Link to comments form with a reference to this page
            // Do this on PreRender as hopefully Page.Title has been set by then
            // strip double-quotes because when this link is picked up by a link checker and exported to CSV, the quotes are misinterpreted as a CSV delimiter
            EncodedTitle = Server.UrlEncode(Server.HtmlDecode(Page.Title.Replace("\"", String.Empty)));

            if (!String.IsNullOrEmpty(CssClass))
            {
                this.text.Attributes["class"] = CssClass;
            }
        }
    }
}