using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Mvc
{
    /// <summary>
    /// The HTML controls that are required to display a page using the www.eastsussex.gov.uk template
    /// </summary>
    public class TemplateHtml
    {
        /// <summary>
        /// The HTML tag including any XHTML namespace declarations
        /// </summary>
        public IHtmlString HtmlTag { get; set; }

        /// <summary>
        /// The standard metadata which should appear within the &lt;head /&gt; section
        /// </summary>
        public IHtmlString Metadata { get; set; }

        /// <summary>
        /// The HTML which should appear at the top of the page above the header
        /// </summary>
        public IHtmlString AboveHeader { get; set; }

        /// <summary>
        /// The sitewide page header
        /// </summary>
        public IHtmlString Header { get; set; }

        /// <summary>
        /// The sitewide page footer
        /// </summary>
        public IHtmlString Footer { get; set; }

        /// <summary>
        /// The sitewide scripts which should be loaded for the page
        /// </summary>
        public IHtmlString Scripts { get; set; }
    }
}