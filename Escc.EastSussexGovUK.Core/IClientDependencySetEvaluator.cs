using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// Assess whether instances of <see cref="IClientDependencySet"/> have requirements for the current request, and gather those requirements
    /// </summary>
    public interface IClientDependencySetEvaluator
    {
        /// <summary>
        /// The content security policy aliases required for the dependent feature.
        /// </summary>
        /// <returns></returns>
        IList<ContentSecurityPolicyDependency> RequiredContentSecurityPolicy { get; }
 
        /// <summary>
        /// The CSS which is required for the dependent feature.
        /// </summary>
        /// <returns>A set of CSS file aliases, optionally qualified by media query aliases</returns>
        IList<CssFileDependency> RequiredCss { get; }

        /// <summary>
        /// The JavaScript which is required for the dependent feature.
        /// </summary>
        /// <returns>A set of JS file aliases, qualified by a priority value which defaults to 100</returns>
        IList<JsFileDependency> RequiredJavaScript { get; }

        /// <summary>
        /// Assess whether an instance of <see cref="IClientDependencySet"/> has requirements for the current request, and adds those requirements to the combined set
        /// </summary>
        /// <param name="dependencySet"></param>
        void EvaluateDependencySet(IClientDependencySet dependencySet);
    }
}