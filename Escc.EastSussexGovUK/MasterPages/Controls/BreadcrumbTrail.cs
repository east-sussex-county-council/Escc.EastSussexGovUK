using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Navigation indicating position within information architecture, which appears on every page on the East Sussex County Council website.
    /// </summary>
    /// <remarks>By default the list is read from web.config.</remarks>
    /// <example>
    /// <code>
    /// &lt;configuration&gt;
    ///     &lt;configSections&gt;
    ///         &lt;sectionGroup name=&quot;EsccWebTeam.EastSussexGovUK&quot;&gt;
    ///             &lt;section name=&quot;GeneralSettings&quot; type=&quot;System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089&quot; /&gt;
    ///         &lt;/sectionGroup&gt;
    ///	    &lt;/configSections&gt;
    ///
    ///     &lt;EsccWebTeam.EastSussexGovUK&gt;
    ///         &lt;BreadcrumbTrail&gt;
    ///             &lt;add key=&quot;Home&quot; value=&quot;http://www.eastsussex.gov.uk/default.htm&quot; /&gt;
    ///             &lt;add key=&quot;Top level section&quot; value=&quot;/toplevelsection/default.htm&quot; /&gt;
    ///             &lt;add key=&quot;Sub-section&quot; value=&quot;/toplevelsection/subsection/default.htm&quot; /&gt;
    ///         &lt;/BreadcrumbTrail&gt;
    ///     &lt;/EsccWebTeam.EastSussexGovUK&gt;
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
            this.BreadcrumbProvider = BreadcrumbTrailFactory.CreateTrailProvider();
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            // Turn breadcrumb trail levels into list items
            var listItems = new List<HtmlGenericControl>();
            var breadcrumbTrail = this.BreadcrumbProvider.BuildTrail();
            var siteContext = new EastSussexGovUKContext();

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

                // screen class prevents printing in IE6. Better browsers never load the "large" stylesheet for printing so the breadcrumb's never visible in the first place.
                list.Attributes["class"] = "breadcrumb screen large";

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
            }

            // If no breadcrumb found and we're running on an internal host name, show a message. 
            // Otherwise it's an easy thing for the developer to miss.
            else if (!siteContext.IsPublicUrl)
            {
                var method = "See EsccWebTeam.EastSussexGovUK web.example.config for an example.";
                this.Controls.Add(new LiteralControl("<p><strong>You need to add a breadcrumb trail. " + method + "</strong></p>"));
            }
        }
    }
}
