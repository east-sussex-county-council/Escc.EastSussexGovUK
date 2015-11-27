using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using EsccWebTeam.EastSussexGovUK;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Tests
{
    [TestFixture]
    public class GoogleTagManagerIdTests
    {
        [TestCase("subdomain.eastsussex.gov.uk", "production")]
        [TestCase("azurewebsitename-deploymentslot.azurewebsites.net", "azurewebsite")]
        [TestCase("azurewebsitename.azurewebsites.net", "azurewebsite")]
        [TestCase("otherazurewebsite.azurewebsites.net", "test")]
        [TestCase("localhost", "test")]
        public void CorrectIdIsSelected(string host, string expectedResult)
        {
            var rules = new NameValueCollection
            {
                {@"\.eastsussex\.gov\.uk$", "production"},
                { @"azurewebsitename[-a-z0-9]*\.azurewebsites.net", "azurewebsite"},
                { @".*", "test"}
            };

            var tagManager = new GoogleTagManagerContainerIdSelector();
            var result = tagManager.SelectContainerId(host, rules);

            Assert.AreEqual(result,expectedResult);
        }
    }
}
