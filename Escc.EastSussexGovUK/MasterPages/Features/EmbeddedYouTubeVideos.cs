using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Escc.EastSussexGovUK.MasterPages.Features
{
    /// <summary>
    /// Turn YouTube video links into embedded videos
    /// </summary>
    public class EmbeddedYouTubeVideos : IClientDependencySet
    {
        /// <summary>
        /// HTML from fields which might contain an embedded YouTube video
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
            return Html.Any(htmlString => Regex.IsMatch(htmlString, @"https?:\/\/(youtu.be\/|www.youtube.com\/watch\?v=)([A-Za-z0-9_-]+)", RegexOptions.IgnoreCase));
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
            return new JsFileDependency[1]
            {
                new JsFileDependency() { JsFileAlias = "EmbedYouTube" }
            };
        }

        /// <summary>
        /// The content security policy aliases required for the dependent feature. These are registered in web.config using <see cref="EsccWebTeam.Data.Web.ContentSecurityPolicy" />.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContentSecurityPolicyDependency> RequiresContentSecurityPolicy()
        {
            return new ContentSecurityPolicyDependency[1]
            {
                new ContentSecurityPolicyDependency(){ Alias = "YouTube" }
            };
        }
    }
}