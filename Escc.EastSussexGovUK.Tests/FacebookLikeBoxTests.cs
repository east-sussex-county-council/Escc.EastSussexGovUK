using System;
using EsccWebTeam.EastSussexGovUK.MasterPages;
using NUnit.Framework;
using Escc.EastSussexGovUK.MasterPages.Features;

namespace Escc.EastSussexGovUK.Tests
{
    [TestFixture]
    public class FacebookLikeBoxTests
    {
        [Test]
        public void NotShownForDoNotTrack()
        {
            var model = new SocialMediaSettings()
            {
                FacebookPageUrl = new Uri("https://www.facebook.com/somepage"),
                DoNotTrack = true
            };

            var feature = new FacebookLikeBox() {SocialMedia = model, EsccWebsiteView = EsccWebsiteView.Desktop};

            Assert.IsFalse(feature.IsRequired());
        }

        [Test]
        public void NotShownInPlainView()
        {
              var model = new SocialMediaSettings()
            {
                FacebookPageUrl = new Uri("https://www.facebook.com/somepage")
            };

            var feature = new FacebookLikeBox() {SocialMedia = model, EsccWebsiteView = EsccWebsiteView.Plain};

            Assert.IsFalse(feature.IsRequired());
        }
    }
}
