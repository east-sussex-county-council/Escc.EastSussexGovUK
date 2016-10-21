using System;
using System.IO;
using System.Reflection;
using System.Web;

namespace Escc.EastSussexGovUK.Rss
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class RssToHtml : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            // Return rss-to-html.xslt, but update the paths within to use assets embedded in this assembly
            context.Response.ContentType = "text/xsl";
            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Escc.EastSussexGovUK.Rss.eastsussexgovuk_rss.rss-to-html.xslt")))
            {
                var xslt = reader.ReadToEnd();
                xslt = xslt.Replace("{escc-logo}", new Uri(context.Request.Url, "escc-logo.gif").ToString());
                xslt = xslt.Replace("{rss-xslt.css}", new Uri(context.Request.Url, "rss-xslt.css").ToString());
                context.Response.Write(xslt);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}