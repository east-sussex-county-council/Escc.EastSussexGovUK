using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Displays a link to the parent section, or the current section home page. See <see cref="BreadcrumbTrail"/> for details.
    /// </summary>
    public class BreadcrumbTrailMobile : WebControl
    {
        /// <summary>
        /// Gets or sets the data source for the breadcrumb trail data
        /// </summary>
        public IBreadcrumbProvider BreadcrumbProvider { get; set; }
        
        /// <summary>
        /// Creates a new <see cref="BreadcrumbTrailMobile"/>
        /// </summary>
        public BreadcrumbTrailMobile()
            : base("nav")
        {
            this.BreadcrumbProvider = BreadcrumbTrailFactory.CreateTrailProvider();
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            var breadcrumbTrail = this.BreadcrumbProvider.BuildTrail();
            if (breadcrumbTrail.Count > 0)
            {
                var indexedKeys = new List<string>(breadcrumbTrail.Keys);
                var lastKey = indexedKeys[indexedKeys.Count - 1];
                if (String.IsNullOrEmpty(breadcrumbTrail[lastKey]))
                {
                    // up one channel, but > 2 so that we don't show a link back to home when there's already one in the menu
                    if (breadcrumbTrail.Count > 2)
                    {
                        var penultimateKey = indexedKeys[indexedKeys.Count - 2];
                        this.Controls.Add(new LiteralControl("<p class=\"screen small medium breadcrumb-mobile\">You are in <a href=\"" + HttpUtility.HtmlEncode(breadcrumbTrail[penultimateKey]) + "\">" + penultimateKey + "</a></p>"));
                    }
                }
                else
                {
                    // home page of channel
                    this.Controls.Add(new LiteralControl("<p class=\"screen small medium breadcrumb-mobile\">You are in <a href=\"" + HttpUtility.HtmlEncode(breadcrumbTrail[lastKey]) + "\">" + lastKey + "</a></p>"));
                }
            }

        }
    }
}