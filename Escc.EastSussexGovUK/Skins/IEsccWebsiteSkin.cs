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

        /// <summary>
        /// The Typekit fonts which are required by the skin.
        /// </summary>
        /// <returns>A set of Typekit font URLs</returns>
        IEnumerable<TypekitFontDependency> RequiresTypekitFonts();
        
        /// <summary>
        /// Class or classes applied to main content with standard text formatting
        /// </summary>
        string TextContentClass { get; }

        /// <summary>
        /// Class or classes applied to supporting content with standard text formatting
        /// </summary>
        string SupportingTextContentClass { get; }
    }
}
