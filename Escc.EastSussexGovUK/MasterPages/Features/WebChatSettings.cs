using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Escc.EastSussexGovUK.MasterPages.Features
{
    /// <summary>
    /// Settings required to work out whether to show web chat on the specified page
    /// </summary>
    public class WebChatSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebChatSettings"/> class.
        /// </summary>
        public WebChatSettings()
        {
            WebChatUrls = new List<Uri>();
            ExcludedUrls = new List<Uri>();
        }

        /// <summary>
        /// Gets or sets the URL of the page where web 
        /// chat may apply.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        [JsonIgnore]
        public Uri PageUrl { get; set; }

        /// <summary>
        /// Gets the URLs where web chat should be shown.
        /// </summary>
        /// <value>
        /// The web chat urls.
        /// </value>
        [JsonProperty("include")]
        public IList<Uri> WebChatUrls { get; private set; }

        /// <summary>
        /// Gets the URLs where web chat should not be shown, overriding <see cref="WebChatUrls"/>.
        /// </summary>
        /// <value>
        /// The excluded urls.
        /// </value>
        [JsonProperty("exclude")]
        public IList<Uri> ExcludedUrls { get; private set; }
    }
}