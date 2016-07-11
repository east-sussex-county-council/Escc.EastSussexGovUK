using System.Security.Cryptography;
using System.Threading;
using System.Web.UI;

namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// Error page for an unhandled exception
    /// </summary>
    public partial class HttpStatus500 : Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            var skinnable = Master as BaseMasterPage;
            if (skinnable != null)
            {
                skinnable.Skin = new CustomerFocusSkin(ViewSelector.CurrentViewIs(MasterPageFile));
            }

            // change status
            Response.Status = "500 Internal Server Error";

            // introduce random delay, so defend against anyone trying to detect errors based on the time taken
            // Code from http://weblogs.asp.net/scottgu/archive/2010/09/18/important-asp-net-security-vulnerability.aspx
            byte[] delay = new byte[1];
            using (RandomNumberGenerator prng = new RNGCryptoServiceProvider())
            {
                prng.GetBytes(delay);
                Thread.Sleep((int)delay[0]);
            }
        }

    }
}
