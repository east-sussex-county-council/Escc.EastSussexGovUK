
using Microsoft.AspNetCore.Html;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// The HTML controls that are required to display a page using the EastSussexGovUK template
    /// </summary>
    public class TemplateHtml
    {
        /// <summary>
        /// The HTML tag including any XHTML namespace declarations
        /// </summary>
        public IHtmlContent HtmlTag { get; set; }

        /// <summary>
        /// The standard metadata which should appear within the &lt;head /&gt; section
        /// </summary>
        public IHtmlContent Metadata { get; set; }

        /// <summary>
        /// The HTML which should appear at the top of the page above the header
        /// </summary>
        public IHtmlContent AboveHeader { get; set; }

        /// <summary>
        /// The sitewide page header
        /// </summary>
        public IHtmlContent Header { get; set; }

        /// <summary>
        /// The sitewide page footer
        /// </summary>
        public IHtmlContent Footer { get; set; }

        /// <summary>
        /// The sitewide scripts which should be loaded for the page
        /// </summary>
        public IHtmlContent Scripts { get; set; }
    }
}