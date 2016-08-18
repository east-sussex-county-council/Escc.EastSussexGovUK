using System;
using System.Web;
using Escc.EastSussexGovUK.Skins;
using Escc.EastSussexGovUK.Views;
using Escc.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    public partial class TextSizePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var skinnable = Master as BaseMasterPage;
            if (skinnable != null)
            {
                skinnable.Skin = new CustomerFocusSkin(ViewSelector.CurrentViewIs(MasterPageFile));
            }

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
                var referrerQuery = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);
                referrerQuery.Remove("nocache");
                referrerQuery.Add("nocache", Guid.NewGuid().ToString());
                var redirectTo = new Uri(Request.UrlReferrer.Scheme + "://" + Request.UrlReferrer.Authority + Request.UrlReferrer.AbsolutePath + "?" + referrerQuery);
                new HttpStatus().SeeOther(redirectTo);
            }
        }
    }
}
