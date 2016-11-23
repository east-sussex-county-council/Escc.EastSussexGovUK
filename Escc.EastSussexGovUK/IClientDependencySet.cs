using System.Collections.Generic;

namespace Escc.EastSussexGovUK
{
    /// <summary>
    /// A set of client dependencies which might be required by a view, and the dependencies it relies on. Dependencies are given as aliases which can be used with
    /// the <see cref="Escc.ClientDependencyFramework"/> and <see cref="ContentSecurityPolicy"/>
    /// </summary>
    public interface IClientDependencySet
    {
        /// <summary>
        /// Determines whether the dependency is required 
        /// </summary>
        /// <returns><c>true</c> if the dependency is required; <c>false</c> otherwise</returns>
        bool IsRequired();
        
        /// <summary>
        /// The CSS which is required for the dependent feature. These are registered in web.config using <see cref="Escc.ClientDependencyFramework"/>.
        /// </summary>
        /// <returns>A set of CSS file aliases, optionally qualified by media query aliases</returns>
        IEnumerable<CssFileDependency> RequiresCss();

        /// <summary>
        /// The JavaScript which is required for the dependent feature. These are registered in web.config using <see cref="Escc.ClientDependencyFramework"/>.
        /// </summary>
        /// <returns>A set of JS file aliases, qualified by a priority value which defaults to 100</returns>
        IEnumerable<JsFileDependency> RequiresJavaScript();

        /// <summary>
        /// The content security policy aliases required for the dependent feature. These are registered in web.config using <see cref="ContentSecurityPolicy"/>.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ContentSecurityPolicyDependency> RequiresContentSecurityPolicy();
    }
}
