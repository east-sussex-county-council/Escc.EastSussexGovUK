﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.Features
{
    /// <summary>
    /// Turn Google Maps links into embedded maps
    /// </summary>
    public class EmbeddedGoogleMaps : IClientDependencySet
    {
        /// <summary>
        /// HTML from fields which might contain an embedded Google Maps link
        /// </summary>
        public IEnumerable<string> Html { get; set; }

        /// <summary>
        /// Determines whether the dependency is required
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the dependency is required; <c>false</c> otherwise
        /// </returns>
        public bool IsRequired()
        {
            if (Html == null) return false;
            return Html.Any(htmlString =>
                !string.IsNullOrEmpty(htmlString) &&
                Regex.IsMatch(htmlString, @"maps\.google\.co\.uk\/maps\/ms\?msid=[0-9a-f.]+&amp;msa=0", RegexOptions.IgnoreCase) ||
                htmlString.Contains(@"/umbraco/api/location/list")
                );
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
            return new JsFileDependency[3]
            {
                new JsFileDependency() { JsFileAlias = "GoogleMaps", Priority = 90 },
                new JsFileDependency() { JsFileAlias = "JQueryRetry", Priority = 90 },
                new JsFileDependency() { JsFileAlias = "EmbedGoogleMaps" }
            };
        }

        /// <summary>
        /// The content security policy aliases required for the dependent feature. These are registered in web.config using <see cref="ContentSecurityPolicy" />.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContentSecurityPolicyDependency> RequiresContentSecurityPolicy()
        {
            return new ContentSecurityPolicyDependency[1]
            {
                new ContentSecurityPolicyDependency(){ Alias = "GoogleMaps" }
            };
        }
    }
}