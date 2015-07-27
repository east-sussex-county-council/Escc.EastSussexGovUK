using System;
using EsccWebTeam.Cms;
using Microsoft.ContentManagement.Publishing;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Load report, apply, pay links from CMS
    /// </summary>
    public partial class ReportApplyPay : System.Web.UI.UserControl
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
                FindReportApplyPay();
            }

            // These are just controls on the page so should always be not null, yet they were null on esascweb01v
            if (this.rap != null && this.report != null && this.apply != null && this.pay != null)
            {
                this.rap.Visible = (this.report.Visible || this.apply.Visible || this.pay.Visible);
            }
        }

        /// <summary>
        /// Starting with the current channel, look up the tree until we find an instance of the "report apply pay" template
        /// </summary>
        private void FindReportApplyPay()
        {
            var channel = CmsUtilities.GetCurrentChannel();
            while (channel != null)
            {
                foreach (Posting p in channel.Postings)
                {
                    if (p.Template.Guid == "{223D21DF-F9F9-4616-B42F-D6C586F22080}")
                    {
                        ShowReportApplyPay(p);
                        return;
                    }
                }

                channel = channel.Parent;
            }
            return;
        }

        /// <summary>
        /// Shows the report apply pay links (from the report apply pay template) on the current page.
        /// </summary>
        /// <param name="p">The p.</param>
        private void ShowReportApplyPay(Posting p)
        {
            var reportHtml = CmsUtilities.ShouldBeUnorderedList(p.Placeholders["defReport"].Datasource.RawContent, null);
            if (!String.IsNullOrEmpty(reportHtml))
            {
                this.report.Visible = true;
                this.reportLinks.Text = reportHtml;
            }

            var applyHtml = CmsUtilities.ShouldBeUnorderedList(p.Placeholders["defApply"].Datasource.RawContent, null);
            if (!String.IsNullOrEmpty(applyHtml))
            {
                this.apply.Visible = true;
                this.applyLinks.Text = applyHtml;
            }

            var payHtml = CmsUtilities.ShouldBeUnorderedList(p.Placeholders["defPay"].Datasource.RawContent, null);
            if (!String.IsNullOrEmpty(payHtml))
            {
                this.pay.Visible = true;
                this.payLinks.Text = payHtml;
            }

            if (CmsHttpContext.Current.Mode == PublishingMode.Unpublished)
            {
                this.source.Visible = true;
                this.source.InnerHtml = String.Format("<p>Report, apply, pay is from <a href=\"{0}\">{1}</a></p>", p.Url, p.Parent.DisplayName == "Channels" ? "Home" : p.Parent.DisplayName);
            }
        }
    }
}