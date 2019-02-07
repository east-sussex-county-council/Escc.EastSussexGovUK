using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Views;
using Exceptionless;

namespace Escc.EastSussexGovUK.WebForms
{
    /// <summary>
    /// Loads a section of the master page, either from a local usercontrol or remotely from the public website.
    /// </summary>
    public class MasterPageControl : PlaceHolder
    {
        /// <summary>
        /// Gets or sets the client used to fetch the HTML
        /// </summary>
        public IHtmlControlProvider HtmlControlProvider { get; set; }

        /// <summary>
        /// Gets or sets an identifier for the control to load.
        /// </summary>
        /// <value>The control.</value>
        public string Control { get; set; }

        /// <summary>
        /// Gets or sets the provider for working out the current context within the site's information architecture.
        /// </summary>
        /// <value>
        /// The breadcrumb provider.
        /// </value>
        public IBreadcrumbProvider BreadcrumbProvider { get; set; }
        private NameValueCollection config = null;


        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            try
            {
                // Check that the Control property is set
                if (HtmlControlProvider == null) throw new ArgumentNullException("HtmlControlProvider", "Property 'HtmlControlProvider' must be set for class MasterPageControl");
                if (String.IsNullOrEmpty(this.Control)) throw new ArgumentNullException("Control", "Property 'Control' must be set for class MasterPageControl");

                // Get the configuration settings for remote master pages. Is this control in there?
                this.config = ConfigurationManager.GetSection("Escc.EastSussexGovUK/RemoteMasterPage") as NameValueCollection;
                if (this.config == null) this.config = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/RemoteMasterPage") as NameValueCollection;
                if (this.config != null && !String.IsNullOrEmpty(this.config["MasterPageControlUrl"]))
                {
                    LoadRemoteControl();
                }
                else
                {
                    LoadLocalControl();
                }
            }
            catch (Exception ex)
            {
                // Report the error and continue. Better to return no HTML than to throw an error that causes the page to
                ex.ToExceptionless().Submit();
            }
        }

        /// <summary>
        /// Loads a local usercontrol.
        /// </summary>
        /// <exception cref="System.Web.HttpException">Thrown if usercontrol does not exist</exception>
        private void LoadLocalControl()
        {
            // Default to the path that used to be hard-coded
            var localControlUrl = "~/masterpages/controls/{0}.ascx";

            // Allow override to load controls from anywhere
            this.config = ConfigurationManager.GetSection("Escc.EastSussexGovUK/GeneralSettings") as NameValueCollection;
            if (this.config == null) this.config = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/GeneralSettings") as NameValueCollection;
            if (this.config != null && !String.IsNullOrEmpty(this.config["MasterPageControlUrl"]))
            {
                localControlUrl = this.config["MasterPageControlUrl"];
            }

            this.Controls.Add(Page.LoadControl(String.Format(CultureInfo.InvariantCulture, localControlUrl, this.Control)));
        }

        /// <summary>
        /// Fetches the master page control from a remote URL.
        /// </summary>
        private void LoadRemoteControl()
        {
            if (BreadcrumbProvider == null)
            {
                BreadcrumbProvider = new BreadcrumbTrailFromConfig(HttpContext.Current.Request.Url);
            }
            var textSize = new TextSize(Page.Request.Cookies?["textsize"]?.Value, Page.Request.QueryString).CurrentTextSize();
            var isLibraryCatalogueRequest = new LibraryCatalogueContext(Page.Request.UserAgent).RequestIsFromLibraryCatalogueMachine();

            var html = HtmlControlProvider.FetchHtmlForControl(
                HttpRuntime.AppDomainAppVirtualPath,
                HttpContext.Current.Request.Url,
                Control,
                BreadcrumbProvider,
                textSize,
                isLibraryCatalogueRequest
                ).Result;

            // Output the HTML
            this.Controls.Add(new LiteralControl(html));
        }
    }
}