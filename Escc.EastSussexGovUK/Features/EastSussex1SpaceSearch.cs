using System;
using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Features
{
    /// <summary>
    /// A search box for the East Sussex 1Space website
    /// </summary>
    public class EastSussex1SpaceSearch : IClientDependencySet
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show East Sussex 1Space search.
        /// </summary>
        /// <value>
        /// <c>true</c> to show search; otherwise, <c>false</c>.
        /// </value>
        public bool ShowEastSussex1SpaceSearch { get; set; }

        /// <summary>
        /// Determines whether the dependency is required
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the dependency is required; <c>false</c> otherwise
        /// </returns>
        public bool IsRequired()
        {
            return ShowEastSussex1SpaceSearch;
        }

        /// <summary>
        /// The CSS which is required for the dependent feature. These are registered in web.config using <see cref="Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of CSS file aliases, optionally qualified by media query aliases
        /// </returns>
        public IEnumerable<CssFileDependency> RequiresCss()
        {
            return new CssFileDependency[1] {new CssFileDependency() {CssFileAlias = "EastSussex1Space", CssRelativeUrl = new Uri("/css/min/1space.css", UriKind.Relative) } };
        }

        /// <summary>
        /// The JavaScript which is required for the dependent feature. These are registered in web.config using <see cref="Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of JS file aliases, qualified by a priority value which defaults to 100
        /// </returns>
        public IEnumerable<JsFileDependency> RequiresJavaScript()
        {
            return new JsFileDependency[1] { new JsFileDependency() { JsFileAlias = "EastSussex1Space", JsRelativeUrl = new Uri("/js/min/1space.js", UriKind.Relative) } };

        }

        /// <summary>
        /// The content security policy aliases required for the dependent feature. These are registered in web.config using <see cref="ContentSecurityPolicy" />.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContentSecurityPolicyDependency> RequiresContentSecurityPolicy()
        {
            return new ContentSecurityPolicyDependency[1]
            {
                new ContentSecurityPolicyDependency(){ Alias = "EastSussex1Space" }
            };
        }
    }
}