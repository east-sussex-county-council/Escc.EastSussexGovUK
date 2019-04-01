﻿using System;
using Escc.EastSussexGovUK.Features;
using Xunit;

namespace Escc.EastSussexGovUK.Tests
{
    public class EmbeddedGoogleMapsTests
    {
        [Fact]
        public void Google_MyMaps_URL_is_recognised()
        {
            const string html = "<a href=\"https://maps.google.co.uk/maps/ms?msid=213884658669219615993.00050357ac950f9508213&amp;msa=0&amp;ll=50.93301,0.796337&amp;spn=0.025343,0.066047\">Google map</a>";
            
            var feature = new EmbeddedGoogleMaps() {Html = new []{html}};

            Assert.True(feature.IsRequired());
        }
    }
}
