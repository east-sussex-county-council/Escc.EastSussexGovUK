using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NUnit.Framework;

namespace Escc.EastSussexGovUK.Tests
{
    [TestFixture]
    class LibraryCatalogueContextTests
    {
        [Test]
        public void LibraryCatalogueUserAgentIsDetected()
        {
            const string customisedInternetExplorer11 = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; SLCC2; .NET CLR 2.0.50727; .NET4.0C; .NET4.0E; .NET CLR 3.5.30729; .NET CLR 3.0.30729; rv:11.0) like Gecko; ESCC Libraries";
            var context = new LibraryCatalogueContext(customisedInternetExplorer11);

            var result = context.RequestIsFromLibraryCatalogueMachine();

            Assert.IsTrue(result);
        }

        [Test]
        public void OrdinaryUserAgentIsNotALibraryCatalogueMachine()
        {
            const string internetExplorer11 = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; SLCC2; .NET CLR 2.0.50727; .NET4.0C; .NET4.0E; .NET CLR 3.5.30729; .NET CLR 3.0.30729; rv:11.0) like Gecko";
            var context = new LibraryCatalogueContext(internetExplorer11);

            var result = context.RequestIsFromLibraryCatalogueMachine();

            Assert.IsFalse(result);
        }
    }
}
