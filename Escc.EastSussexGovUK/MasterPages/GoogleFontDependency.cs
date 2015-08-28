using System;

namespace Escc.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// A Google font on which an <see cref="IClientDependencySet"/> depends
    /// </summary>
    public class GoogleFontDependency
    {
        /// <summary>
        /// The URL of a Google font. For example https://fonts.googleapis.com/css?family=Source+Sans+Pro:400,600
        /// </summary>
        public Uri FontUrl { get; set; }
    }
}