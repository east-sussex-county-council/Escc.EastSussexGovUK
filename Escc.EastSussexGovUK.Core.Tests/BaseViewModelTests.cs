using System;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Skins;
using Moq;
using Xunit;

namespace Escc.EastSussexGovUK.Core.Tests
{
    public class BaseViewModelTests
    {
        private class FakeViewModel : BaseViewModel
        {
            public FakeViewModel(IViewModelDefaultValuesProvider defaultValues) : base(defaultValues)
            {
            }
        }

        [Fact]
        public void BaseViewModel_requires_IDefaultValuesProvider()
        {
            Assert.Throws<ArgumentNullException>(() => new FakeViewModel(null));
        }

        [Fact]
        public void BaseViewModel_requires_IBreadcrumbProvider()
        {
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            Assert.Throws<ArgumentException>(() => new FakeViewModel(defaultValues.Object));
        }

        [Fact]
        public void Skin_defaults_to_CustomerFocus()
        {
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            defaultValues.Setup(x => x.Breadcrumb).Returns(new Mock<IBreadcrumbProvider>().Object);

            var model = new FakeViewModel(defaultValues.Object);

            Assert.IsType<CustomerFocusSkin>(model.EsccWebsiteSkin);
        }

        [Theory]
        [InlineData("https://www.example.org/")]
        [InlineData("https://www.example.org/some-folder/")]
        public void ClientBaseUrl_has_trailing_slash_trimmed(string url)
        {
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            defaultValues.Setup(x => x.Breadcrumb).Returns(new Mock<IBreadcrumbProvider>().Object);
            defaultValues.Setup(x => x.ClientFileBaseUrl).Returns(new Uri(url));

            var model = new FakeViewModel(defaultValues.Object);

            Assert.Equal(url.TrimEnd('/'), model.ClientFileBaseUrl);
        }

        [Fact]
        public void BaseViewModel_instantiates_TemplateHtml()
        {
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            defaultValues.Setup(x => x.Breadcrumb).Returns(new Mock<IBreadcrumbProvider>().Object);

            var model = new FakeViewModel(defaultValues.Object);

            Assert.NotNull(model.TemplateHtml);
        }

        [Fact]
        public void BaseViewModel_instantiates_Metadata()
        {
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            defaultValues.Setup(x => x.Breadcrumb).Returns(new Mock<IBreadcrumbProvider>().Object);

            var model = new FakeViewModel(defaultValues.Object);

            Assert.NotNull(model.Metadata);
        }
    }
}
