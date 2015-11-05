using System.Security.Cryptography;
using System.Threading;
using System.Web.UI;
using EsccWebTeam.Data.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// Error page for an unhandled exception
    /// </summary>
    public partial class Status400 : Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            EastSussexGovUKContext.HttpStatus400BadRequest(this.errorContainer);
        }

    }
}
