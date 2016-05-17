using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EsccWebTeam.Data.Web;
using EsccWebTeam.EastSussexGovUK.MasterPages;

namespace Escc.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// A custom skin for marriage pages on www.eastsussex.gov.uk
    /// </summary>
    public class PreviousMarriageSkin : DefaultSkin
    {
        private readonly EsccWebsiteView _currentView;
        private Uri _requestUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarriageSkin" /> class.
        /// </summary>
        /// <param name="currentView">The current view.</param>
        /// <param name="requestUrl">The request URL.</param>
        public PreviousMarriageSkin(EsccWebsiteView currentView, Uri requestUrl)
        {
            _currentView = currentView;
            _requestUrl = requestUrl;
        }

        /// <summary>
        /// Determines whether the skin should be applied, based on the whether the URL starts with /community/registration/registeramarriage
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the skin should be applied; <c>false</c> otherwise
        /// </returns>
        public override bool IsRequired()
        {
            if (_requestUrl != null)
            {
                _requestUrl = Iri.MakeAbsolute(_requestUrl);
                return _requestUrl.AbsolutePath.StartsWith("/community/registration/registeramarriage", StringComparison.OrdinalIgnoreCase);
            }
            else return true;
        }

        /// <summary>
        /// The CSS which is required for the skin. These are registered in web.config using <see cref="Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of CSS file aliases, optionally qualified by media query aliases
        /// </returns>
        public override IEnumerable<CssFileDependency> RequiresCss()
        {
            var css = new List<CssFileDependency>(base.RequiresCss())
            {
                new CssFileDependency() {CssFileAlias = "RegistrationSmall"}
            };
            if (_currentView == EsccWebsiteView.Desktop)
            {
                css.Add(new CssFileDependency() {CssFileAlias = "RegistrationMedium", MediaQueryAlias = "Medium"});
                css.Add(new CssFileDependency() {CssFileAlias = "RegistrationLarge", MediaQueryAlias = "Large"});
            }
            return css;
        }

        /// <summary>
        /// The JavaScript which is required for the skin. These are registered in web.config using <see cref="Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of JS file aliases, qualified by a priority value which defaults to 100
        /// </returns>
        public override IEnumerable<JsFileDependency> RequiresJavaScript()
        {
            return new List<JsFileDependency>(base.RequiresJavaScript())
            {
                new JsFileDependency() {JsFileAlias = "MarriageSkin"}
            };
        }

        /// <summary>
        /// The Typekit fonts which are required by the skin.
        /// </summary>
        /// <returns>
        /// A set of Typekit font URLs
        /// </returns>
        public override IEnumerable<TypekitFontDependency> RequiresTypekitFonts()
        {
            return new List<TypekitFontDependency>(base.RequiresTypekitFonts())
            {
                new TypekitFontDependency() { TypekitUrl = new Uri("https://use.typekit.net/djq4xlq.js")}
            };
        }

        /// <summary>
        /// Require the content security policy for typekit
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ContentSecurityPolicyDependency> RequiresContentSecurityPolicy()
        {
            return new List<ContentSecurityPolicyDependency>(base.RequiresContentSecurityPolicy())
            {
                new ContentSecurityPolicyDependency() {Alias = "Typekit"}
            };
        }
    }
}