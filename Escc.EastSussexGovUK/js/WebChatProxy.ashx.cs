using Escc.Net;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
namespace EsccWebTeam.EastSussexGovUK.js
{
    /// <summary>
    /// Because the Agilisys web chat script uses document.write we can't use XHR to request the data, because it's async and the data arrives
    /// after the DOM has loaded and document.write can no longer operate. So, to still manage the settings remotely, use this proxy to request 
    /// the data and prepend a global variable, then include that in the script instead.
    /// </summary>
    public class WebChatProxy : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string json;

            // Request data
            var client = new HttpRequestClient(new ConfigurationProxyProvider());
            var request = client.CreateRequest(new Uri(ConfigurationManager.AppSettings["WebChatUrl"]));

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    json = reader.ReadToEnd();
                }
                    
                context.Response.StatusCode = (int)response.StatusCode;
            }

            // Return the data
            context.Response.ContentType = "application/javascript";
            context.Response.Write("esccWebChatData=" + json);
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