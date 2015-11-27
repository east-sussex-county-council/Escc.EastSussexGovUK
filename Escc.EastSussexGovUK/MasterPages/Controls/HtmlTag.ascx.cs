using System;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// The HTML tag, including different versions for IE 
    /// </summary>
    public partial class HtmlTag : System.Web.UI.UserControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = new EastSussexGovUKContext();
            GoogleTagManagerContainerId = context.GoogleTagManagerContainerId;
        }

        /// <summary>
        /// Gets or sets the Google Tag Manager container id.
        /// </summary>
        /// <value>
        /// The Google Tag Manager container id.
        /// </value>
        protected string GoogleTagManagerContainerId { get; set; }
    }
}