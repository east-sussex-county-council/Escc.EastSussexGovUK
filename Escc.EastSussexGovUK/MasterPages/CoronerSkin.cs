using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EsccWebTeam.Data.Web;
using EsccWebTeam.EastSussexGovUK.MasterPages;

namespace Escc.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// A custom skin for coroner pages on www.eastsussex.gov.uk
    /// </summary>
    public class CoronerSkin : CustomerFocusSkin
    {
        private readonly EsccWebsiteView _currentView;
        private Uri _requestUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoronerSkin" /> class.
        /// </summary>
        /// <param name="currentView">The current view.</param>
        /// <param name="requestUrl">The request URL.</param>
        public CoronerSkin(EsccWebsiteView currentView, Uri requestUrl) : base(currentView)
        {
            _currentView = currentView;
            _requestUrl = requestUrl;
        }

        /// <summary>
        /// Determines whether the skin should be applied, based on the whether the URL contains /coroner
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the skin should be applied; <c>false</c> otherwise
        /// </returns>
        public override bool IsRequired()
        {
            if (_requestUrl != null)
            {
                _requestUrl = Iri.MakeAbsolute(_requestUrl);
                return _requestUrl.AbsolutePath.ToLowerInvariant().Contains("/coroner");
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
                new CssFileDependency() {CssFileAlias = "CoronerSkinSmall"},
                new CssFileDependency() {CssFileAlias = "CoronerSkinMedium", MediaQueryAlias = "Medium"}
            };
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
                new JsFileDependency() {JsFileAlias = "CoronerSkin"}
            };
        }
    }
}