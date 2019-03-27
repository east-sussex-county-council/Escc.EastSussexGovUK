using System;
using System.Collections.Generic;
using System.Text;
using Escc.EastSussexGovUK.Features;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Escc.EastSussexGovUK.Core.Tests
{
    public class ViewModelDefaultValuesProviderTests
    {
        [Fact]
        public void ViewModelDefaultValuesProvider_requires_HttpContextAccessor()
        {
            Assert.Throws<ArgumentNullException>(() => new ViewModelDefaultValuesProvider(
                Options.Create(new Metadata.Metadata()),
                Options.Create(new MvcSettings()),
                new Mock<IBreadcrumbProvider>().Object,
                null));
        }
    }
}
