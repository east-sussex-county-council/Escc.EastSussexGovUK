using System;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Views;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Tests
{
    [TestFixture]
    public class FacebookLikeBoxTests
    {
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
