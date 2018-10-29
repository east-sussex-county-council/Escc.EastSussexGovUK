using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using Escc.EastSussexGovUK.Features;
using Escc.Net;
using Escc.Net.Configuration;
using Escc.Web;

namespace Escc.EastSussexGovUK.WebForms
{
    /// <summary>
    /// Master page for desktop browsers
    /// </summary>
    public partial class Desktop : BaseMasterPage
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected new void Page_Load(object sender, EventArgs e)
        {
            // Apply selected text size to page
            var textSize = new TextSize(Request.Cookies, Request.QueryString);
            int baseTextSize = textSize.CurrentTextSize();
            if (baseTextSize > 1)
            {
                // Add a space if there are other classes, then add to body tag
                this.bodyclass.Controls.Add(new LiteralControl(" size" + baseTextSize.ToString(CultureInfo.InvariantCulture)));

            }

            // Support web fonts required by the current skin
            if (Skin != null)
            {
                var fontsHtml = new StringBuilder();
                foreach (var font in Skin.RequiresGoogleFonts())
                {
                    fontsHtml.Append("<link href=\"").Append(font.FontUrl).Append("\" rel=\"stylesheet\" type=\"text/css\" />");
                }

                if (Skin.RequiresTypekitFonts().Any())
                {
                    foreach (var font in Skin.RequiresTypekitFonts())
                    {
                        fontsHtml.Append("<script src=\"").Append(font.TypekitUrl).Append("\"></script>");
                    }
                    this.Typekit.Visible = true;
                }
                this.fonts.Text = fontsHtml.ToString();

                AddClientDependencies(Skin);
            }

            // Support web chat
            var context = new HostingEnvironmentContext();
            if (context.WebChatSettingsUrl != null)
            {
                var webChat = new WebChat();
                webChat.WebChatSettings = new WebChatSettingsFromApi(context.WebChatSettingsUrl, new ConfigurationProxyProvider(), new ApplicationCacheStrategy<WebChatSettings>(TimeSpan.FromMinutes(context.WebChatSettingsCacheDuration))).ReadWebChatSettings();
                webChat.WebChatSettings.PageUrl = new Uri(Request.Url.AbsolutePath, UriKind.Relative);
                if (webChat.IsRequired())
                {
                    AddClientDependencies(webChat);
                }
            }

            // Run the base method as well
            base.Page_Load(sender, e);
        }

        private void AddClientDependencies(IClientDependencySet dependencySet)
        {
            foreach (var cssFileDependency in dependencySet.RequiresCss())
            {
                if (String.IsNullOrEmpty(cssFileDependency.MediaQueryAlias)) skinSmall.FileList.Add(cssFileDependency.CssFileAlias);
                if (cssFileDependency.MediaQueryAlias == skinMedium.MediaConfiguration) skinMedium.FileList.Add(cssFileDependency.CssFileAlias);
                if (cssFileDependency.MediaQueryAlias == skinLarge.MediaConfiguration) skinLarge.FileList.Add(cssFileDependency.CssFileAlias);
            }

            foreach (var scriptDependency in dependencySet.RequiresJavaScript())
            {
                skinScript.FileList.Add(scriptDependency.JsFileAlias);
            }

            var csp = new ContentSecurityPolicyHeaders(Response.Headers);
            var cspConfig = new ContentSecurityPolicyFromConfig();
            foreach (var contentSecurityPolicy in dependencySet.RequiresContentSecurityPolicy())
            {
                csp.AppendPolicy(cspConfig.Policies[contentSecurityPolicy.Alias]);
            }
            csp.UpdateHeaders();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // There are two opening body tags, one with a class attribute and one without. Only show the appropriate one.
            // Do it late so that code has a chance to add controls.
            this.classyBody.Visible = (this.bodyclass.Controls.Count > 0);
            this.body.Visible = !this.classyBody.Visible;
        }
    }
}