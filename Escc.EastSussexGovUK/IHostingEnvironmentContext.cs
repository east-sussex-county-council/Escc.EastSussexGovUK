using System;

namespace Escc.EastSussexGovUK
{
    /// <summary>
    /// In development and production environments the site may span several domains. Get information about the current environment.
    /// </summary>
    public interface IHostingEnvironmentContext
    {
        /// <summary>
        /// Gets the base URL, if any, to prefix links to the website with. Intended for sub-domains to use when linking back to the main site.
        /// </summary>
        /// <value>The base URL.</value>
        Uri BaseUrl { get; }

        /// <summary>
        /// Gets whether the current request is for a publicly available URL.
        /// </summary>
        /// <value><c>true</c> if URL is public; otherwise, <c>false</c>.</value>
        bool IsPublicUrl { get; }
    }
}