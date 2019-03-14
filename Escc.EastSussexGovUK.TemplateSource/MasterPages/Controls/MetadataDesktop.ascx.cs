using System;
using System.Web;
using Escc.EastSussexGovUK.Mvc;

namespace Escc.EastSussexGovUK.TemplateSource.MasterPages.Controls
{
    /// <summary>
    /// Standard elements which appear in the &lt;head /&gt; section of the desktop master page
    /// </summary>
    public partial class MetadataDesktop : System.Web.UI.UserControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Preprend the base URL if specified (which it should be if this is a subdomain of eastsussex.gov.uk)
            var siteContext = new HostingEnvironmentContext(HttpContext.Current.Request.Url);
            if (siteContext.BaseUrl != null)
            {
                var urlPrefix = siteContext.BaseUrl.ToString().TrimEnd('/');
                apple.Text = urlPrefix + apple.Text;
                windows.Text = urlPrefix + windows.Text;
                search.Text = urlPrefix + search.Text;
            }
        }
    }
}