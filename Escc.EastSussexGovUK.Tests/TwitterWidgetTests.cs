using System;
using System.Web;
using EsccWebTeam.EastSussexGovUK.MasterPages;
using NUnit.Framework;
using Escc.EastSussexGovUK.MasterPages.Features;

namespace Escc.EastSussexGovUK.Tests
{
    [TestFixture]
    public class TwitterWidgetTests
    {
        [Test]
        public void NotShownInMobileView()
        {
              var model = new SocialMediaSettings()
            {
                TwitterWidgetScript = new HtmlString("script code here")
            };

            var feature = new FacebookLikeBox() {SocialMedia = model, EsccWebsiteView = EsccWebsiteView.Mobile};

            Assert.IsFalse(feature.IsRequired());
        }
    }
}
