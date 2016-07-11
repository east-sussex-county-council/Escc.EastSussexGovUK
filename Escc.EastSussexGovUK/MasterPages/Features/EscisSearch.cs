using System.Collections.Generic;
using Escc.Web;

namespace Escc.EastSussexGovUK.MasterPages.Features
{
    /// <summary>
    /// A search box for the ESCIS website
    /// </summary>
    public class EscisSearch : IClientDependencySet
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show ESCIS search.
        /// </summary>
        /// <value>
        /// <c>true</c> to show search; otherwise, <c>false</c>.
        /// </value>
        public bool ShowEscisSearch { get; set; }

        /// <summary>
        /// Determines whether the dependency is required
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the dependency is required; <c>false</c> otherwise
        /// </returns>
        public bool IsRequired()
        {
            return ShowEscisSearch;
        }

        /// <summary>
        /// The CSS which is required for the dependent feature. These are registered in web.config using <see cref="Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of CSS file aliases, optionally qualified by media query aliases
        /// </returns>
        public IEnumerable<CssFileDependency> RequiresCss()
        {
            return new CssFileDependency[0];
        }

        /// <summary>
        /// The JavaScript which is required for the dependent feature. These are registered in web.config using <see cref="Escc.ClientDependencyFramework" />.
        /// </summary>
        /// <returns>
        /// A set of JS file aliases, qualified by a priority value which defaults to 100
        /// </returns>
        public IEnumerable<JsFileDependency> RequiresJavaScript()
        {
            return new JsFileDependency[1] { new JsFileDependency() { JsFileAlias = "Escis" } };

        }

        /// <summary>
        /// The content security policy aliases required for the dependent feature. These are registered in web.config using <see cref="ContentSecurityPolicy" />.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContentSecurityPolicyDependency> RequiresContentSecurityPolicy()
        {
            return new ContentSecurityPolicyDependency[1]
            {
                new ContentSecurityPolicyDependency(){ Alias = "Escis" }
            };
        }
    }
}