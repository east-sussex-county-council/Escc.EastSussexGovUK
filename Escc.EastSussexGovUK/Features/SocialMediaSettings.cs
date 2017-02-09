using System;
using System.Collections.Generic;
using System.Web;

namespace Escc.EastSussexGovUK.Features
{
    /// <summary>
    /// Settings for which social media to display on a page and how it should be configured
    /// </summary>
    public class SocialMediaSettings
    {
        /// <summary>
        /// Gets or sets the facebook page URL.
        /// </summary>
        /// <value>
        /// The facebook page URL.
        /// </value>
        public Uri FacebookPageUrl { get; set; }

        /// <summary>
        /// Gets or sets whether to show faces when using the Facebook Page Plugin.
        /// </summary>
        /// <value>
        ///   <c>true</c> to show faces; otherwise, <c>false</c>.
        /// </value>
        public bool FacebookShowFaces { get; set; }

        /// <summary>
        /// Gets or sets whether to show recent statuses when using the Facebook Page Plugin.
        /// </summary>
        /// <value>
        ///   <c>true</c> to show statuses; otherwise, <c>false</c>.
        /// </value>
        public bool FacebookShowFeed { get; set; }
        
        /// <summary>
        /// Gets or sets the Twitter account, not including the @ symbol.
        /// </summary>
        public string TwitterAccount { get; set; }

        /// <summary>
        /// Gets or sets a Twitter widget script.
        /// </summary>
        public IHtmlString TwitterWidgetScript { get; set; }

        /// <summary>
        /// Gets or sets the social media order, based on an ordered sequence of social media network names, eg "Facebook" and "Twitter".
        /// </summary>
        public IEnumerable<string> SocialMediaOrder { get; set; }
    }
}