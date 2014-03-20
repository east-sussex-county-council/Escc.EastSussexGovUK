using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Related information section
    /// </summary>
    [ParseChildren(ChildrenAsProperties = true)]
    public partial class Related : System.Web.UI.UserControl
    {
        /// <summary>
        /// Gets or sets the template for the content of the "Pages" section.
        /// </summary>
        /// <value>The pages template.</value>
        [TemplateContainer(typeof(HtmlContainer))]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)] // This makes it validate in Visual Studio (allegedly)
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)] // This makes it validate in Visual Studio (allegedly)
        public ITemplate PagesTemplate { get; set; }

        /// <summary>
        /// Gets or sets the template for the content of the "Websites" section.
        /// </summary>
        /// <value>The websites template.</value>
        [TemplateContainer(typeof(HtmlContainer))]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)] // This makes it validate in Visual Studio (allegedly)
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)] // This makes it validate in Visual Studio (allegedly)
        public ITemplate WebsitesTemplate { get; set; }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            bool hasPages = (this.PagesTemplate != null);
            bool hasSites = (this.WebsitesTemplate != null);

            this.Visible = (hasPages || hasSites);
            if (hasPages && hasSites)
            {
                this.PagesTemplate.InstantiateIn(this.pagesSectionContent);
                this.WebsitesTemplate.InstantiateIn(this.sitesSectionContent);

                this.pagesOnly.Visible = false;
                this.sitesOnly.Visible = false;
            }
            else if (hasPages)
            {
                this.PagesTemplate.InstantiateIn(this.pagesOnlyContent);

                this.pagesSection.Visible = false;
                this.sitesSection.Visible = false;
                this.sitesOnly.Visible = false;
            }
            else if (hasSites)
            {
                this.WebsitesTemplate.InstantiateIn(this.sitesOnlyContent);

                this.pagesSection.Visible = false;
                this.pagesOnly.Visible = false;
                this.sitesSection.Visible = false;
            }

        }
    }

    /// <summary>
    /// Container for templated controls
    /// </summary>
    internal class HtmlContainer : PlaceHolder, INamingContainer { }
}