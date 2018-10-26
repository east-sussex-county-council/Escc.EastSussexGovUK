using System.Security.Cryptography;
using System.Threading;
using System.Web.UI;
using Escc.EastSussexGovUK.Skins;
using Escc.EastSussexGovUK.Views;

namespace Escc.EastSussexGovUK.WebForms
{
    /// <summary>
    /// Error page for a 403 Forbidden request, such as a directory without a default page
    /// </summary>
    public partial class HttpStatus403 : Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            var skinnable = Master as BaseMasterPage;
            if (skinnable != null)
            {
                skinnable.Skin = new CustomerFocusSkin();
            }

            // change status
            Response.Status = "403 Forbidden";

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
