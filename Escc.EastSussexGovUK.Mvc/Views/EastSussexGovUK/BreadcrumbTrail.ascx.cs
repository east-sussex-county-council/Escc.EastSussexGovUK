﻿using System;
using System.Web.Mvc;
using EsccWebTeam.EastSussexGovUK.MasterPages.Controls;

namespace Escc.EastSussexGovUK.Mvc.Views.EastSussexGovUK
{
    /// <summary>
    /// An MVC-compatible wrapper around the WebForms EsccWebTeam.EastSussexGovUK.MasterPages.Controls.BreadcrumbTrail control
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ViewUserControl{EsccWebTeam.EastSussexGovUK.MasterPages.Controls.IBreadcrumbProvider}" />
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