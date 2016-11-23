using System;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Features;

namespace Escc.EastSussexGovUK.Mvc.Views.EastSussexGovUK
{
    /// <summary>
    /// An MVC-compatible wrapper around the WebForms Escc.EastSussexGovUK.Features.BreadcrumbTrail control
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ViewUserControl{IBreadcrumbProvider}" />
    public partial class BreadcrumbTrail : ViewUserControl<IBreadcrumbProvider>
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.breadcrumb.BreadcrumbProvider = Model;
        }
    }
}