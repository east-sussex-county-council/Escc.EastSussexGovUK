using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using EsccWebTeam.Data.Web;

namespace EsccWebTeam.EastSussexGovUK.MasterPages
{
    /// <summary>
    /// Master page for desktop browsers
    /// </summary>
    public partial class Desktop : BaseMasterPage
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected new void Page_Load(object sender, EventArgs e)
        {
            // Apply selected text size to page
            var siteContext = new EastSussexGovUKContext();
            int baseTextSize = siteContext.TextSize;
            if (baseTextSize > 1)
            {
                // Add a space if there are other classes, then add to body tag
                this.bodyclass.Controls.Add(new LiteralControl(" size" + baseTextSize.ToString(CultureInfo.InvariantCulture)));

            }

            // Support web fonts required by the current skin
            if (Skin != null)
            {
                var fontsHtml = new StringBuilder();
                foreach (var font in Skin.RequiresGoogleFonts())
                {
                    fontsHtml.Append("<link href=\"").Append(font.FontUrl).Append("\" rel=\"stylesheet\" type=\"text/css\" />");
                }

                if (Skin.RequiresTypekitFonts().Any())
                {
                    foreach (var font in Skin.RequiresTypekitFonts())
                    {
                        fontsHtml.Append("<script src=\"").Append(font.TypekitUrl).Append("\"></script>");
                    }
                    this.Typekit.Visible = true;
                }
                this.fonts.Text = fontsHtml.ToString();
            }

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