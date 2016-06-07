using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Base class for standard error messages
    /// </summary>
    /// <seealso cref="System.Web.UI.UserControl" />
    public abstract class ErrorUserControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// Gets or sets the currently-applied skin.
        /// </summary>
        /// <value>
        /// The skin.
        /// </value>
        public IEsccWebsiteSkin Skin { get; set; }
    }
}