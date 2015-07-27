using System;
using System.Web;
using System.Web.UI;
using EsccWebTeam.Cms;
using EsccWebTeam.Cms.Placeholders;
using Microsoft.ContentManagement.Publishing;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Load social media widgets from settings in CMS
    /// </summary>
    public partial class SocialMediaDesktop : System.Web.UI.UserControl
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
                Posting p = FindSocialMedia();
                if (p != null)
                {
                    ShowSocialMedia(p);
                }
            }

            if (this.social != null)
            {
                this.social.Visible = (this.social.Controls.Count > 0);
            }
        }

        /// <summary>
        /// Starting with the current channel, look up the tree until we find an instance of the "report apply pay" template
        /// </summary>
        internal static Posting FindSocialMedia()
        {
            var channel = CmsUtilities.GetCurrentChannel();
            while (channel != null)
            {
                foreach (Posting p in channel.Postings)
                {
                    if (p.Template.Guid == "{1584DE25-5842-4A3F-8FD2-718C1C1FE931}")
                    {
                        return p;
                    }
                }

                channel = channel.Parent;
            }
            return null;
        }

        /// <summary>
        /// Shows the report apply pay links (from the report apply pay template) on the current page.
        /// </summary>
        /// <param name="p">The p.</param>
        private void ShowSocialMedia(Posting p)
        {

            var socialSettings = SocialMediaPlaceholderControl.GetValue(p.Placeholders["defSocial"]);
            if (socialSettings.FacebookLikePosition == 1)
            {
                ShowFacebook(socialSettings);
                ShowTwitter(socialSettings);
            }
            else
            {
                ShowTwitter(socialSettings);
                ShowFacebook(socialSettings);
            }

            if (CmsHttpContext.Current.Mode == PublishingMode.Unpublished)
            {
                this.source.Visible = true;
                this.source.InnerHtml = String.Format("<p>Social media is from <a href=\"{0}\">{1}</a></p>", p.Url, p.Parent.DisplayName == "Channels" ? "Home" : p.Parent.DisplayName);
            }
        }

        private void ShowFacebook(SocialMediaPlaceholderValue socialSettings)
        {
            if (socialSettings.FacebookLikeUrl != null)
            {
                var facebookLike = (FacebookLike)Page.LoadControl("~/masterpages/controls/facebooklike.ascx");
                facebookLike.FacebookPage = socialSettings.FacebookLikeUrl.ToString();
                facebookLike.ShowFaces = socialSettings.FacebookShowFaces;
                facebookLike.ShowFeed = socialSettings.FacebookShowFeed;
                this.social.Controls.Add(facebookLike);
            }
        }

        private void ShowTwitter(SocialMediaPlaceholderValue socialSettings)
        {
            if (!String.IsNullOrEmpty(socialSettings.TwitterWidget))
            {
                // HTML is stored as HTML encoded string because it's not well-formed XML, so decode it before display
                this.social.Controls.Add(new LiteralControl("<div class=\"supporting\">" + HttpUtility.HtmlDecode(socialSettings.TwitterWidget) + "</div>"));
            }
            else if (!String.IsNullOrEmpty(socialSettings.TwitterSearch))
            {
                var twitterSearch = (TwitterSearch)Page.LoadControl("~/masterpages/controls/twittersearch.ascx");
                twitterSearch.Search = socialSettings.TwitterSearch;
                this.social.Controls.Add(twitterSearch);
            }
        }
    }
}