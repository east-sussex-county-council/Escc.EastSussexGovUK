using System;
using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Features
{
    /// <summary>
    /// Web chat facility which appears only on selected pages
    /// </summary>
    public class WebChat : IClientDependencySet
    {
        /// <summary>
        /// Gets or sets the web chat settings, with details of where web chat should appear.
        /// </summary>
        /// <value>
        /// The web chat settings.
        /// </value>
        public WebChatSettings WebChatSettings { get; set; }

        /// <summary>
        /// Determines whether the dependency is required
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the dependency is required; <c>false</c> otherwise
        /// </returns>
        public bool IsRequired()
        {
            return UrlStartsWithOneOfThese(WebChatSettings.PageUrl, WebChatSettings.WebChatUrls) && !UrlStartsWithOneOfThese(WebChatSettings.PageUrl, WebChatSettings.ExcludedUrls);
        }

        /// <summary>
        /// Checks if <c>url</c> starts with one of the <c>urlsToMatch</c>. Web chat usually appears in whole sections rather than individual pages, 
        /// so use <c>StartsWith</c> rather than looking for an exact match.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="urlsToMatch">The urls to match.</param>
        /// <returns></returns>
        private static bool UrlStartsWithOneOfThese(Uri url, IList<Uri> urlsToMatch)
        {
            var len = urlsToMatch.Count;
            var urlToTest = url.ToString();
            for (var i = 0; i < len; i++)
            {
                if (urlToTest.StartsWith(urlsToMatch[i].ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
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
            return new JsFileDependency[1] {new JsFileDependency() {JsFileAlias = "WebChat", JsRelativeUrl = new Uri("/js/min/webchat.js", UriKind.Relative) }};
        }

        /// <summary>
        /// The content security policy aliases required for the dependent feature. These are registered in web.config using <see cref="ContentSecurityPolicy" />.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContentSecurityPolicyDependency> RequiresContentSecurityPolicy()
        {
            return new ContentSecurityPolicyDependency[1] {new ContentSecurityPolicyDependency() {Alias = "WebChat"}};
        }
    }
}