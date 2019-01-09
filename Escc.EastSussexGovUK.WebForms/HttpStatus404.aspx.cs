using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using Escc.EastSussexGovUK.Skins;
using Escc.EastSussexGovUK.Views;
using Escc.Redirects;
using Escc.Redirects.Handlers;
using Escc.Web;
using Exceptionless;

namespace Escc.EastSussexGovUK.WebForms
{
    /// <summary>
    /// If a request is not found, check whether it should be redirected, or return a 404
    /// </summary>
    public partial class NotFound : System.Web.UI.Page
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            var requestedUrl = new NotFoundRequestPathResolver().NormaliseRequestedPath(Request.Url);

            if (requestedUrl != null)
            {
                // Try short URLs and moved pages in the database
                if (TryShortOrMovedUrl(requestedUrl))
                {
                    return;
                }

                // Try moved URLs which use regular expressions.
                // TryUriPattern(requestedPath, "^pattern-to-look-for$", "replacement-pattern", 301); 
            }

            // If none found, just show the content of this page (a 404 message)
            Show404(requestedUrl);
        }

        private void Show404(Uri requestedUrl)
        {
            // Return the correct HTTP status code
            new Web.HttpStatus().NotFound();

            // Set the page title
            Page.Title = "Page not found";

            var skinnable = Master as BaseMasterPage;
            if (skinnable != null)
            {
                skinnable.Skin = new CustomerFocusSkin();
            }

            var nonce = Guid.NewGuid().ToString().Replace("-", String.Empty);
            var config = new ContentSecurityPolicyFromConfig();
            var filter = new ContentSecurityPolicyUrlFilter(Request.Url, config.UrlsToExclude);
            if (filter.ApplyPolicy() && !Response.HeadersWritten)
            {
                new ContentSecurityPolicyHeaders(Response.Headers).AppendPolicy($"script-src 'nonce-{nonce}'").UpdateHeaders();
            }

            // Configure the tracking script and track the 404 with Google Analytics
            script.TagName = "script";
            script.Attributes.Add("nonce", nonce);

            if (requestedUrl != null)
            {
                script.Attributes.Add("data-request", Server.HtmlEncode(Regex.Replace(requestedUrl.PathAndQuery, @"[^A-Za-z0-9/\-_\.\?=:#+%]", String.Empty)));
            }

            var normalisedReferrer = String.Empty;
            try
            {
                if (Request.UrlReferrer != null)
                {
                    normalisedReferrer = Request.UrlReferrer.ToString().Replace("'", "\'");
                }
            }
            catch (UriFormatException)
            {
                // Catch this error and simply ignore the referrer if it is an invalid URI, which can happen in a hacking scenario.
                // For example, if the request contains an invalid referring URL such as http://google.com', when you access the 
                // Request.UrlReferrer property .NET creates a Uri instance which throws this exception.
            }

            script.Attributes.Add("data-referrer", Server.HtmlEncode(Regex.Replace(normalisedReferrer, @"[^A-Za-z0-9/\-_\.\?=:#+%]", String.Empty)));
        }

        /// <summary>
        /// Short URLs and moved pages are stored in a database. See whether there's a match, and redirect.
        /// </summary>
        /// <param name="requestedUrl">The requested URL.</param>
        private bool TryShortOrMovedUrl(Uri requestedUrl)
        {
            try
            {
                // Try to match the requested URL to a redirect and, if successful, run handlers for the redirect
                var matchers = new IRedirectMatcher[] {new SqlServerRedirectMatcher(ConfigurationManager.ConnectionStrings["RedirectsReader"].ConnectionString) };
                var handlers = new IRedirectHandler[] {new ConvertToAbsoluteUrlHandler(), new PreserveQueryStringHandler(), new DebugInfoHandler(), new GoToUrlHandler()};

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
            return false;
        }
    }
}
