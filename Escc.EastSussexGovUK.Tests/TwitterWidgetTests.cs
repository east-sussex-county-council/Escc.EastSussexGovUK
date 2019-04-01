using System;
using System.Web;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Views;
using Xunit;

namespace Escc.EastSussexGovUK.Tests
{ 
    public class TwitterWidgetTests
    {
        [Fact]
        public void Twitter_not_shown_in_Plain_view()
        {
            var model = new SocialMediaSettings();

            var feature = new FacebookLikeBox() {SocialMedia = model, EsccWebsiteView = EsccWebsiteView.Plain};

            Assert.False(feature.IsRequired());
        }
    }
}
