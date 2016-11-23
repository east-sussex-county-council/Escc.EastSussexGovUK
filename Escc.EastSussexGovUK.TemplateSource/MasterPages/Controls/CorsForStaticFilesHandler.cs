using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using Escc.Web;

namespace Escc.EastSussexGovUK.TemplateSource.MasterPages.Controls
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

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The context.</param>
        public void ProcessRequest(HttpContext context)
        {
            ProcessCorsHeaders(context);
            new HttpCacheHeaders().CacheUntil(context.Response.Cache, DateTime.Now.AddDays(1));

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
                new HttpStatus().NotFound(context.Response);
            }
        }

        private static void ProcessCorsHeaders(HttpContext context)
        {
            new CorsHeaders(context.Request.Headers, context.Response.Headers, new CorsPolicyFromConfig().CorsPolicy).UpdateHeaders();
        }

        #endregion
    }
}
