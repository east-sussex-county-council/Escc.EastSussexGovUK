using System;

namespace Escc.EastSussexGovUK
{
    /// <summary>
    /// A Typekit on which an <see cref="IClientDependencySet"/> depends
    /// </summary>
    public class TypekitFontDependency
    {
        /// <summary>
        /// The URL of a Typekit font group. For example //use.typekit.net/xxxxxx.js
        /// </summary>
        public Uri TypekitUrl { get; set; }
    }
}