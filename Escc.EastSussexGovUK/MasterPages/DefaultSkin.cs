
using System;
using System.Collections.Generic;
using Escc.EastSussexGovUK.MasterPages;

namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// The default skin on www.eastsussex.gov.uk
    /// </summary>
    public class DefaultSkin : IEsccWebsiteSkin
    {
        /// <summary>
        /// Class or classes applied to supporting content with standard text formatting
        /// </summary>
        public virtual string SupportingTextContentClass { get { return "supporting-text"; } }

        /// <summary>
        /// Determines whether the skin should be applied
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the skin should be applied; <c>false</c> otherwise
        /// </returns>
        public virtual bool IsRequired()
        {
            return true;
        }

        /// <summary>
        /// The content security policy aliases required for the skin. These are registered in web.config using <see cref="EsccWebTeam.Data.Web.ContentSecurityPolicy" />.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<ContentSecurityPolicyDependency> RequiresContentSecurityPolicy()
        {
            return new ContentSecurityPolicyDependency[0];
        }

        /// <summary>
        /// The CSS which is required for the dependent feature. These are registered in web.config using <see cref="Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of CSS file aliases, optionally qualified by media query aliases
        /// </returns>
        public virtual IEnumerable<CssFileDependency> RequiresCss()
        {
            return new CssFileDependency[0];
        }

        /// <summary>
        /// The Google fonts which are required by the skin.
        /// </summary>
        /// <returns>
        /// A set of Google Font URLs
        /// </returns>
        public virtual IEnumerable<GoogleFontDependency> RequiresGoogleFonts()
        {
            return new GoogleFontDependency[0];
        }

        /// <summary>
        /// The Typekit fonts which are required by the skin.
        /// </summary>
        /// <returns>
        /// A set of Typekit font URLs
        /// </returns>
        public virtual IEnumerable<TypekitFontDependency> RequiresTypekitFonts()
        {
            return new TypekitFontDependency[0];
        }

        /// <summary>
        /// The JavaScript which is required for the dependent feature. These are registered in web.config using <see cref="Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of JS file aliases, qualified by a priority value which defaults to 100
        /// </returns>
        public virtual IEnumerable<JsFileDependency> RequiresJavaScript()
        {
            return new JsFileDependency[0];
        }
    }
}