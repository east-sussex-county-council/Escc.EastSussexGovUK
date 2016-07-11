using System.Security.Cryptography;
using System.Threading;
using System.Web.UI;

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
            }

            EastSussexGovUKContext.HttpStatus400BadRequest(this.errorContainer);
        }

    }
}
