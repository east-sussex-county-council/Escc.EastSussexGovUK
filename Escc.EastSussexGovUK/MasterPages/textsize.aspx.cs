using System;
using System.Web;
using EsccWebTeam.Data.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    public partial class TextSize : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["textsize"] != null && Request.QueryString["textsize"].Length == 1)
            {
                HttpCookie textSize = new HttpCookie("textsize", Request.QueryString["textsize"]);
                textSize.Expires = DateTime.Now.AddMonths(1);
                if (Request.Url.Host.IndexOf("eastsussex.gov.uk", StringComparison.Ordinal) > -1) textSize.Domain = ".eastsussex.gov.uk";
                Response.Cookies.Add(textSize);
            }

            // Add a cache-busting parameter so that the user isn't returned to an HTTP-cached version of the page which has the old text size
            if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath != Request.Url.AbsolutePath)
            {
                var redirectTo = Iri.RemoveQueryStringParameter(Request.UrlReferrer, "nocache");
                redirectTo = new Uri(Iri.PrepareUrlForNewQueryStringParameter(redirectTo) + "nocache=" + Guid.NewGuid().ToString());
                Http.Status303SeeOther(redirectTo);
            }
            
            // Apps which run over https from another subdomain will never have a referrer, so have a general redirect ready for each.
            else if (Request.QueryString["from"] == "elibrary")
            {
                Response.StatusCode = 303;
                Response.AddHeader("Location", "/elibrary");
            }

        }
    }
}
