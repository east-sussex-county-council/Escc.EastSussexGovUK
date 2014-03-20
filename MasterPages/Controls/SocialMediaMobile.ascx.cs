using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using EsccWebTeam.Cms;
using EsccWebTeam.Cms.Placeholders;
using EsccWebTeam.Data.Web;
using Microsoft.ContentManagement.Publishing;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Load social media widgets from settings in CMS
    /// </summary>
    public partial class SocialMediaMobile : System.Web.UI.UserControl
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
                Posting p = SocialMediaDesktop.FindSocialMedia();
                if (p != null)
                {
                    ShowSocialMedia(p);
                }
            }

            if (this.social != null)
            {
                this.social.Visible = (this.fbContainer.Visible || this.twContainer.Visible);
            }
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
                SwopPositions();
                ShowTwitter(socialSettings);
                ShowFacebook(socialSettings);
            }

            if (CmsHttpContext.Current.Mode == PublishingMode.Unpublished)
            {
                this.source.Visible = true;
                this.source.InnerHtml = String.Format("<p>Social media is from <a href=\"{0}\">{1}</a></p>", p.Url, p.Parent.DisplayName == "Channels" ? "Home" : p.Parent.DisplayName);
            }
        }

        private void SwopPositions()
        {
            var fbControl = this.fbContainer;
            this.social.Controls.Remove(fbControl);
            this.social.Controls.Add(fbControl);
        }

        private void ShowFacebook(SocialMediaPlaceholderValue socialSettings)
        {
            if (socialSettings.FacebookLikeUrl != null)
            {
                this.fb.HRef = HttpUtility.HtmlEncode(socialSettings.FacebookLikeUrl.ToString());
                this.fb.InnerHtml = HttpUtility.HtmlEncode(Iri.ShortenForDisplay(socialSettings.FacebookLikeUrl));
                this.fbContainer.Visible = true;
            }
        }

        private void ShowTwitter(SocialMediaPlaceholderValue socialSettings)
        {
            if (!String.IsNullOrEmpty(socialSettings.TwitterWidget))
            {
                // parse link from Twitter code
                var match = Regex.Match(HttpUtility.HtmlDecode(socialSettings.TwitterWidget), "href=\"(?<Url>.*?)\".*?>(?<LinkText>.*?)</a>");
                if (match.Success)
                {
                    this.tw.HRef = match.Groups["Url"].Value;
                    this.tw.InnerText = match.Groups["LinkText"].Value;
                    this.twContainer.Visible = true;
                }
            }
            else if (!String.IsNullOrEmpty(socialSettings.TwitterSearch))
            {
                this.tw.HRef = String.Format(CultureInfo.CurrentCulture, this.tw.HRef, HttpUtility.UrlEncode(socialSettings.TwitterSearch.ToString()));
                this.tw.InnerText = String.Format(CultureInfo.CurrentCulture, this.tw.InnerText, socialSettings.TwitterSearch.ToString());
                this.twContainer.Visible = true;
            }
        }
    }
}