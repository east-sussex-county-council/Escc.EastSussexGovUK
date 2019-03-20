using System;

namespace Escc.EastSussexGovUK.Features
{
    /// <summary>
    /// Settings to configure a request to the web chat settings API
    /// </summary>
    public class WebChatApiSettings
    {
        /// <summary>
        /// Gets the URL where web chat settings can be found as JSON data.
        /// </summary>
        /// <value>
        /// The web chat settings URL.
        /// </value>
        public Uri WebChatSettingsUrl { get; set; }

        /// <summary>
        /// Gets the length of time, in minutes, to cache web chat settings JSON data for. Defaults to 60 minutes.
        /// </summary>
        /// <value>
        /// The length of time in minutes.
        /// </value>
        public int CacheMinutes { get; set; } = 60;
    }
}
