using System.Security.Cryptography;
using System.Threading;
using System.Web.UI;

namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// Error page for a 403 Forbidden request, such as a directory without a default page
    /// </summary>
    public partial class HttpStatus403 : Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
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
