using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using EsccWebTeam.Data.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Data
{
    /// <summary>
    /// Set up a custom file extension in IIS to run against the .NET framework, then intercept the request for the extension and
    /// rewrite it to request another page. 
    /// </summary>
    /// <remarks>There is no need to register an HTTP handler for the extension, as it never reaches that
    /// stage in the request lifecycle. Used for alternative representations of the data on a page.</remarks>
    public class RewriteByExtensionModule : IHttpModule
    {
        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            // Begin request is the earliest possible hook into the request pipeline
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        /// <summary>
        /// Fires at the very start of the request pipeline.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void context_BeginRequest(object sender, EventArgs e)
        {
            NameValueCollection config = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/Data") as NameValueCollection;
            if (config == null) return;

            var context = HttpContext.Current;
            var urlExtension = Path.GetExtension(context.Request.Url.AbsolutePath.ToUpperInvariant());

            // Check whether an ASPX page is being requested with a custom extension
            foreach (string key in config)
            {
                if (key.StartsWith(".", StringComparison.Ordinal) && key.ToUpperInvariant() == urlExtension)
                {
                    ChangeExtensionAndParsePage(context.Request.Url, ".aspx", config[key]);
                }
            }

            // Check whether a Microsoft CMS page is being requested with a custom extension
            if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["NRORIGINALURL"]))
            {
                var postingUrl = Iri.MakeAbsolute(new Uri(HttpContext.Current.Request.QueryString["NRORIGINALURL"], UriKind.Relative));
                urlExtension = Path.GetExtension(postingUrl.AbsolutePath.ToUpperInvariant());

                foreach (string key in config)
                {
                    if (key.StartsWith(".", StringComparison.Ordinal) && key.ToUpperInvariant() == urlExtension)
                    {
                        ChangeExtensionAndParsePage(postingUrl, ".htm", config[key]);
                    }
                }
            }
        }

        private static void ChangeExtensionAndParsePage(Uri requestedUrl, string realExtension, string parserUrl)
        {
            var urlToParse = Path.ChangeExtension(requestedUrl.AbsolutePath, realExtension);
            if (requestedUrl.Query.Length > 1) urlToParse += requestedUrl.Query;

            HttpContext.Current.RewritePath(String.Format(CultureInfo.InvariantCulture, parserUrl, HttpUtility.UrlEncode(urlToParse)));
        }


    }
}