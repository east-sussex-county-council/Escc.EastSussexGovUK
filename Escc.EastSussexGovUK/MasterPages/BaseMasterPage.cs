using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using EsccWebTeam.Cms;
using Escc.Web.Metadata;
using Microsoft.ContentManagement.Publishing;

namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// Base class for website master pages
    /// </summary>
    /// <remarks>
    /// <para>As well as a separate master page you can also make text replacements as the page loads (primarily intended for ensuring
    /// the correct subdomain is used for internal links).</para>
    /// <example>
    /// &lt;configuration&gt;
    ///   &lt;configSections&gt;
    ///     &lt;sectionGroup name=&quot;EsccWebTeam.EastSussexGovUK&quot;&gt;
    ///       &lt;section name=&quot;HttpReplaceOnRender&quot; type=&quot;System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089&quot; /&gt;
    ///       &lt;section name=&quot;HttpsReplaceOnRender&quot; type=&quot;System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089&quot; /&gt;
    ///     &lt;/sectionGroup&gt;
    ///   &lt;/configSections&gt;
    ///   &lt;EsccWebTeam.EastSussexGovUK&gt;
    ///     &lt;HttpReplaceOnRender&gt;
    ///       &lt;add key=&quot;example-string-replace&quot; value=&quot;example-replaced&quot; /&gt;
    ///       &lt;add key=&quot;REGEX:example-regular-expression-replace&quot; value=&quot;example-replaced&quot; /&gt;
    ///     &lt;/HttpReplaceOnRender&gt;
    ///     &lt;HttpsReplaceOnRender&gt;
    ///       &lt;add key=&quot;example-string-replace&quot; value=&quot;example-replaced&quot; /&gt;
    ///       &lt;add key=&quot;REGEX:example-regular-expression-replace&quot; value=&quot;example-replaced&quot; /&gt;
    ///     &lt;/HttpsReplaceOnRender&gt;
    ///   &lt;/EsccWebTeam.EastSussexGovUK&gt;
    /// &lt;/configuration&gt;
    /// </example>
    /// </remarks>
    public class BaseMasterPage : System.Web.UI.MasterPage, IHasMetadata
    {
        private EastSussexGovUKContext siteContext = new EastSussexGovUKContext();

        /// <summary>
        /// Gets or sets whether to override the default rendered HTML using settings in web.config and <see cref="RewriteRenderedHtml"/>
        /// </summary>
        /// <remarks>Property introduced because overriding Render changes the action of the server form in BlogEngine.net, which breaks the comment feature.</remarks>
        protected bool OverrideRenderMethod { get; set; }

        /// <summary>
        /// Handles the Init event of the Page control.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Default behaviour
            this.OverrideRenderMethod = true;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Common headers for all master pages: Force IE to use latest rendering engine or Chrome Frame
            Response.AddHeader("X-UA-Compatible", "IE=edge,chrome=1");

            // It Do Not Track header is specified, spec says the response should echo it back to show compliance
            if (siteContext.DoNotTrack == true) Response.AddHeader("DNT", "1");
            else if (siteContext.DoNotTrack == false) Response.AddHeader("DNT", "0");

            // Google recommends specifying this to ensure proxy caches don't serve our mobile template to desktop users
            // ...but it doesn't work as ASP.NET ignores it. Also gzip uses Vary: Accept-encoding.
            // https://developers.google.com/webmasters/smartphone-sites/details
            // Response.AddHeader("Vary", "User-Agent");

            if (CmsUtilities.IsCmsEnabled())
            {
                AddCmsClasses();
            }
        }

        /// <summary>
        /// Causes a browser to delete a cookie by setting it to expire yesterday
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="domain">The domain.</param>
        private void DeleteCookie(string name, string domain)
        {
            if (Request.Cookies[name] != null)
            {
                HttpCookie cookie = new HttpCookie(name);
                cookie.Expires = DateTime.Now.AddDays(-1d);
                if (!String.IsNullOrEmpty(domain)) cookie.Domain = domain;
                Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// Adds classes representing CMS edit modes an enclosing tag, similar to the way Modernizr does for browser features.
        /// </summary>
        /// <remarks>Not using HTML tag because Modernizr overwrites the classes you put there</remarks>
        private void AddCmsClasses()
        {
            var place = this.FindControl("bodyclass");
            if (place != null)
            {
                if (CmsUtilities.IsEditing) place.Controls.Add(new LiteralControl(" cms-edit"));
                else if (CmsUtilities.IsPreviewing) place.Controls.Add(new LiteralControl(" cms-preview"));
                else if (CmsHttpContext.Current.Mode == PublishingMode.Unpublished) place.Controls.Add(new LiteralControl(" cms-unpublished"));
            }
        }

        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the server control content.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            // Rendering to pageHtml and rewriting changes the action of the server form in BlogEngine.net, which breaks the comment feature,
            // so this property is added to provide a way to disable that behaviour.
            if (!this.OverrideRenderMethod)
            {
                base.Render(writer);
                return;
            }

            // Make any runtime replacements which customise the view of the site

            // Get the HTML for the page
            var tempWriter = new StringWriter();
            base.Render(new HtmlTextWriter(tempWriter));
            string pageHtml = tempWriter.ToString();

            var replaceSettings = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/" + Request.Url.Scheme.Substring(0, 1).ToUpperInvariant() + Request.Url.Scheme.Substring(1) + "ReplaceOnRender") as NameValueCollection;
            if (replaceSettings != null)
            {
                // Update the HTML based on config settings
                foreach (string pattern in replaceSettings)
                {
                    if (pattern.StartsWith("REGEX:", StringComparison.OrdinalIgnoreCase))
                    {
                        pageHtml = Regex.Replace(pageHtml, pattern.Substring(6), replaceSettings[pattern]);
                    }
                    else
                    {
                        pageHtml = pageHtml.Replace(pattern, replaceSettings[pattern]);
                    }
                }
            }

            pageHtml = RewriteRenderedHtml(pageHtml);

            // Output the modified HTML
            writer.Write(pageHtml);
        }

        /// <summary>
        /// When overridden by a child master page, provides a hook to modify the rendered HTML of the page
        /// </summary>
        /// <param name="pageHtml">Rendered HTML already modified by global changes in web.config</param>
        /// <returns>Modified HTML</returns>
        protected virtual string RewriteRenderedHtml(string pageHtml)
        {
            return pageHtml;
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        public IMetadata Metadata
        {
            get
            {
                IMetadata metadataControl = null;

                metadataControl = FindMetadataControl(this.FindControl("metadata"));

                return metadataControl;
            }
        }

        private IMetadata FindMetadataControl(Control control)
        {
            if (control == null) return null;

            var thisControl = control as IMetadata;
            if (thisControl != null)
            {
                return thisControl;
            }
            else
            {
                foreach (Control child in control.Controls)
                {
                    var descendantControl = FindMetadataControl(child);
                    if (descendantControl != null)
                    {
                        return descendantControl;
                    }
                }
            }
            return null;
        }
    }
}