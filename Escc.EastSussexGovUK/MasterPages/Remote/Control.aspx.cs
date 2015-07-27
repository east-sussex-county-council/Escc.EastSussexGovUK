using System;
using System.Text.RegularExpressions;
using System.Web;
using EsccWebTeam.Data.Web;
using EsccWebTeam.EastSussexGovUK.MasterPages.Controls;


namespace EsccWebTeam.EastSussexGovUK.MasterPages.Remote
{
    /// <summary>
    /// Loads a usercontrol in response to a remote request made by <see cref="MasterPageControl"/>
    /// </summary>
    public partial class Control : System.Web.UI.Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["control"]))
            {
                try
                {
                    // Sanitise request
                    string controlName = Regex.Replace(Request.QueryString["control"].ToUpperInvariant(), "[^0-9A-Z]", String.Empty);

                    // Load control, or fail with 400 if it doesn't exist
                    var usercontrol = LoadControl("~/masterpages/controls/" + controlName + ".ascx");
                    this.placeholder.Controls.Add(usercontrol);
                }
                catch (HttpException)
                {
                    // Usercontrol doesn't exist
                    Http.Status400BadRequest();
                }
            }
            else
            {
                Http.Status400BadRequest();
            }
        }
    }
}