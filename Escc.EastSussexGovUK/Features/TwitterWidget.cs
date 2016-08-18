using System.Collections.Generic;
using Escc.EastSussexGovUK.Views;

namespace Escc.EastSussexGovUK.Features
{
    /// <summary>
    /// A Twitter search or profile widget
    /// </summary>
    public class TwitterWidget : IClientDependencySet
    {
        /// <summary>
        /// Gets or sets the social media settings.
        /// </summary>
        /// <value>
        /// The social media.
        /// </value>
        public SocialMediaSettings SocialMedia { get; set; }

        /// <summary>
        /// Gets or sets the current layout applied to www.eastsussex.gov.uk
        /// </summary>
        public EsccWebsiteView EsccWebsiteView { get; set; }

        /// <summary>
        /// Determines whether the dependency is required
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the dependency is required; <c>false</c> otherwise
        /// </returns>
        public bool IsRequired()
        {
            if (SocialMedia == null) return false;
            return SocialMedia.TwitterWidgetScript != null && EsccWebsiteView == EsccWebsiteView.Desktop;
        }

        /// <summary>
        /// The CSS which is required for the dependent feature. These are registered in web.config using <see cref="Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of CSS file aliases, optionally qualified by media query aliases
        /// </returns>
        public IEnumerable<CssFileDependency> RequiresCss()
        {
            return new CssFileDependency[1] {new CssFileDependency() {CssFileAlias = "SocialMediaLarge", MediaQueryAlias = "Large"}};
        }

        /// <summary>
        /// The JavaScript which is required for the dependent feature. These are registered in web.config using <see cref="Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of JS file aliases, qualified by a priority value which defaults to 100
        /// </returns>
        public IEnumerable<JsFileDependency> RequiresJavaScript()
        {
            return new JsFileDependency[0];
        }

        /// <summary>
        /// The content security policy aliases required for the dependent feature. These are registered in web.config using <see cref="ContentSecurityPolicy" />.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContentSecurityPolicyDependency> RequiresContentSecurityPolicy()
        {
            return new ContentSecurityPolicyDependency[1] { new ContentSecurityPolicyDependency() { Alias = "Twitter" } };
        }
    }
}