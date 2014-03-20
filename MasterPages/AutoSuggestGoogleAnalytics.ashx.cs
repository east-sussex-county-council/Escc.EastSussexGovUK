using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Web;
using EsccWebTeam.Data.Web;

namespace EsccWebTeam.EastSussexGovUK
{
    /// <summary>
    /// Return search terms tracked by Google Analytics as JSON, to be used as search suggestions
    /// </summary>
    public class AutoSuggestGoogleAnalytics : IHttpHandler
    {
        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            ProcessCorsHeaders(context);

            if (!String.IsNullOrEmpty(context.Request.QueryString["term"]))
            {
                using (var analyticsService = new GoogleAnalytics.Service())
                {
                    analyticsService.UseDefaultCredentials = true;
                    var keywords = analyticsService.GoogleAnalyticsSearchSuggestions(context.Request.QueryString["term"].ToLower(CultureInfo.CurrentCulture));
                    WriteResultAsJson(context, keywords);
                }
            }
        }

        private void ProcessCorsHeaders(HttpContext context)
        {
            var config = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/RemoteMasterPage") as NameValueCollection;
            if (config != null && !String.IsNullOrEmpty(config["CorsAllowedOrigins"]))
            {
                var allowedOrigins = new List<string>(config["CorsAllowedOrigins"].Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries));
                Cors.AllowCrossOriginRequest(context.Request, context.Response, allowedOrigins);
            }
        }

        /// <summary>
        /// Outputs the search terms as a JSON response
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="keywords">The keywords.</param>
        private void WriteResultAsJson(HttpContext context, string[] keywords)
        {
            context.Response.ContentType = "text/javascript";
            context.Response.Write("[");

            int len = keywords.Length;
            for (int i = 0; i < len; i++)
            {

                // preferred term needs only a value property
                context.Response.Write("{ \"label\": \"" + keywords[i].ToString() + "\", \"value\": \"" + keywords[i].ToString() + "\" }");

                if (i < len - 1) HttpContext.Current.Response.Write(",");
            }

            context.Response.Write("]");
        }



        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}