using System;
using System.Collections.Generic;
using System.Text;

namespace Escc.EastSussexGovUK.Views
{
    /// <summary>
    /// Configuration settings for loading MVC layouts for the EastSussexGovUK template and their component parts 
    /// </summary>
    public class RemoteMasterPageSettings
    {
        /// <summary>
        /// Gets or sets the URL to request component parts of the layout views from, where {0} is substituted for a token identifying the individual view
        /// </summary>
        public Uri PartialViewUrl { get; set; } = new Uri("/masterpages/remote/control.aspx?control={0}", UriKind.Relative);

        /// <summary>
        /// Gets or sets the number of minutes HTML requested from <c>PartialViewUrl</c> may be cached for. Defaults to 60 minutes.
        /// </summary>
        public int CacheMinutes { get; set; } = 60;
    }
}
