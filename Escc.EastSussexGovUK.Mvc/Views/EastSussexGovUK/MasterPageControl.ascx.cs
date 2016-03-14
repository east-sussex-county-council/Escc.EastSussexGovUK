using System;
using System.Web.Mvc;

namespace Escc.EastSussexGovUK.Mvc.Views.EastSussexGovUK
{
    /// <summary>
    /// An MVC-compatible wrapper around the WebForms EsccWebTeam.EastSussexGovUK.MasterPages.Controls.MasterPageControl control
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ViewUserControl{Escc.EastSussexGovUK.Mvc.Views.EastSussexGovUK.MasterPageControlData}" />
    public partial class MasterPageControl : ViewUserControl<MasterPageControlData>
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.masterPageControl.Control = Model.Control;
            this.masterPageControl.BreadcrumbProvider = Model.BreadcrumbProvider;
        }
    }
}