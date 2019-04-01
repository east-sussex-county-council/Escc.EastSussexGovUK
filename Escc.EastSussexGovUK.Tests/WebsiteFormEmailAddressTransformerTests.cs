using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Escc.AddressAndPersonalDetails;
using Escc.EastSussexGovUK.Features;
using Xunit;

namespace Escc.EastSussexGovUK.Tests
{
    public class WebsiteFormEmailAddressTransformerTests
    {
        [Fact]
        public void Transform_bad_address_returns_null()
        {
            var email = new ContactEmail("Health%20and%20Safety%20");
            var transformer = new WebsiteFormEmailAddressTransformer(new Uri("https://example.org"));

            var result = transformer.TransformEmailAddress(email);

            Assert.Null(result);
        }

        [Fact]
        public void Transform_address_returns_form_URL_with_email_as_display_name()
        {
            var email = new ContactEmail("first.last@example.org");
            var transformer = new WebsiteFormEmailAddressTransformer(new Uri("https://example.org"));

            var result = transformer.TransformEmailAddress(email);

            Assert.Equal("https://example.org/contactus/emailus/email.aspx?n=first.last%40example.org&e=first.last&d=example.org", result);
        }
    }
}
