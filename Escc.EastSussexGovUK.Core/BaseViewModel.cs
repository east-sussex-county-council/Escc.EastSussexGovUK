using System;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Skins;
using Escc.EastSussexGovUK.Views;
using Microsoft.AspNetCore.Html;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// Base class for view models used with the EastSussexGovUK template
    /// </summary>
    public abstract class BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        /// <param name="breadcrumbProvider">The breadcrumb provider which provides essential context for the views.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected BaseViewModel(IViewModelDefaultValuesProvider defaultValues)
        {
            if (defaultValues == null)
            {
                throw new ArgumentNullException(nameof(defaultValues));
            }

            BreadcrumbProvider = defaultValues.Breadcrumb ?? throw new ArgumentException($"{nameof(defaultValues)}.Breadcrumb cannot be null", nameof(defaultValues));
            Metadata = defaultValues.Metadata;
            ClientFileBaseUrl = defaultValues.ClientFileBaseUrl?.ToString().TrimEnd('/');
            ClientFileVersion = defaultValues.ClientFileVersion;
        }

        /// <summary>
        /// Gets or sets the HTML strings that make up the site template
        /// </summary>
        public TemplateHtml TemplateHtml { get; set; } = new TemplateHtml();

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        public Metadata.Metadata Metadata { get; set; } = new Metadata.Metadata();

        /// <summary>
        /// Gets or sets the current master page or layout
        /// </summary>
        public EsccWebsiteView EsccWebsiteView { get; set; }

        /// <summary>
        /// Gets or sets the skin applied to content 
        /// </summary>
        public IEsccWebsiteSkin EsccWebsiteSkin { get; set; } = new CustomerFocusSkin();

        /// <summary>
        /// Gets or sets the base URL to use for sitewide client-side files such as CSS and JavaScript that are not part of the current application
        /// </summary>
        /// <remarks>This uses <see cref="string"> rather than <see cref="Uri"/> because Uri.ToString() appends a trailing / on  which in this case is never wanted</remarks>
        public string ClientFileBaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the version string to append to client-side files such as CSS and JavaScript to ensure that previously cached versions are not returned
        /// </summary>
        public string ClientFileVersion { get; set; }

        private const string MEDIA_QUERY_MEDIUM = "only screen and (min-width: 474px)";
        private const string MEDIA_QUERY_LARGE = "only screen and (min-width: 802px)";

        /// <summary>
        /// This media query should be applied to all &lt;link /&gt; elements intended to load CSS for medium screens and above
        /// </summary>
        public string MediaQueryMedium { get { return MEDIA_QUERY_MEDIUM; } }

        /// <summary>
        /// This media query should be applied to all &lt;link /&gt; elements intended to load CSS for large screens and above
        /// </summary>
        public string MediaQueryLarge { get { return MEDIA_QUERY_LARGE; } }

        /// <summary>
        /// Gets or sets the provider for working out the current context within the site's information architecture.
        /// </summary>
        public IBreadcrumbProvider BreadcrumbProvider { get; set; }

        /// <summary>
        /// Gets or sets the HTML to display as a 'latest' update, where supported.
        /// </summary>
        public IHtmlContent Latest { get; set; }

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
