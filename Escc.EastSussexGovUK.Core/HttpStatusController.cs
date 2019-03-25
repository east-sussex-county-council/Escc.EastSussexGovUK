using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
//using Escc.Redirects;
//using Escc.Redirects.Handlers;
using Exceptionless;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// Return standard pages to display for HTTP status codes
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [AllowAnonymous]
    public class HttpStatusController : Controller
    {
        private readonly IEastSussexGovUKTemplateRequest _templateRequest;
        private readonly IViewModelDefaultValuesProvider _defaultModelValues;

        public HttpStatusController(IEastSussexGovUKTemplateRequest templateRequest, IViewModelDefaultValuesProvider defaultModelValues)
        {
            _templateRequest = templateRequest;
            _defaultModelValues = defaultModelValues;
        }

        /// <summary>
        /// Displays the 400 Bad Request HTTP status page
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Status400()
        {
            var model = new HttpStatusViewModel(_defaultModelValues)
            {
                TemplateHtml = await _templateRequest.RequestTemplateHtmlAsync(),
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View("~/_EastSussexGovUK_HttpStatus_BadRequest.cshtml", model);
        }
        
        /// <summary>
        /// Displays the 403 Forbidden HTTP status page
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Status403()
        {
            RandomDelay();
            var model = new HttpStatusViewModel(_defaultModelValues)
            {
                TemplateHtml = await _templateRequest.RequestTemplateHtmlAsync(),
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View("~/_EastSussexGovUK_HttpStatus_Forbidden.cshtml", model);
        }

        /// <summary>
        /// Displays the 410 Gone HTTP status page
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Status410()
        {
            var model = new HttpStatusViewModel(_defaultModelValues)
            {
                TemplateHtml = await _templateRequest.RequestTemplateHtmlAsync(),
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View("~/_EastSussexGovUK_HttpStatus_Gone.cshtml", model);
        }

        /// <summary>
        /// Displays the 500 Internal Server Error HTTP status page
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Status500()
        {
            RandomDelay();

            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            exceptionHandlerPathFeature?.Error?.ToExceptionless().Submit();

            var model = new HttpStatusViewModel(_defaultModelValues)
            {
                TemplateHtml = await _templateRequest.RequestTemplateHtmlAsync(),
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View("~/_EastSussexGovUK_HttpStatus_InternalServerError.cshtml", model);
        }
        
        /// <summary>
        /// Responds to a 404 Not Found status by checking for and executing redirects, and displaying the 404 HTTP status page otherwise
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Status404()
        {
            /* var requestedPath = new NotFoundRequestPathResolver().NormaliseRequestedPath(Request.Url);

             // Dereference linked data URIs
             // Linked data has separate URIs for things and documentation about those things. Try to match the URI for a thing and redirect to its documentation.
             ActionResult redirectResult;
             if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SchoolUrl"]))
             {
                 var schoolUrl = String.Format(CultureInfo.InvariantCulture, ConfigurationManager.AppSettings["SchoolUrl"], "$1");
                 redirectResult = TryUriPattern(requestedPath, "^id/school/([0-9]+)$", schoolUrl, 303, Response.Headers);
                 if (redirectResult != null) return redirectResult;

                 redirectResult = TryUriPattern(requestedPath, "^id/school/([0-9]+)/closure/[0-9]+$", schoolUrl, 303, Response.Headers);
                 if (redirectResult != null) return redirectResult;
             }

             // See if it's a short URL for activating a service
             string guidPattern = "[A-Fa-f0-9]{8,8}-[A-Fa-f0-9]{4,4}-[A-Fa-f0-9]{4,4}-[A-Fa-f0-9]{4,4}-[A-Fa-f0-9]{12,12}";
             redirectResult = TryUriPattern(requestedPath, @"^schs\?c=(" + guidPattern + ")$", "https://" + Request.Url.Authority + "/educationandlearning/schools/schoolclosures/closurealertactivate.aspx?code=$1", 303, Response.Headers);
             if (redirectResult != null) return redirectResult;

             redirectResult = TryUriPattern(requestedPath, @"^schu\?c=(" + guidPattern + ")$", "https://" + Request.Url.Authority + "/educationandlearning/schools/schoolclosures/closurealertdeactivate.aspx?code=$1", 303, Response.Headers);
             if (redirectResult != null) return redirectResult;

             if (TryShortOrMovedUrl(requestedPath))
             {
                 // redirect matched and followed - stop processing
                 return null;
             }*/

             // If no redirects matched, show the 404 page
             var model = new HttpStatusViewModel(_defaultModelValues)
             {
                 TemplateHtml = await _templateRequest.RequestTemplateHtmlAsync(),
                 RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
             };

             return View("~/_EastSussexGovUK_HttpStatus_NotFound.cshtml", model);
         }
         
        private void RandomDelay()
        {
            // introduce random delay, so defend against anyone trying to detect errors based on the time taken
            // Code from http://weblogs.asp.net/scottgu/archive/2010/09/18/important-asp-net-security-vulnerability.aspx
            byte[] delay = new byte[1];
            using (RandomNumberGenerator prng = new RNGCryptoServiceProvider())
            {
                prng.GetBytes(delay);
                Thread.Sleep((int)delay[0]);
            }
        }
        /*

        /// <summary>
        /// Redirect if the requested URL matches a regular expression pattern.
        /// </summary>
        /// <param name="requestedPath">Cleaned-up URL of originating request</param>
        /// <param name="requestedUriPattern">Regular expression pattern which should match the requested URI</param>
        /// <param name="destinationPattern">Pattern which, when used as the replacement for the regular expression pattern, should point to the destination URL.</param>
        /// <param name="httpStatus">The HTTP status.</param>
        private ActionResult TryUriPattern(Uri requestedPath, string requestedUriPattern, string destinationPattern, int httpStatus, NameValueCollection responseHeaders)
        {
            if (requestedPath == null) return null;

            if (httpStatus != 301 && httpStatus != 303)
            {
                throw new ArgumentException("For linked data use HTTP status code 303 as the URL isn't wrong, just different from what we want to show. " +
                    "For old, moved sections use 301 as we don't want the old URL used again.", nameof(httpStatus));
            }
            var requestedLocalPath = requestedPath.PathAndQuery.TrimStart('/');

            // If current request matches the pattern, redirect. 
            // Strictly speaking IgnoreCase should be false, but we shouldn't be creating 
            // any URIs that differ only by case so let's be helpful

            // Tried using regex within SQL from http://www.simple-talk.com/sql/t-sql-programming/tsql-regular-expression-workbench/
            // but it takes 500ms even with just two records in the table, compared to less than 1ms for ordinary matches.
            // Keep regex matches in code instead - editors aren't likely to be adding them anyway.
            if (Regex.IsMatch(requestedLocalPath, requestedUriPattern, RegexOptions.IgnoreCase))
            {
                // Generate redirect headers and end this response to ensure they're followed
                var destinationUrl = new Uri(Regex.Replace(requestedLocalPath, requestedUriPattern, destinationPattern), UriKind.RelativeOrAbsolute);
                if (!destinationUrl.IsAbsoluteUri) destinationUrl = new Uri(Request.Url, destinationUrl);

                switch (httpStatus)
                {
                    case 301:
                        return new RedirectResult(destinationUrl.ToString(), true);
                    case 303:
                        responseHeaders.Add("Location", destinationUrl.ToString());
                        return new HttpStatusCodeResult(303); 
                }
            }
            return null;
        }

        /// <summary>
        /// Short URLs and moved pages are stored in a database. See whether there's a match, and redirect.
        /// </summary>
        /// <param name="requestedUrl">The requested path.</param>
        private bool TryShortOrMovedUrl(Uri requestedUrl)
        {
            if (!String.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["RedirectsReader"]?.ConnectionString))
            {
                try
                {
                    // Try to match the requested URL to a redirect and, if successful, run handlers for the redirect
                    var matchers = new IRedirectMatcher[] { new SqlServerRedirectMatcher(ConfigurationManager.ConnectionStrings["RedirectsReader"].ConnectionString) };
                    var handlers = new IRedirectHandler[] { new ConvertToAbsoluteUrlHandler(), new PreserveQueryStringHandler(), new DebugInfoHandler(), new GoToUrlHandler() };

                    foreach (var matcher in matchers)
                    {
                        var redirect = matcher.MatchRedirect(requestedUrl);
                        if (redirect != null)
                        {
                            foreach (var handler in handlers)
                            {
                                redirect = handler.HandleRedirect(redirect);
                            }
                            return true;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // If there's a problem, publish the error and continue to show 404 page
                    ex.ToExceptionless().Submit();
                }
            }
            return false;
        }*/
    }
}