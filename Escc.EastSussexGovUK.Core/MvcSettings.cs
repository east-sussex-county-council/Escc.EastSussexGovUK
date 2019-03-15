using System;
using System.Collections.Generic;
using System.Text;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// Configuration settings for loading MVC layouts for the EastSussexGovUK template and their component parts 
    /// </summary>
    public class MvcSettings
    {
        /// <summary>
        /// Gets or sets the application-relative path to the standard layout view
        /// </summary>
        public string DesktopMvcLayout { get; set; } = "~/_EastSussexGovUK_Desktop.cshtml";

        /// <summary>
        /// Gets or sets the application-relative path to the full screen layout view
        /// </summary>
        public string FullScreenMvcLayout { get; set; } = "~/_EastSussexGovUK_FullScreen.cshtml";

        /// <summary>
        /// Gets or sets the application-relative path to the plain layout view
        /// </summary>
        public string PlainMvcLayout { get; set; } = "~/_EastSussexGovUK_Plain.cshtml";

        /// <summary>
        /// Gets or sets the URL to request component parts of the layout views from, where {0} is substituted for a token identifying the individual view
        /// </summary>
        public Uri PartialViewUrl { get; set; } = new Uri("/masterpages/remote/control.aspx?control={0}", UriKind.Relative);

        /// <summary>
        /// Gets or sets the number of minutes HTML requested from <c>PartialViewUrl</c> may be cached for. Defaults to 60 minutes.
        /// </summary>
        public int CacheMinutes { get; set; } = 60;

        /// <summary>
        /// Gets or sets the base URL to use for sitewide client-side files such as CSS and JavaScript that are not part of the current application
        /// </summary>
        public Uri ClientFileBaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the version string to append to client-side files such as CSS and JavaScript to ensure that previously cached versions are not returned
        /// </summary>
        public string ClientFileVersion { get; set; }
    }
}
