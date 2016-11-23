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
                // See if it's a short URL for activating a service
                string guidPattern = "[A-Fa-f0-9]{8,8}-[A-Fa-f0-9]{4,4}-[A-Fa-f0-9]{4,4}-[A-Fa-f0-9]{4,4}-[A-Fa-f0-9]{12,12}";
                if (TryUriPattern(requestedUrl, @"^schs\?c=(" + guidPattern + ")$", "https://" + Request.Url.Authority + "/educationandlearning/schools/closurealerts/closurealertactivate.aspx?code=$1", 303))
                {
                    return;
                }
                if (TryUriPattern(requestedUrl, @"^schu\?c=(" + guidPattern + ")$", "https://" + Request.Url.Authority + "/educationandlearning/schools/closurealerts/closurealertdeactivate.aspx?code=$1", 303))
                {
                    return;
                }

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
                skinnable.Skin = new CustomerFocusSkin(ViewSelector.CurrentViewIs(MasterPageFile));
                css.Attributes["class"] = skinnable.Skin.TextContentClass;
            }

            var nonce = Guid.NewGuid().ToString().Replace("-", String.Empty);
            new ContentSecurityPolicyHeaders(Response.Headers).AppendPolicy($"script-src 'nonce-{nonce}'").UpdateHeaders();

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
                var matchers = new IRedirectMatcher[] {new SqlServerRedirectMatcher() { ThrowErrorOnMissingConfiguration = false } };
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

        /// <summary>
        /// Redirect if the requested URL matches a regular expression pattern.
        /// </summary>
        /// <param name="requestedUrl">The requested URL.</param>
        /// <param name="requestedUriPattern">Regular expression pattern which should match the requested URI</param>
        /// <param name="destinationPattern">Pattern which, when used as the replacement for the regular expression pattern, should point to the destination URL.</param>
        /// <param name="httpStatus">The HTTP status.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">For linked data use HTTP status code 303 as the URL isn't wrong, just different from what we want to show. " +
        ///                     "For old, moved sections use 301 as we don't want the old URL used again. - httpStatus</exception>
        private bool TryUriPattern(Uri requestedUrl, string requestedUriPattern, string destinationPattern, int httpStatus)
        {
            if (requestedUrl == null) return false;

            if (httpStatus != 301 && httpStatus != 303)
            {
                throw new ArgumentException("For linked data use HTTP status code 303 as the URL isn't wrong, just different from what we want to show. " +
                    "For old, moved sections use 301 as we don't want the old URL used again.", "httpStatus");
            }

            var requestedPath = requestedUrl.PathAndQuery.TrimStart('/');

            // If current request matches the pattern, redirect. 
            // Strictly speaking IgnoreCase should be false, but we shouldn't be creating 
            // any URIs that differ only by case so let's be helpful

            // Tried using regex within SQL from http://www.simple-talk.com/sql/t-sql-programming/tsql-regular-expression-workbench/
            // but it takes 500ms even with just two records in the table, compared to less than 1ms for ordinary matches.
            // Keep regex matches in code instead - editors aren't likely to be adding them anyway.
            if (Regex.IsMatch(requestedPath, requestedUriPattern, RegexOptions.IgnoreCase))
            {
                // Generate redirect headers and end this response to ensure they're followed
                var destinationUrl = new Uri(Regex.Replace(requestedPath, requestedUriPattern, destinationPattern), UriKind.RelativeOrAbsolute);
                if (!destinationUrl.IsAbsoluteUri) destinationUrl = new Uri(Request.Url, destinationUrl);

                switch (httpStatus)
                {
                    case 301:
                        new Web.HttpStatus().MovedPermanently(destinationUrl);
                        return true;
                    case 303:
                        new Web.HttpStatus().SeeOther(destinationUrl);
                        return true;
                }
            }
            return false;

        }
    }
}
