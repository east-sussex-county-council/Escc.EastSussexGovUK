using System;
using EsccWebTeam.Cms;
using Microsoft.ContentManagement.Publishing;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Load alert messages
    /// </summary>
    public partial class AlertFullScreen : System.Web.UI.UserControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Hide API from non-CMS boxes
            bool alertFoundInCms = false;
            if (CmsUtilities.IsCmsEnabled())
            {
                alertFoundInCms = FindAlert();
            }

            // Need to be careful alert message does not wrap even on mobile, as there's a fixed pixel depth
            // for it to occupy. Closures message alone is OK but if any other alert display a generic link.
            if (this.closures.Visible && !alertFoundInCms)
            {
                // Let the closure link display
                this.alert.Visible = true;
                this.disruption.Visible = false;
            }
            else if (alertFoundInCms)
            {
                // No room for both, just display generic link
                this.closures.Visible = false;
                this.alert.Visible = true;
                this.disruption.Visible = true;
            }
            else
            {
                this.alert.Visible = false;
            }
        }

        /// <summary>
        /// Starting with the current channel, look up the tree until we find an instance of the "alert" template
        /// </summary>
        private bool FindAlert()
        {
            var channel = CmsUtilities.GetCurrentChannel();
            while (channel != null)
            {
                foreach (Posting p in channel.Postings)
                {
                    if (p.Template.Guid == "{330D6824-BEC3-4A52-9163-BEB0CA6416E1}")
                    {
                        var alertHtml = p.Placeholders["defAlert"].Datasource.RawContent;
                        if (!String.IsNullOrEmpty(alertHtml))
                        {
                            return true;
                        }
                    }
                }

                channel = channel.Parent;
            }
            return false;
        }
    }
}