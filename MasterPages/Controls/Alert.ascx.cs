using System;
using EsccWebTeam.Cms;
using Microsoft.ContentManagement.Publishing;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Load alert messages
    /// </summary>
    public partial class Alert : System.Web.UI.UserControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Hide API from non-CMS boxes
            if (CmsUtilities.IsCmsEnabled())
            {
                FindAlert();
            }

            this.alert.Visible = (!String.IsNullOrEmpty(this.alertText.Text) || this.closures.Visible);
        }

        /// <summary>
        /// Starting with the current channel, look up the tree until we find an instance of the "alert" template
        /// </summary>
        private void FindAlert()
        {
            var channel = CmsUtilities.GetCurrentChannel();
            while (channel != null)
            {
                foreach (Posting p in channel.Postings)
                {
                    if (p.Template.Guid == "{330D6824-BEC3-4A52-9163-BEB0CA6416E1}")
                    {
                        ShowAlert(p);
                        return;
                    }
                }

                channel = channel.Parent;
            }
            return;
        }

        /// <summary>
        /// Shows the report apply pay links (from the alert template) on the current page.
        /// </summary>
        /// <param name="p">The p.</param>
        private void ShowAlert(Posting p)
        {
            var alertHtml = p.Placeholders["defAlert"].Datasource.RawContent;
            if (!String.IsNullOrEmpty(alertHtml))
            {
                this.alertText.Text = alertHtml;
            }

            if (CmsHttpContext.Current.Mode == PublishingMode.Unpublished)
            {
                this.source.Visible = true;
                this.source.InnerHtml = String.Format("<p>Alert is from <a href=\"{0}\">{1}</a></p>", p.Url, p.Parent.DisplayName == "Channels" ? "Home" : p.Parent.DisplayName);
            }
        }
    }
}