using System;
using System.Globalization;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Xml.XPath;
using EsccWebTeam.Data.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Load adverts from an XML file
    /// </summary>
    public partial class Adverts : System.Web.UI.UserControl
    {
        EastSussexGovUKContext siteContext = new EastSussexGovUKContext();

        /// <summary>
        /// Displays the adverts relevant to the current folder.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check that the advert placeholder is there. If not, a page has overriden the placeholder.
            if (this.advertLinks == null) return;

            // Load settings into Xml document
            var advertFile = Server.MapPath("~/masterpages/adverts.xml");
            if (!File.Exists(advertFile)) return;
            var advertDocument = new XPathDocument(advertFile);
            var advertNavigator = advertDocument.CreateNavigator();

            // Get URL to base search on
            var paths = Iri.ListFilesAndFoldersInPath(this.siteContext.RequestUrl);

            foreach (string path in paths)
            {
                // Look for a node relating to each folder of the URL, starting with the most specific.
                // Have to start with the most specific to allow overriding deeper in the hierarchy.
                var nodeList = advertNavigator.Select("/adverts/add[@url='" + path.ToLower(CultureInfo.CurrentCulture) + "']");
                if (nodeList.Count > 0)
                {
                    // Add each advert that matches the path
                    while (nodeList.MoveNext())
                    {
                        // make sure it's got all its data
                        string imageUrl = String.Empty;
                        string navigateUrl = String.Empty;
                        string description = String.Empty;

                        if (nodeList.Current.MoveToAttribute("imageUrl", String.Empty))
                        {
                            imageUrl = nodeList.Current.Value;
                            nodeList.Current.MoveToParent();
                        }

                        if (nodeList.Current.MoveToAttribute("navigateUrl", String.Empty))
                        {
                            navigateUrl = nodeList.Current.Value;
                            nodeList.Current.MoveToParent();
                        }

                        if (nodeList.Current.MoveToAttribute("description", String.Empty))
                        {
                            description = nodeList.Current.Value;
                            nodeList.Current.MoveToParent();
                        }

                        if (!String.IsNullOrEmpty(imageUrl) && !String.IsNullOrEmpty(navigateUrl) && !String.IsNullOrEmpty(description))
                        {
                            // Add link
                            HtmlAnchor link = new HtmlAnchor();
                            link.Attributes["class"] = "supporting advert";
                            if (navigateUrl.StartsWith("/"))
                            {
                                var linkPrefix = (this.Context.Request.Url.Scheme == Uri.UriSchemeHttps) ? Uri.UriSchemeHttp + "://" + this.Context.Request.Url.Host : "";
                                link.HRef = linkPrefix + navigateUrl;

                                // For people that can edit the site, changing the link triggers the warning that there's an "unpublished" link on the page,
                                // from Console.js in EsccWebTEam.Cms.WebAuthor project, so include an attribute which JavaScript can look for to know that the link is OK.
                                link.Attributes["data-unpublished"] = "false";
                            }
                            else
                            {
                                link.HRef = navigateUrl;
                            }
                            this.advertLinks.Controls.Add(link);

                            // Add image to link
                            HtmlImage image = new HtmlImage();
                            image.Alt = description;
                            image.Src = imageUrl;
                            link.Controls.Add(image);
                        }
                    }

                    // And stop looking further up the tree
                    break;
                }

            }
        }
    }
}