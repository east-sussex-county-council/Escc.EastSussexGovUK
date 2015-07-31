﻿using System;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Footer for the mobile template
    /// </summary>
    public partial class FooterMobile : System.Web.UI.UserControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Preprend the base URL if specified (which it should be if this is a subdomain of eastsussex.gov.uk)
            var context = new EastSussexGovUKContext();
            if (context.BaseUrl != null)
            {
                var urlPrefix = context.BaseUrl.ToString().TrimEnd('/');
                this.social.HRef = urlPrefix + this.social.HRef;
                this.about.HRef = urlPrefix + this.about.HRef;
                this.privacy.HRef = urlPrefix + this.privacy.HRef;
            }
        }
    }
}