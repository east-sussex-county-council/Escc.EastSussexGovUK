using System;
using System.Web;
using Escc.Web;

namespace Escc.EastSussexGovUK.WebForms
{
    public partial class EastSussex1Space : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Visible)
            {
                var policy = new ContentSecurityPolicyHeaders(HttpContext.Current.Response.Headers);
                policy.AppendPolicy(new ContentSecurityPolicyFromConfig().Policies["EastSussex1Space"]);
                policy.UpdateHeaders();
            }
        }
    }
}