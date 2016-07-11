using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Escc.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
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