using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Skins
{
    /// <summary>
    /// A skin is applied to content between the header and footer of www.eastsussex.gov.uk
    /// </summary>
    public interface IEsccWebsiteSkin : IClientDependencySet
    {
        /// <summary>
        /// The Google fonts which are required by the skin.
        /// </summary>
        /// <returns>A set of Google Font URLs</returns>
        IEnumerable<GoogleFontDependency> RequiresGoogleFonts();
    }
}
