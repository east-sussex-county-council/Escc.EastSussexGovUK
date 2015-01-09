using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Caching;
using EsccWebTeam.Data.Web;
using EsccWebTeam.Data.Xml;
using EsccWebTeam.EastSussexGovUK.MasterPages.Remote;

namespace EsccWebTeam.EastSussexGovUK.js
{
    /// <summary>
    /// IE8 and IE9 won't allow CORS requests where the protocol differs. Since www.eastsussex.gov.uk uses HTTP but new.eastsussex.gov.uk uses HTTPS, 
    /// proxy the data over HTTP to allow it work until all pages are migrated to HTTPS.
    /// </summary>
    public class ServiceAlertsProxy : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var alerts = String.Empty;
            
            // Get from local cache if it's not expired
            if (context.Cache["ServiceAlertsProxy"] != null)
            {
                alerts = context.Cache["ServiceAlertsProxy"].ToString();
            }

            if (String.IsNullOrEmpty(alerts))
            {
                // Request service alert data
                var request = XmlHttpRequest.Create(new Uri(ConfigurationManager.AppSettings["ServiceAlertsUrl"]));
                var expiryDate = DateTime.UtcNow.AddMinutes(5);

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        alerts = reader.ReadToEnd();
                    }
                    
                    // Proxy important headers from the source, and use the expiry date to expire a local cache
                    var expires = response.GetResponseHeader("Expires");
                    if (!String.IsNullOrEmpty(expires)) expiryDate = DateTime.Parse(expires);

                    context.Response.StatusCode = (int)response.StatusCode;
                    context.Cache.Insert("ServiceAlertsProxy", alerts, null, expiryDate, Cache.NoSlidingExpiration);
                }
            }

            // Return the alerts
            Cors.AllowCrossOriginRequest(context.Request, context.Response, new RemoteMasterPageXmlConfigurationProvider().CorsAllowedOrigins());
            context.Response.ContentType = "application/javascript";
            context.Response.Write(alerts);
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