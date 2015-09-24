using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EsccWebTeam.EastSussexGovUK.MasterPages;
using NUnit.Framework;
using Escc.EastSussexGovUK.MasterPages;

namespace Escc.EastSussexGovUK.Tests
{
    [TestFixture]
    class MarriageSkinTests
    {
        [Test]
        public void MarriageSkinIsAppliedToMarriageUrl()
        {
            var marriageUrl = new Uri("https://www.eastsussex.gov.uk/community/registration/registeramarriage/example.html");
            var skin = new MarriageSkin(EsccWebsiteView.Desktop, marriageUrl);

            var required = skin.IsRequired();

            Assert.AreEqual(required, true);
        }

        [Test]
        public void MarriageSkinIsNotAppliedToOtherUrls()
        {
            var otherUrl = new Uri("https://www.eastsussex.gov.uk/community/registration/registerabirth/example.html");
            var skin = new MarriageSkin(EsccWebsiteView.Desktop, otherUrl);

            var required = skin.IsRequired();

            Assert.AreEqual(required, false);
        }
    }
}
