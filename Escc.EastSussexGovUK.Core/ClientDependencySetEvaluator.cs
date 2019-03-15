using System;
using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// Assess whether instances of <see cref="IClientDependencySet"/> have requirements for the current request, and gather those requirements
    /// </summary>
    public class ClientDependencySetEvaluator : IClientDependencySetEvaluator
    {
        private readonly List<CssFileDependency> _requiredCss = new List<CssFileDependency>();
        private readonly List<JsFileDependency> _requiredJavaScript = new List<JsFileDependency>();
        private readonly List<ContentSecurityPolicyDependency> _requiredContentSecurityPolicy = new List<ContentSecurityPolicyDependency>();

        /// <summary>
        /// Assess whether an instance of <see cref="IClientDependencySet"/> has requirements for the current request, and adds those requirements to the combined set
        /// </summary>
        /// <param name="dependencySet"></param>
        public void EvaluateDependencySet(IClientDependencySet dependencySet)
        {
            if (dependencySet == null)
            {
                throw new ArgumentNullException(nameof(dependencySet));
            }

            if (dependencySet.IsRequired())
            {
                _requiredCss.AddRange(dependencySet.RequiresCss());
                _requiredJavaScript.AddRange(dependencySet.RequiresJavaScript());
                _requiredContentSecurityPolicy.AddRange(dependencySet.RequiresContentSecurityPolicy());                 
            }
        }

        /// <summary>
        /// The CSS which is required for the dependent feature.
        /// </summary>
        /// <returns>A set of CSS file aliases, optionally qualified by media query aliases</returns>
        public IList<CssFileDependency> RequiredCss => _requiredCss;

        /// <summary>
        /// The JavaScript which is required for the dependent feature.
        /// </summary>
        /// <returns>A set of JS file aliases, qualified by a priority value which defaults to 100</returns>
        public IList<JsFileDependency> RequiredJavaScript => _requiredJavaScript;

        /// <summary>
        /// The content security policy aliases required for the dependent feature.
        /// </summary>
        /// <returns></returns>
        public IList<ContentSecurityPolicyDependency> RequiredContentSecurityPolicy => _requiredContentSecurityPolicy;
    }
}
