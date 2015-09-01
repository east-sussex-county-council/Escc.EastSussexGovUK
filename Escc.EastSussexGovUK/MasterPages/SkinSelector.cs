using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EsccWebTeam.EastSussexGovUK.MasterPages;

namespace Escc.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// A helper to select which of several possible skins to apply
    /// </summary>
    public static class SkinSelector
    {
        /// <summary>
        /// Selects the first of the possible skins that applies, or the default skin if none apply.
        /// </summary>
        /// <param name="possibleSkins">The possible skins.</param>
        /// <param name="defaultSkin">The default skin.</param>
        /// <returns></returns>
        public static IEsccWebsiteSkin SelectSkin(IEnumerable<IEsccWebsiteSkin> possibleSkins, IEsccWebsiteSkin defaultSkin)
        {
            foreach (var possibleSkin in possibleSkins)
            {
                if (possibleSkin.IsRequired())
                {
                    return possibleSkin;
                }
            }
            return defaultSkin;
        }
    }
}