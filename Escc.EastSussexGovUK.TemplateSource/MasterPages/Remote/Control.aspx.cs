using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using Escc.EastSussexGovUK.Views;
using Escc.Web;

namespace Escc.EastSussexGovUK.TemplateSource.MasterPages.Remote
{
    /// <summary>
    /// Loads a usercontrol in response to a remote request made by <see cref="MasterPageControl"/>
    /// </summary>
    public partial class Control : System.Web.UI.Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["control"]))
            {
                try
                {
                    // Sanitise request
                    string controlName = Regex.Replace(Request.QueryString["control"].ToUpperInvariant(), "[^0-9A-Z]", String.Empty);

                    // Load control, or fail with 400 if it doesn't exist
                    var localControlUrl = "~/masterpages/controls/{0}.ascx";

                    // Allow override to load controls from anywhere
                    var config = ConfigurationManager.GetSection("Escc.EastSussexGovUK/GeneralSettings") as NameValueCollection;
                    if (config == null) config = ConfigurationManager.GetSection("EsccWebTeam.EastSussexGovUK/GeneralSettings") as NameValueCollection;
                    if (config != null && !String.IsNullOrEmpty(config["MasterPageControlUrl"]))
                    {
                        localControlUrl = config["MasterPageControlUrl"];
                    }

                    var usercontrol = LoadControl(String.Format(CultureInfo.InvariantCulture, localControlUrl, controlName));
                    this.placeholder.Controls.Add(usercontrol);
                }
                catch (HttpException)
                {
                    // Usercontrol doesn't exist
                    new HttpStatus().BadRequest();
                }
            }
            else
            {
                new HttpStatus().BadRequest();
            }
        }
    }
}