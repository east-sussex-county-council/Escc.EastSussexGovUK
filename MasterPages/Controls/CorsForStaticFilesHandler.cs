using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using EsccWebTeam.Data.Web;
using EsccWebTeam.EastSussexGovUK.MasterPages.Remote;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Serve a static file with CORS headers and HTTP caching
    /// </summary>
    public class CorsForStaticFilesHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessCorsHeaders(context);
            Http.CacheFor(24,0,context.Response);

            var filename = HostingEnvironment.MapPath(context.Request.Url.AbsolutePath);
            if (!String.IsNullOrEmpty(filename) && File.Exists(filename))
            {
                using (var reader = new StreamReader(filename))
                {
                    string line;
                    while(!String.IsNullOrEmpty(line = reader.ReadLine()))
                    {
                        context.Response.Write(line);
                    }
                }
            }
            else
            {
                Http.Status404NotFound(context.Response);
            }
        }

        private static void ProcessCorsHeaders(HttpContext context)
        {
            Cors.AllowCrossOriginRequest(context.Request, context.Response, new RemoteMasterPageXmlConfigurationProvider().CorsAllowedOrigins());
        }

        #endregion
    }
}
