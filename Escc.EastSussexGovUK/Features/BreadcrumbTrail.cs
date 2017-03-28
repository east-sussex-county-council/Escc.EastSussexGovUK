using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Escc.EastSussexGovUK.Features
{
    /// <summary>
    /// Navigation indicating position within information architecture, which appears on every page on the East Sussex County Council website.
    /// </summary>
    /// <remarks>By default the list is read from web.config.</remarks>
    /// <example>
    /// <code>
    /// &lt;configuration&gt;
    ///     &lt;configSections&gt;
    ///         &lt;sectionGroup name=&quot;Escc.EastSussexGovUK&quot;&gt;
    ///             &lt;section name=&quot;GeneralSettings&quot; type=&quot;System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089&quot; /&gt;
    ///         &lt;/sectionGroup&gt;
    ///	    &lt;/configSections&gt;
    ///
    ///     &lt;Escc.EastSussexGovUK&gt;
    ///         &lt;BreadcrumbTrail&gt;
    ///             &lt;add key=&quot;Home&quot; value=&quot;https://www.eastsussex.gov.uk/&quot; /&gt;
    ///             &lt;add key=&quot;Top level section&quot; value=&quot;/toplevelsection/&quot; /&gt;
    ///             &lt;add key=&quot;Sub-section&quot; value=&quot;/toplevelsection/subsection/&quot; /&gt;
    ///         &lt;/BreadcrumbTrail&gt;
    ///     &lt;/Escc.EastSussexGovUK&gt;
    /// &lt;/configuration&gt;
    /// </code>
    /// </example>
    public class BreadcrumbTrail : WebControl
    {
        /// <summary>
        /// Gets or sets the data source for the breadcrumb trail data
        /// </summary>
        public IBreadcrumbProvider BreadcrumbProvider { get; set; }

        /// <summary>
        /// Nested list indicating position within information architecture, which appears on every page on the East Sussex County Council website.
        /// </summary>
        public BreadcrumbTrail()
            : base("nav")
        {
            this.BreadcrumbProvider = new BreadcrumbTrailFromConfig();
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            // Turn breadcrumb trail levels into list items
            var listItems = new List<HtmlGenericControl>();
            var breadcrumbTrail = this.BreadcrumbProvider.BuildTrail();
            var siteContext = new HostingEnvironmentContext();

            foreach (string key in breadcrumbTrail.Keys)
            {
                var listItem = new HtmlGenericControl("li");
                if (String.IsNullOrEmpty(breadcrumbTrail[key]))
                {
                    listItem.InnerText = key;
                }
                else
                {
                    HtmlAnchor link = new HtmlAnchor();
                    link.InnerText = key;
                    link.HRef = breadcrumbTrail[key];
                    listItem.Controls.Add(link);
                }
                listItems.Add(listItem);
            }

            // add the finished list to the "home" list
            if (listItems.Count > 0)
            {
                HtmlGenericControl list = new HtmlGenericControl("ol");

                list.Attributes["class"] = "breadcrumb large";

                for (var i = 0; i < listItems.Count; i++)
                {
                    listItems[i].Attributes["class"] = (i == (listItems.Count - 1)) ? "current" : "up";
                    list.Controls.Add(listItems[i]);
                }

                this.Controls.Add(new LiteralControl("<h2 class=\"aural\">You are here</h2>"));
                for (var i = 0; i < list.Controls.Count; i++)
                {
                    var item = (list.Controls[i] as HtmlGenericControl);
                    item.Attributes["class"] = (item.Attributes["class"] + " level" + (i + 1).ToString(CultureInfo.InvariantCulture)).Trim();
                    if (item.Controls[0] is HtmlAnchor)
                    {
                        item.Controls.AddAt(0, new LiteralControl("<span class=\"aural\">Level " + (i + 1).ToString(CultureInfo.CurrentCulture) + ": </span>"));
                    }
                    else
                    {
                        item.Controls.AddAt(0, new LiteralControl("<span class=\"aural\">Current level: </span>"));
                    }
                }

                this.Controls.Add(list);

                //==================================== Structured Breadcrumbs =====================================================\\
                // Create structed breadcrumb markup for Google.
                // https://developers.google.com/search/docs/data-types/breadcrumbs

                // Json-LD script is built as a string.
                StringBuilder breadcrumbScript = new StringBuilder();
                breadcrumbScript.Append("{ \"@context\": \"http://schema.org\", \"@type\": \"BreadcrumbList\", \"itemListElement\": [");

                var position = 1;
                // The breadcrumbTrail collection contains all the data needed for position, id and name.
                foreach (var item in breadcrumbTrail)
                {
                    breadcrumbScript.Append("{\"@type\" : \"ListItem\",\"position\": " + position + " ,\"item\": {\"@id\": " + "\"" + item.Value + "\",");
                    // if the breadcrumb is not the last in the collection, keep script open.
                    if (position != breadcrumbTrail.Count)
                    {
                        breadcrumbScript.Append("\"name\": " + "\"" + item.Key + "\"}},");
                    }
                    // if the breadcrumb is the last in the collection, close the script
                    else
                    {
                        breadcrumbScript.Append("\"name\": " + "\"" + item.Key + "\"}}]}");
                    }
                    position++;
                }

                // Add the script string to the page within <script> tags of type 'application/ld+json'
                HtmlGenericControl script = new HtmlGenericControl("script");
                script.Attributes.Add("type", "application/ld+json");
                script.InnerHtml = breadcrumbScript.ToString();
                this.Controls.Add(script);
                //===================================Structured Breadcrumbs======================================================\\
            }

            // If no breadcrumb found and we're running on an internal host name, show a message. 
            // Otherwise it's an easy thing for the developer to miss.
            else if (!siteContext.IsPublicUrl)
            {
                this.Controls.Add(new LiteralControl("<p><strong>You need to add a breadcrumb trail. See the documentation for " + this.BreadcrumbProvider.GetType().FullName + ".</strong></p>"));
            }
        }
    }
}
