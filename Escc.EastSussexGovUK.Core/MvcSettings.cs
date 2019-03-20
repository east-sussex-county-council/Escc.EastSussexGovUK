using System;
using System.Collections.Generic;
using System.Text;
using Escc.EastSussexGovUK.Views;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// Configuration settings for loading MVC layouts for the EastSussexGovUK template 
    /// </summary>
    public class MvcSettings : RemoteMasterPageSettings
    {
        /// <summary>
        /// Gets or sets the application-relative path to the standard view file
        /// </summary>
        public string DesktopViewPath { get; set; } = "~/_EastSussexGovUK_Desktop.cshtml";

        /// <summary>
        /// Gets or sets the application-relative path to the full screen view file
        /// </summary>
        public string FullScreenViewPath { get; set; } = "~/_EastSussexGovUK_FullScreen.cshtml";

        /// <summary>
        /// Gets or sets the application-relative path to the plain view file
        /// </summary>
        public string PlainViewPath { get; set; }= "~/_EastSussexGovUK_Plain.cshtml";

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
