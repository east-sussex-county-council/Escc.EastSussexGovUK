using System;
using System.Web.Mvc;
using Escc.Web.Metadata;

namespace Escc.EastSussexGovUK.Mvc.Views.EastSussexGovUK
{
    /// <summary>
    /// An MVC-compatible wrapper around the WebForms Escc.Web.Metadata.MetadataControl control
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ViewUserControl{Escc.Web.Metadata.Metadata}" />
    public partial class MetadataControl : ViewUserControl<Metadata>
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.metadata.Metadata = Model;
        }
    }
}