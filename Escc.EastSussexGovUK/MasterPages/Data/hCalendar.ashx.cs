using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using EsccWebTeam.Data.Web;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Escc.Net;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Data
{
    /// <summary>
    /// Parse a web page for hCalendar microformats contained within it. Called using <see cref="RewriteByExtensionModule"/>.
    /// </summary>
    public class HCalendar : IHttpHandler
    {

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            if (String.IsNullOrEmpty(context.Request.QueryString["url"])) NothingToDo(context);

            NameValueCollection config = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/Data") as NameValueCollection;
            if (config == null) throw new ConfigurationErrorsException("Configuration section not found: <EsccWebTeam.EastSussexGovUK><Data /></EsccWebTeam.EastSussexGovUK>");

            try
            {
                Uri requestedUri = new Uri(context.Request.QueryString["url"], UriKind.RelativeOrAbsolute);
                requestedUri = Iri.MakeAbsolute(requestedUri);
                Uri uriToProcess = requestedUri;

                uriToProcess = calendar.TransformHost(config, uriToProcess);

                // Run code relevant to the requested microformat
                ParseHCalendar(context, config, uriToProcess, requestedUri);

                // Cache, but not for long because this could be updates to existing events at short notice, eg cancellations and closures
                Http.CacheFor(0,5);
            }
            catch (ThreadAbortException)
            {
                // just ignore this exception, it comes from calling Response.End in NothingToDo()
                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                NothingToDo(context);
            }
        }

        /// <summary>
        /// Parses the the given URI for any hCalendar microformats and returns the response as an iCalendar file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configurationSettings">The configuration settings.</param>
        /// <param name="uriToProcess">The URI to process.</param>
        /// <param name="requestedUri">The requested URI.</param>
        private void ParseHCalendar(HttpContext context, NameValueCollection configurationSettings, Uri uriToProcess, Uri requestedUri)
        {
            // Ensure we have the config value pointing to the XSLT which transforms a web page into an iCalendar
            // XSLT comes from a project called X2V by Brian Suda http://suda.co.uk/projects/microformats/hcalendar/
            if (String.IsNullOrEmpty(configurationSettings["HCalendarPrepareXslt"])) throw new ConfigurationErrorsException("Configuration setting not found: <EsccWebTeam.EastSussexGovUK><Data><add key=\"HCalendarPrepareXslt\" value=\"filename\" /></Data></EsccWebTeam.EastSussexGovUK>");
            if (String.IsNullOrEmpty(configurationSettings["HCalendarXslt"])) throw new ConfigurationErrorsException("Configuration setting not found: <EsccWebTeam.EastSussexGovUK><Data><add key=\"HCalendarXslt\" value=\"filename\" /></Data></EsccWebTeam.EastSussexGovUK>");

            // Get the web page, transform it using XSLT, and return the result as a string
            try
            {
                // This is a request for a local web page so we never need a proxy (and in fact it causes problems).
                // However using HttpRequestClient.CreateRequest uses the proxy credentials for the request itself, and that is
                // necessary to work on staging servers.
                uriToProcess = new Uri(Iri.PrepareUrlForNewQueryStringParameter(uriToProcess) + "template=plain",
                    UriKind.RelativeOrAbsolute);
                var client = new HttpRequestClient(new ConfigurationProxyProvider());
                var request = client.CreateRequest(uriToProcess);
                request.Proxy = null;
                var responseXml = client.RequestXPath(request, 2);

                // First update HTML to a format the hCalendar XSLT can handle
                var transform = new XslCompiledTransform(false);
                transform.Load(HttpContext.Current.Server.MapPath(configurationSettings["HCalendarPrepareXslt"]));
                var modifiedXml = new StringWriter();
                transform.Transform(responseXml.CreateNavigator(), null, modifiedXml);

                // Load the modified HTML, and transform to iCalendar 
                responseXml = new XPathDocument(new StringReader(modifiedXml.ToString()));
                transform.Load(HttpContext.Current.Server.MapPath(configurationSettings["HCalendarXslt"]));
                modifiedXml = new StringWriter();
                transform.Transform(responseXml.CreateNavigator(), null, modifiedXml);

                // Get the text and add the source URL it was downloaded from
                var responseText = modifiedXml.ToString();
                var sourceUrl =
                    Iri.MakeAbsolute(new Uri(Path.ChangeExtension(requestedUri.AbsolutePath, ".ics"), UriKind.Relative))
                        .ToString();
                if (requestedUri.Query.Length > 1) sourceUrl += requestedUri.Query;
                responseText = responseText.Replace("(Best Practice: should be URL that this was ripped from)",
                    sourceUrl);

                // Replace Unicode n-dash with hyphen because iCalendar format is ASCII
                responseText = responseText.Replace("–", "-");

                // Ensure there's a timezone specified (either UTC or UK) ONLY IF a time is also specified. 
                // Specifying a timezone ensures events display with the correct localised time in calendars 
                // set to timezones other than UK. See http://blog.jonudell.net/2011/10/17/x-wr-timezone-considered-harmful/
                // 
                // Why would we ever have hCalendar data that doesn't have a timezone or wasn't already UTC? 
                // For all-day events to show as all-day events you need to specify a "floating time", eg 4 July.
                // In Google Calendar you can get away with giving a timezone using TZID=UK, but for Outlook to 
                // recognise all-day events you have to omit the timezone and have a genuine "floating time".
                responseText = Regex.Replace(responseText, "DTSTART;VALUE=DATE-TIME(.*?)[^Z]" + Environment.NewLine,
                    "DTSTART;TZID=UK;VALUE=DATE-TIME$1" + Environment.NewLine);
                responseText = Regex.Replace(responseText, "DTEND;VALUE=DATE-TIME(.*?)[^Z]" + Environment.NewLine,
                    "DTEND;TZID=UK;VALUE=DATE-TIME$1" + Environment.NewLine);

                var ukTimeZone = new StringBuilder("METHOD:PUBLISH").Append(Environment.NewLine)
                    .Append("BEGIN:VTIMEZONE").Append(Environment.NewLine)
                    .Append("TZID:UK").Append(Environment.NewLine)
                    .Append("X-LIC-LOCATION:Europe/London").Append(Environment.NewLine)
                    .Append("BEGIN:DAYLIGHT").Append(Environment.NewLine)
                    .Append("TZOFFSETFROM:+0000").Append(Environment.NewLine)
                    .Append("TZOFFSETTO:+0100").Append(Environment.NewLine)
                    .Append("TZNAME:BST").Append(Environment.NewLine)
                    .Append("DTSTART:19700329T010000").Append(Environment.NewLine)
                    .Append("RRULE:FREQ=YEARLY;INTERVAL=1;BYDAY=-1SU;BYMONTH=3").Append(Environment.NewLine)
                    .Append("END:DAYLIGHT").Append(Environment.NewLine)
                    .Append("BEGIN:STANDARD").Append(Environment.NewLine)
                    .Append("TZOFFSETFROM:+0100").Append(Environment.NewLine)
                    .Append("TZOFFSETTO:+0000").Append(Environment.NewLine)
                    .Append("TZNAME:GMT").Append(Environment.NewLine)
                    .Append("DTSTART:19701025T020000").Append(Environment.NewLine)
                    .Append("RRULE:FREQ=YEARLY;INTERVAL=1;BYDAY=-1SU;BYMONTH=10").Append(Environment.NewLine)
                    .Append("END:STANDARD").Append(Environment.NewLine)
                    .Append("END:VTIMEZONE");
                responseText = responseText.Replace("METHOD:PUBLISH", ukTimeZone.ToString());

                // DTSTAMP needs to have a timezone or Google Calendar won't import it, and RFC2445 which defines iCalendar says
                // in section 4.8.7.2 "The property indicates the date/time that the instance of the iCalendar object was created...
                // The value MUST be specified in the UTC time format." Since we're creating the iCalendar object now, remove any
                // existing DTSTAMP and add a correct DTSTAMP property here. DTSTAMP needs to be present for Outlook 2003 to work.
                responseText = Regex.Replace(responseText, "DTSTAMP:.*?" + Environment.NewLine, String.Empty);
                responseText = responseText.Replace("END:VEVENT",
                    "DTSTAMP:" + DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture) +
                    Environment.NewLine + "END:VEVENT");

                // Specify that the event should be marked as free time, not busy
                responseText = responseText.Replace("END:VEVENT",
                    "X-MICROSOFT-CDO-INTENDEDSTATUS:FREE" + Environment.NewLine + "END:VEVENT"); // Outlook
                responseText = responseText.Replace("END:VEVENT",
                    "X-MICROSOFT-CDO-BUSYSTATUS:FREE" + Environment.NewLine + "END:VEVENT"); // Outlook
                responseText = responseText.Replace("END:VEVENT",
                    "TRANSP:TRANSPARENT" + Environment.NewLine + "END:VEVENT"); // Google Calendar

                // Return the string as an iCalendar download
                context.Response.ContentType = "text/calendar";
                context.Response.AddHeader("Content-Disposition", "attachment; filename=calendar.ics");
                context.Response.Write(responseText);
            }
            catch (XmlException ex)
            {
                ex.Data.Add("URL to process", uriToProcess);
                throw;
            }
            catch (WebException ex)
            {
                if (ex.Message == "The remote server returned an error: (310) Gone.")
                {
                    // If the page we're trying to parse has gone, this representation of it has gone too.
                    Http.Status310Gone(context.Response);
                    context.Response.ContentType = "text/plain";
                    context.Response.End();
                }
                else if (ex.Message == "The remote server returned an error: (404) Not Found.")
                {
                    // If the page we're trying to parse doesn't exist, this representation of it doesn't exist either.
                    Http.Status404NotFound(context.Response);
                    context.Response.ContentType = "text/plain";
                    context.Response.End();
                }
                else
                {
                    ex.Data.Add("URL to process", uriToProcess);
                    throw;
                }
            }
        }

        /// <summary>
        /// Takes action if request was not valid. 
        /// </summary>
        /// <param name="context">The context.</param>
        private void NothingToDo(HttpContext context)
        {
            Http.Status404NotFound(context.Response);
            context.Response.ContentType = "text/plain";
            context.Response.End();
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
                return true;
            }
        }
    }
}
