using System.Security.Cryptography;
using System.Threading;
using System.Web.UI;
using Escc.EastSussexGovUK;
using Escc.EastSussexGovUK.Skins;
using Escc.EastSussexGovUK.Views;
using Escc.EastSussexGovUK.WebForms;
using Escc.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// Error page for an unhandled exception
    /// </summary>
    public partial class Status400 : Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            var skinnable = Master as BaseMasterPage;
            if (skinnable != null)
            {
                skinnable.Skin = new CustomerFocusSkin(ViewSelector.CurrentViewIs(MasterPageFile));
                css.Attributes["class"] = skinnable.Skin.TextContentClass;
            }

            // Return the correct HTTP status code
            new HttpStatus().BadRequest();

            // Set the page title
            Page.Title = "Bad request";
        }

    }
}
