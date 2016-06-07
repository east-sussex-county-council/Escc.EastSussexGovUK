using System;
using System.Web.UI;
using EsccWebTeam.Data.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Error message to be displayed when a page has been removed.
    /// </summary>
    public partial class Error310 : ErrorUserControl
    {
        /// <summary>
        /// Return 310 code 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Return the correct HTTP status code
            Http.Status310Gone();

            // Set the page title
            Page.Title = "Page gone";

            if (Skin != null)
            {
                css.Attributes["class"] = Skin.TextContentClass;
            }
        }
    }
}