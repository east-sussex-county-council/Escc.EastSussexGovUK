using System;
using System.Collections.Generic;
using Escc.EastSussexGovUK.MasterPages;

namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// Skin applied to pages on www.eastsussex.gov.uk based on the work of the Government Digital Service
    /// </summary>
    public class CustomerFocusSkin : IEsccWebsiteSkin
    {
        /// <summary>
        /// Class or classes applied to supporting content with standard text formatting
        /// </summary>
        public string SupportingTextContentClass
        {
            get { return "text-content content-small content-medium"; }
        }

        /// <summary>
        /// Determines whether the skin should be applied
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the skin should be applied; <c>false</c> otherwise
        /// </returns>
        public bool IsRequired()
        {
            return true;
        }

        /// <summary>
        /// The content security policy aliases required for the skin. These are registered in web.config using <see cref="EsccWebTeam.Data.Web.ContentSecurityPolicy" />.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContentSecurityPolicyDependency> RequiresContentSecurityPolicy()
        {
            return new ContentSecurityPolicyDependency[0];
        }

        /// <summary>
        /// The CSS which is required for the skin. These are registered in web.config using <see cref="Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of CSS file aliases, optionally qualified by media query aliases
        /// </returns>
        public IEnumerable<CssFileDependency> RequiresCss()
        {
            return new CssFileDependency[] {
                new CssFileDependency() { CssFileAlias = "ContentSmall", Priority = 10 },
                new CssFileDependency() { CssFileAlias = "ContentMedium", MediaQueryAlias = "Medium", Priority = 10 },
                new CssFileDependency() { CssFileAlias = "ContentLarge", MediaQueryAlias = "Large", Priority = 10 }
            };
        }

        /// <summary>
        /// The Google fonts which are required by the skin.
        /// </summary>
        /// <returns>
        /// A set of Google Font URLs
        /// </returns>
        public IEnumerable<GoogleFontDependency> RequiresGoogleFonts()
        {
            return new GoogleFontDependency[]
            {
                new GoogleFontDependency() { FontUrl = new Uri("https://fonts.googleapis.com/css?family=Source+Sans+Pro:400,600") }
            };
        }

        /// <summary>
        /// The Typekit fonts which are required by the skin.
        /// </summary>
        /// <returns>
        /// A set of Typekit font URLs
        /// </returns>
        public IEnumerable<TypekitFontDependency> RequiresTypekitFonts()
        {
            return new TypekitFontDependency[0];
        }

        /// <summary>
        /// The JavaScript which is required for the skin. These are registered in web.config using <see cref="Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of JS file aliases, qualified by a priority value which defaults to 100
        /// </returns>
        public IEnumerable<JsFileDependency> RequiresJavaScript()
        {
            return new JsFileDependency[0];
        }
    }
}