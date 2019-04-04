using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Escc.Redirects;
using Exceptionless;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
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
        private readonly INotFoundRequestPathResolver _notFoundRequestPathResolver;
        private readonly IRedirectMatcher _redirectMatcher;
        private readonly IConvertToAbsoluteUrlHandler _convertToAbsoluteUrlHandler;
        private readonly IPreserveQueryStringHandler _preserveQueryStringHandler;

        public HttpStatusController(IEastSussexGovUKTemplateRequest templateRequest, IViewModelDefaultValuesProvider defaultModelValues, INotFoundRequestPathResolver notFoundRequestPathResolver, IRedirectMatcher redirectMatcher, IConvertToAbsoluteUrlHandler convertToAbsoluteUrlHandler, IPreserveQueryStringHandler preserveQueryStringHandler)
        {
            _templateRequest = templateRequest;
            _defaultModelValues = defaultModelValues;
            _notFoundRequestPathResolver = notFoundRequestPathResolver;
            _redirectMatcher = redirectMatcher;
            _convertToAbsoluteUrlHandler = convertToAbsoluteUrlHandler;
            _preserveQueryStringHandler = preserveQueryStringHandler;
        }

        /// <summary>
        /// Displays the 400 Bad Request HTTP status page
        /// </summary>
        /// <returns></returns>
        [Route("httpstatus/400")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Status400()
        {
            var model = new HttpStatusViewModel(_defaultModelValues)
            {
                TemplateHtml = await _templateRequest.RequestTemplateHtmlAsync(),
                RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier
            };
            return View("~/_EastSussexGovUK_HttpStatus_BadRequest.cshtml", model);
        }

        /// <summary>
        /// Displays the 403 Forbidden HTTP status page
        /// </summary>
        /// <returns></returns>
        [Route("httpstatus/403")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Status403()
        {
            RandomDelay();
            var model = new HttpStatusViewModel(_defaultModelValues)
            {
                TemplateHtml = await _templateRequest.RequestTemplateHtmlAsync(),
                RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier
            };
            return View("~/_EastSussexGovUK_HttpStatus_Forbidden.cshtml", model);
        }

        /// <summary>
        /// Displays the 410 Gone HTTP status page
        /// </summary>
        /// <returns></returns>
        [Route("httpstatus/410")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Status410()
        {
            var model = new HttpStatusViewModel(_defaultModelValues)
            {
                TemplateHtml = await _templateRequest.RequestTemplateHtmlAsync(),
                RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier
            };
            return View("~/_EastSussexGovUK_HttpStatus_Gone.cshtml", model);
        }

        /// <summary>
        /// Displays the 500 Internal Server Error HTTP status page
        /// </summary>
        /// <returns></returns>
        [Route("httpstatus/500")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Status500()
        {
            RandomDelay();

            var exceptionHandlerPathFeature = HttpContext?.Features.Get<IExceptionHandlerPathFeature>();
            exceptionHandlerPathFeature?.Error?.ToExceptionless().Submit();

            var model = new HttpStatusViewModel(_defaultModelValues)
            {
                TemplateHtml = await _templateRequest.RequestTemplateHtmlAsync(),
                RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier
            };
            return View("~/_EastSussexGovUK_HttpStatus_InternalServerError.cshtml", model);
        }

        /// <summary>
        /// Responds to a 404 Not Found status by checking for and executing redirects, and displaying the 404 HTTP status page otherwise
        /// </summary>
        /// <returns></returns>
        [Route("httpstatus/404")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Status404()
        {
            try
            {
                // Look for a redirect for the requested URL
                var requestFeature = HttpContext.Features.Get<IHttpRequestFeature>();
                var absoluteRequestedUrl = new Uri(new Uri(Request.GetDisplayUrl()), new Uri(requestFeature.RawTarget, UriKind.Relative));
                absoluteRequestedUrl = _notFoundRequestPathResolver?.NormaliseRequestedPath(absoluteRequestedUrl);

                var redirect = _redirectMatcher?.MatchRedirect(absoluteRequestedUrl);
                if (redirect != null)
                {
                    redirect = _convertToAbsoluteUrlHandler?.HandleRedirect(redirect) ?? redirect;
                    redirect = _preserveQueryStringHandler?.HandleRedirect(redirect) ?? redirect;
                    Response.Headers.Add("X-ESCC-Redirect", redirect.RedirectId.ToString());
                    Response.Headers.Add("Location", redirect.DestinationUrl.ToString());
                    return new StatusCodeResult(redirect.StatusCode);
                }
            }
            catch (Exception ex)
            {
                // If there's a problem with redirects, publish the error and continue to show 404 page
                ex.ToExceptionless().Submit();
            }

            // If no redirects matched, show the 404 page
            var model = new HttpStatusViewModel(_defaultModelValues)
            {
                TemplateHtml = await _templateRequest.RequestTemplateHtmlAsync(),
                RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier
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
    }
}