using System;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Displays a Twitter search widget when the <see cref="Search"/> property is not empty
    /// </summary>
    public partial class TwitterSearch : System.Web.UI.UserControl
    {
        /// <summary>
        /// Gets or sets the search term.
        /// </summary>
        /// <value>The search.</value>
        public string Search { get; set; }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Search))
            {
                var searchTerm = Search.Replace("'", "\'");
                this.searchTerm1.Text = searchTerm;
                this.searchTerm2.Text = searchTerm;
                this.Visible = true;
            }
            else
            {
                this.Visible = false;
            }
        }
    }
}