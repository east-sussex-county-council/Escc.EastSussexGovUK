using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Escc.AddressAndPersonalDetails;
using Escc.EastSussexGovUK.Features;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Tests
{
    [TestFixture]
    public class WebsiteFormEmailAddressTransformerTests
    {
        [Test]
        public void TransformBadAddressReturnsNull()
        {
            var email = new ContactEmail("Health%20and%20Safety%20");
            var transformer = new WebsiteFormEmailAddressTransformer(new Uri("https://example.org"));

            var result = transformer.TransformEmailAddress(email);

            Assert.IsNull(result);
        }

        [Test]
        public void TransformAddressReturnsFormUrlWithEmailAsDisplayName()
        {
            var email = new ContactEmail("first.last@example.org");
            var transformer = new WebsiteFormEmailAddressTransformer(new Uri("https://example.org"));

            var result = transformer.TransformEmailAddress(email);

            Assert.AreEqual("https://example.org/contactus/emailus/email.aspx?n=first.last%40example.org&e=first.last&d=example.org", result);
        }
    }
}
