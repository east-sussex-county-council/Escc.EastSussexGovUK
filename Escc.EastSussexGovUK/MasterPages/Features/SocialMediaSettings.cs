﻿using System;
using System.Collections.Generic;
using System.Web;

namespace Escc.EastSussexGovUK.MasterPages.Features
{
    public class SocialMediaSettings
    {
        public Uri FacebookPageUrl { get; set; }
        public bool FacebookShowFaces { get; set; }
        public bool FacebookShowFeed { get; set; }
        public string TwitterAccount { get; set; }
        public IHtmlString TwitterWidgetScript { get; set; }
        [Obsolete("The Do Not Track standard is not recognised by the ad industry")]
        public bool DoNotTrack { get; set; }

        public IEnumerable<string> SocialMediaOrder { get; set; }
    }
}