using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EsccWebTeam.Data.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    public partial class EastSussex1Space : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Visible)
            {
                var policy = new ContentSecurityPolicy(HttpContext.Current.Request.Url);
                policy.ParsePolicy(HttpContext.Current.Response.Headers["Content-Security-Policy"], true);
                policy.AppendFromConfig("EastSussex1Space");
                policy.UpdateHeader(HttpContext.Current.Response);
            }
        }
    }
}