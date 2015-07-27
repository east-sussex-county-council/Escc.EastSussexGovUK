
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Standard HTML to include above the header on the mobile template
    /// </summary>
    public partial class AboveHeaderMobile : System.Web.UI.UserControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Link to choose.ashx within the scope of the current application
            var applicationPath = HttpRuntime.AppDomainAppVirtualPath.ToLower(CultureInfo.CurrentCulture).TrimEnd('/');

            // Context may be passed in on the querystring as part of the remote template.
            if (!String.IsNullOrEmpty(Request.QueryString["path"]))
            {
                applicationPath = Regex.Replace(Request.QueryString["path"], "[^a-z0-9-./]", String.Empty);
            }

            this.switchView.HRef = applicationPath + this.switchView.HRef;
        }
    }
}