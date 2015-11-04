using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using Dapper;
using EsccWebTeam.Data.Web;
using Exceptionless;

namespace EsccWebTeam.EastSussexGovUK.MasterPages
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

            var requestedPath = NormaliseRequestedPath();

            if (!String.IsNullOrEmpty(requestedPath))
            {
                // Dereference linked data URIs
                // Linked data has separate URIs for things and documentation about those things. Try to match the URI for a thing and redirect to its documentation.
                var schoolUrl = String.Format(CultureInfo.InvariantCulture, ConfigurationManager.AppSettings["SchoolUrl"], "$1");
                TryUriPattern(requestedPath, "^id/school/([0-9]+)$", schoolUrl, 303); // schools
                TryUriPattern(requestedPath, "^id/school/([0-9]+)/closure/[0-9]+$", schoolUrl, 303); // school closures

                // See if it's a short URL for activating a service
                TryActivationUrls(requestedPath);

                // Try short URLs and moved pages in the database
                TryShortOrMovedUrl(requestedPath);

                // Try moved URLs which use regular expressions.
                // TryUriPattern(requestedPath, "^pattern-to-look-for$", "replacement-pattern", 301); 
            }

            // If none found, just show the content of this page (a 404 message)
            EastSussexGovUKContext.HttpStatus404NotFound(this.content);
        }

        /// <summary>
        /// See if it's a short URL for activating a service
        /// </summary>
        /// <param name="requestedPath">The requested path.</param>
        private void TryActivationUrls(string requestedPath)
        {
            // If not, see if it matches a short URL with a GUID, used to confirm subscriptions to services
            string guidPattern = "[A-Fa-f0-9]{8,8}-[A-Fa-f0-9]{4,4}-[A-Fa-f0-9]{4,4}-[A-Fa-f0-9]{4,4}-[A-Fa-f0-9]{12,12}";

            TryUriPattern(requestedPath, @"^schs\?c=(" + guidPattern + ")$", "/educationandlearning/schools/closurealerts/closurealertactivate.aspx?code=$1", 303);
            TryUriPattern(requestedPath, @"^schu\?c=(" + guidPattern + ")$", "/educationandlearning/schools/closurealerts/closurealertdeactivate.aspx?code=$1", 303);
        }


        /// <summary>
        /// Normalise the path which was originally requested - different depending on whether it came direct from IIS or via ASP.NET
        /// </summary>
        /// <returns></returns>
        private string NormaliseRequestedPath()
        {
            var requestedPath = String.Empty;
            if (!String.IsNullOrEmpty(Request.QueryString["aspxerrorpath"]))
            {
                requestedPath = Request.QueryString["aspxerrorpath"];
            }
            else
            {
                try
                {
                    string urlNotFound = Server.UrlDecode(Request.QueryString.ToString());
                    if (urlNotFound.Length > 4)
                    {
                        requestedPath = new Uri(urlNotFound.Substring(4)).PathAndQuery;
                    }

                }
                catch (UriFormatException)
                {
                    // If someone's trying to feed in something other than an unrecognised URL, just show them the standard 404
                    EastSussexGovUKContext.HttpStatus404NotFound(this.content);
                }
            }
            if (!String.IsNullOrEmpty(requestedPath))
            {
                requestedPath = requestedPath.Replace(Environment.NewLine, String.Empty).Trim('/').ToLower(CultureInfo.CurrentCulture);
            }
            return requestedPath;
        }


        /// <summary>
        /// Short URLs and moved pages are stored in a database. See whether there's a match, and redirect.
        /// </summary>
        /// <param name="requestedPath">The requested path.</param>
        private void TryShortOrMovedUrl(string requestedPath)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RedirectsReader"].ConnectionString))
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("@request", requestedPath);
                    using (var reader = conn.ExecuteReader("usp_Redirect_MatchRequest", parameter, commandType: CommandType.StoredProcedure))
                    {
                        while (reader.Read())
                        {
                            var redirectId = reader["RedirectId"].ToString();
                            var destinationUrl = reader["Destination"].ToString();
                            var redirectType = (RedirectType) Enum.Parse(typeof (RedirectType), reader["Type"].ToString());
                            HttpContext.Current.Response.AppendHeader("X-ESCC-Redirect", redirectId);
                            switch (redirectType)
                            {
                                case RedirectType.ShortUrl:
                                    GoToUrl(destinationUrl, 303);
                                    break;
                                case RedirectType.Moved:
                                    GoToUrl(destinationUrl, 301);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // If there's a problem, publish the error and continue to show 404 page
                ex.ToExceptionless().Submit();
            }
        }

        private enum RedirectType
        {
            ShortUrl = 1,
            Moved = 2
        }

        /// <summary>
        /// Redirect if the requested URL matches a regular expression pattern.
        /// </summary>
        /// <param name="requestedPath">Cleaned-up URL of originating request</param>
        /// <param name="requestedUriPattern">Regular expression pattern which should match the requested URI</param>
        /// <param name="destinationPattern">Pattern which, when used as the replacement for the regular expression pattern, should point to the destination URL.</param>
        /// <param name="httpStatus">The HTTP status.</param>
        private void TryUriPattern(string requestedPath, string requestedUriPattern, string destinationPattern, int httpStatus)
        {
            if (httpStatus != 301 && httpStatus != 303)
            {
                throw new ArgumentException("For linked data use HTTP status code 303 as the URL isn't wrong, just different from what we want to show. " +
                    "For old, moved sections use 301 as we don't want the old URL used again.", "httpStatus");
            }

            // If current request matches the pattern, redirect. 
            // Strictly speaking IgnoreCase should be false, but we shouldn't be creating 
            // any URIs that differ only by case so let's be helpful

            // Tried using regex within SQL from http://www.simple-talk.com/sql/t-sql-programming/tsql-regular-expression-workbench/
            // but it takes 500ms even with just two records in the table, compared to less than 1ms for ordinary matches.
            // Keep regex matches in code instead - editors aren't likely to be adding them anyway.
            if (Regex.IsMatch(requestedPath, requestedUriPattern, RegexOptions.IgnoreCase))
            {
                GoToUrl(Regex.Replace(requestedPath, requestedUriPattern, destinationPattern), httpStatus);
            }

        }


        /// <summary>
        /// Once a URL has been recognised, this redirects to it with correct HTTP response
        /// </summary>
        /// <param name="redirectTo">The URL to redirect to</param>
        /// <param name="httpStatus">The HTTP status.</param>
        private void GoToUrl(string redirectTo, int httpStatus)
        {
            // Generate redirect headers and end this response to ensure they're followed
            var destinationUrl = new Uri(redirectTo, UriKind.RelativeOrAbsolute);
            if (!destinationUrl.IsAbsoluteUri) destinationUrl = Iri.MakeAbsolute(destinationUrl, Request.Url, true);

            switch (httpStatus)
            {
                case 301:
                    Http.Status301MovedPermanently(destinationUrl);
                    break;
                case 303:
                    Http.Status303SeeOther(destinationUrl);
                    break;
            }
        }
    }
}