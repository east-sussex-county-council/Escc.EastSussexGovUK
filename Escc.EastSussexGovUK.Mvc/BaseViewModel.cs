using System;
using System.Web;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Skins;
using Escc.EastSussexGovUK.Views;
using Escc.Web.Metadata;

namespace Escc.EastSussexGovUK.Mvc
{
    /// <summary>
    /// Base class for view models used on www.eastsussex.gov.uk
    /// </summary>
    public abstract class BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        protected BaseViewModel() : this(new BreadcrumbTrailFromConfig(HttpContext.Current.Request.Url))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        /// <param name="breadcrumbProvider">The breadcrumb provider to replace the default <see cref="BreadcrumbTrailFromConfig"/>.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected BaseViewModel(IBreadcrumbProvider breadcrumbProvider)
        {
            if (breadcrumbProvider == null) throw new ArgumentNullException(nameof(breadcrumbProvider));

            IsPublicView = true;
            EsccWebsiteSkin = new CustomerFocusSkin();
            Metadata = new Metadata();
            BreadcrumbProvider = breadcrumbProvider;
        }

        /// <summary>
        /// Gets or sets the HTML strings that make up the site template
        /// </summary>
        public TemplateHtml TemplateHtml { get; set; } = new TemplateHtml();

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public Metadata Metadata { get; set; }

        /// <summary>
        /// Gets or sets whether the current view is a publicly-visible page
        /// </summary>
        public bool IsPublicView { get; set; }

        /// <summary>
        /// Gets or sets the current master page or layout
        /// </summary>
        public EsccWebsiteView EsccWebsiteView { get; set; }

        /// <summary>
        /// Gets or sets the skin applied to content 
        /// </summary>
        public IEsccWebsiteSkin EsccWebsiteSkin { get; set; }

        /// <summary>
        /// Gets or sets the provider for working out the current context within the site's information architecture.
        /// </summary>
        /// <value>
        /// The breadcrumb provider.
        /// </value>
        public IBreadcrumbProvider BreadcrumbProvider { get; set; }

        /// <summary>
        /// Gets or sets the HTML to display as a 'latest' update, where supported.
        /// </summary>
        /// <value>
        /// The latest.
        /// </value>
        public IHtmlString Latest { get; set; }

        /// <summary>
        /// Gets or sets whether the East Sussex 1Space widget should be shown, where supported.
        /// </summary>
        /// <value>
        /// <c>true</c> to show the widget; otherwise, <c>false</c>.
        /// </value>
        public bool ShowEastSussex1SpaceWidget { get; set; }

        /// <summary>
        /// Gets or sets whether the ESCIS widget should be shown, where supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> to show the widget; otherwise, <c>false</c>.
        /// </value>
        public bool ShowEscisWidget { get; set; }

        /// <summary>
        /// Gets or sets the social media widgets to display, where supported.
        /// </summary>
        public SocialMediaSettings SocialMedia { get; set; }

        /// <summary>
        /// Gets or sets whether web chat should be enabled, where supported.
        /// </summary>
        public WebChatSettings WebChat { get; set; }
    }
}
