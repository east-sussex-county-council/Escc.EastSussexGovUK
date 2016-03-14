using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using Escc.Dates;
using Escc.Html;
using EsccWebTeam.Data.Web;
using Escc.Net;
using Exceptionless;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Data
{
    /// <summary>
    /// Displays a calendar link with some help text. Called using <see cref="RewriteByExtensionModule"/>.
    /// </summary>
    public partial class calendar : System.Web.UI.Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Request.QueryString["url"]))
            {
                EastSussexGovUKContext.HttpStatus404NotFound(this.article);
                return;
            }

            NameValueCollection config = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/Data") as NameValueCollection;
            if (config == null) throw new ConfigurationErrorsException("Configuration section not found: <EsccWebTeam.EastSussexGovUK><Data /></EsccWebTeam.EastSussexGovUK>");

            Uri requestedUri = new Uri(Request.QueryString["url"], UriKind.RelativeOrAbsolute);
            requestedUri = Iri.MakeAbsolute(requestedUri);
            Uri uriToProcess = requestedUri;

            uriToProcess = TransformHost(config, uriToProcess);

            try
            {
                // Get data from the page to be parsed, to build this page
                ParsePageForMetadata(uriToProcess, requestedUri);

                // Link to the actual calendar download
                var urlToParse = Path.ChangeExtension(requestedUri.AbsolutePath, ".ics");
                if (requestedUri.Query.Length > 1) urlToParse += requestedUri.Query;

                this.download.HRef = HttpUtility.HtmlAttributeEncode(urlToParse);
                this.subscribe.HRef = HttpUtility.HtmlAttributeEncode("webcals://" + Request.Url.Authority + urlToParse);

                var ukNow = DateTime.Now.ToUkDateTime();
                Http.CacheDaily(ukNow.Hour, ukNow.Minute);
            }
            catch (Exception ex)
            {
                ex.Data.Add("URL requested", uriToProcess);
                ex.ToExceptionless().Submit();
                EastSussexGovUKContext.HttpStatus404NotFound(this.article);
            }
        }

        /// <summary>
        /// Transforms the host name, for hosts which can't make requests to their own hostname.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="uriToProcess">The URI to process.</param>
        /// <returns></returns>
        internal static Uri TransformHost(NameValueCollection config, Uri uriToProcess)
        {
            if (config["TransformHosts"] != null)
            {
                List<string> beforeAndAfter = new List<string>(config["TransformHosts"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                Dictionary<string, string> hostTransforms = new Dictionary<string, string>();
                foreach (string pair in beforeAndAfter)
                {
                    string[] splitPair = pair.Split('=');
                    hostTransforms.Add(splitPair[0], splitPair[1]);
                }

                if (hostTransforms.ContainsKey(uriToProcess.Host))
                {
                    uriToProcess = new Uri(uriToProcess.ToString().Replace("://" + uriToProcess.Host, "://" + hostTransforms[uriToProcess.Host]));
                }
            }
            return uriToProcess;
        }

        /// <summary>
        /// Parses the the given URI for any hCalendar microformats and returns the response as an iCalendar file.
        /// </summary>
        /// <param name="uriToProcess">The URI to process.</param>
        /// <param name="requestedUri">The requested URI.</param>
        private void ParsePageForMetadata(Uri uriToProcess, Uri requestedUri)
        {
            // Get the web page and use its metadata to build this page
            try
            {
                // This is a request for a local web page so we never need a proxy (and in fact it causes problems).
                // However using HttpRequestClient.CreateRequest uses the proxy credentials for the request itself, and that is
                // necessary to work on staging servers.
                uriToProcess = new Uri(Iri.PrepareUrlForNewQueryStringParameter(uriToProcess) + "template=plain", UriKind.RelativeOrAbsolute);
                var client = new HttpRequestClient(new ConfigurationProxyProvider());
                var request = client.CreateRequest(uriToProcess);
                request.Proxy = null;
                var responseXml = client.RequestXPath(request,2);

                var nav = responseXml.CreateNavigator();
                var namespaceManager = new XmlNamespaceManager(nav.NameTable);
                nav.MoveToRoot();
                nav.MoveToFirstChild();
                namespaceManager.AddNamespace("xhtml", nav.NamespaceURI);

                var property = GetMetadataField("/xhtml:html/xhtml:body//xhtml:h1", nav, namespaceManager);
                if (!String.IsNullOrEmpty(property))
                {
                    var html = new HtmlTagSantiser();
                    property = html.StripTags(property);
                    this.headcontent.Title = String.Format(CultureInfo.CurrentCulture, this.headcontent.Title, property);
                    this.heading.InnerHtml = String.Format(CultureInfo.CurrentCulture, this.heading.InnerHtml, property);
                    this.download.InnerHtml = String.Format(CultureInfo.CurrentCulture, this.download.InnerHtml, property);
                }

                property = GetMetadataField("/xhtml:html/xhtml:head/xhtml:meta[@class='eGMS.IPSV']/@content", nav, namespaceManager);
                if (!String.IsNullOrEmpty(property)) this.headcontent.IpsvPreferredTerms = property;

                property = GetMetadataField("/xhtml:html/xhtml:head/xhtml:meta[@name='DC.creator']/@content", nav, namespaceManager);
                if (!String.IsNullOrEmpty(property)) this.headcontent.Creator = property;

                property = GetMetadataField("/xhtml:html/xhtml:head/xhtml:meta[@name='DCTERMS.created']/@content", nav, namespaceManager);
                if (!String.IsNullOrEmpty(property)) this.headcontent.DateCreated = property;

                property = GetMetadataField("/xhtml:html/xhtml:head/xhtml:meta[@name='DCTERMS.issued']/@content", nav, namespaceManager);
                if (!String.IsNullOrEmpty(property)) this.headcontent.DateIssued = property;

                property = GetMetadataField("/xhtml:html/xhtml:head/xhtml:meta[@name='DCTERMS.modified']/@content", nav, namespaceManager);
                if (!String.IsNullOrEmpty(property)) this.headcontent.DateModified = property;

                property = GetMetadataField("/xhtml:html/xhtml:head/xhtml:meta[@name='description']/@content", nav, namespaceManager);
                if (!String.IsNullOrEmpty(property)) this.headcontent.Description = String.Format(CultureInfo.CurrentCulture, this.headcontent.Description, property);

                property = GetMetadataField("/xhtml:html/xhtml:head/xhtml:meta[@class='eGMS.LGIL']/@content", nav, namespaceManager);
                if (!String.IsNullOrEmpty(property)) this.headcontent.LgilType = property;
            }
            catch (WebException ex)
            {
                if (ex.Message == "The remote server returned an error: (310) Gone.")
                {
                    // If the page we're trying to parse has gone, this representation of it has gone too.
                    EastSussexGovUKContext.HttpStatus310Gone(this.article);
                }
                else if (ex.Message == "The remote server returned an error: (404) Not Found.")
                {
                    // If the page we're trying to parse doesn't exist, this representation of it doesn't exist either.
                    EastSussexGovUKContext.HttpStatus404NotFound(this.article);
                }
                else
                {
                    throw;
                }
            }
        }

        private string GetMetadataField(string xpath, XPathNavigator nav, XmlNamespaceManager namespaceManager)
        {
            var expr = nav.Compile(xpath);
            expr.SetContext(namespaceManager);
            var it = nav.Select(expr);

            if (it.Count == 1)
            {
                it.MoveNext();
                return it.Current.InnerXml;
            }
            return String.Empty;
        }
    }
}