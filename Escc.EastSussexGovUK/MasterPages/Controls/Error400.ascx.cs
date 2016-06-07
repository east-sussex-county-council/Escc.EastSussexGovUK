using System;
using System.Web.UI;
using EsccWebTeam.Data.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Error message indicating that the request should not be repeated.
    /// </summary>
    public partial class Error400 : ErrorUserControl
    {
        /// <summary>
        /// Return 404 code and track 404 using Google Analytics
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Return the correct HTTP status code
            Http.Status400BadRequest();

            // Set the page title
            Page.Title = "Bad request";

            if (Skin != null)
            {
                css.Attributes["class"] = Skin.TextContentClass;
            }
        }
    }
}