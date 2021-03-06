﻿using System;
using System.Linq;
using System.Text;
using System.Web;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Views;
using Escc.Net;
using Escc.Net.Configuration;
using Escc.Web;
using Microsoft.Extensions.Options;

namespace Escc.EastSussexGovUK.WebForms
{
    /// <summary>
    /// Master page for content which needs the whole screen, such as maps
    /// </summary>
    public partial class FullScreen : BaseMasterPage
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected new void Page_Load(object sender, EventArgs e)
        {
            // Configure remote master page
            var httpClientProvider = new HttpClientProvider(new ConfigurationProxyProvider());
            var masterPageSettings = new RemoteMasterPageSettingsFromConfig();
            var forceCacheRefresh = (Page.Request.QueryString["ForceCacheRefresh"] == "1"); // Provide a way to force an immediate update of the cache
            var remoteMasterPageClient = new RemoteMasterPageHtmlProvider(masterPageSettings.MasterPageControlUrl(), httpClientProvider, Request.UserAgent, new RemoteMasterPageMemoryCacheProvider { CacheDuration = TimeSpan.FromMinutes(masterPageSettings.CacheTimeout()) }, forceCacheRefresh);
            this.htmlTag.HtmlControlProvider = remoteMasterPageClient;
            this.metadataFullScreen.HtmlControlProvider = remoteMasterPageClient;
            if (this.headerFullScreen != null) this.headerFullScreen.HtmlControlProvider = remoteMasterPageClient;
            this.scriptsFullScreen.HtmlControlProvider = remoteMasterPageClient;

            // Support web fonts required by the current skin
            if (Skin != null)
            {
                var fontsHtml = new StringBuilder();
                foreach (var font in Skin.RequiresGoogleFonts())
                {
                    fontsHtml.Append("<link href=\"").Append(font.FontUrl).Append("\" rel=\"stylesheet\" type=\"text/css\" />");
                }

                this.fonts.Text = fontsHtml.ToString();

                AddClientDependencies(Skin);
            }

            // Support web chat
            var context = new HostingEnvironmentContext(HttpContext.Current.Request.Url);
            if (context.WebChatSettingsUrl != null)
            {
                var webChat = new WebChat();
                var webChatApiSettings = Options.Create(new WebChatApiSettings { WebChatSettingsUrl = context.WebChatSettingsUrl, CacheMinutes = context.WebChatSettingsCacheDuration });
                webChat.WebChatSettings = new WebChatSettingsFromApi(webChatApiSettings, httpClientProvider, new ApplicationCacheStrategy<WebChatSettings>()).ReadWebChatSettings().Result;
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

            var cspConfig = new ContentSecurityPolicyFromConfig();
            var filter = new ContentSecurityPolicyUrlFilter(Request.Url, cspConfig.UrlsToExclude);
            if (filter.ApplyPolicy() && !Response.HeadersWritten)
            {
                var csp = new ContentSecurityPolicyHeaders(Response.Headers);
                foreach (var contentSecurityPolicy in dependencySet.RequiresContentSecurityPolicy())
                {
                    csp.AppendPolicy(cspConfig.Policies[contentSecurityPolicy.Alias]);
                }
                csp.UpdateHeaders();
            }
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