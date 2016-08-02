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
        public void NotShownInPlainView()
        {
              var model = new SocialMediaSettings()
            {
                TwitterWidgetScript = new HtmlString("script code here")
            };

            var feature = new FacebookLikeBox() {SocialMedia = model, EsccWebsiteView = EsccWebsiteView.Plain};

            Assert.IsFalse(feature.IsRequired());
        }
    }
}
