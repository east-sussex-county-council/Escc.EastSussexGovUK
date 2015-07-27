using System;

namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// Master page for mobile phones (especially smart phones)
    /// </summary>
    public partial class FullScreen : BaseMasterPage
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected new void Page_Load(object sender, EventArgs e)
        {
            // Run the base method as well
            base.Page_Load(sender, e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // There are two opening body tags, one with a class attribute and one without. Only show the appropriate one.
            // Do it late so that code has a chance to add controls.
            this.classyBody.Visible = (this.bodyclass.Controls.Count > 0);
            this.body.Visible = !this.classyBody.Visible;
        }
    }
}