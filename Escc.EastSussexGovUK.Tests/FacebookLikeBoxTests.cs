using System;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Views;
using Xunit;

namespace Escc.EastSussexGovUK.Tests
{
    public class FacebookLikeBoxTests
    {
        [Fact]
        public void Facebook_Page_Plugin_not_showin_in_Plain_view()
        {
              var model = new SocialMediaSettings()
            {
                FacebookPageUrl = new Uri("https://www.facebook.com/somepage")
            };

            var feature = new FacebookLikeBox() {SocialMedia = model, EsccWebsiteView = EsccWebsiteView.Plain};

            Assert.False(feature.IsRequired());
        }
    }
}
