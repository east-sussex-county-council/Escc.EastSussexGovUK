using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Views;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Escc.EastSussexGovUK.Core.Tests
{
    public class EastSussexGovUKTemplateRequestTests
    {
        [Fact]
        public void EastSussexGovUKTemplateRequest_requires_HttpContextAccessor()
        {
            Assert.Throws<ArgumentNullException>(() => new EastSussexGovUKTemplateRequest(
                null, 
                new Mock<IViewSelector>().Object, 
                new Mock<IHtmlControlProvider>().Object, 
                new Mock<IBreadcrumbProvider>().Object, 
                new Mock<ILibraryCatalogueContext>().Object, 
                new Mock<ITextSize>().Object, 
                new Mock<IWebChatSettingsService>().Object
                ));
        }

        [Fact]
        public void EastSussexGovUKTemplateRequest_requires_HttpContext()
        {
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            Assert.Throws<ArgumentNullException>(() => new EastSussexGovUKTemplateRequest(
                httpContextAccessor.Object,
                new Mock<IViewSelector>().Object,
                new Mock<IHtmlControlProvider>().Object,
                new Mock<IBreadcrumbProvider>().Object,
                new Mock<ILibraryCatalogueContext>().Object,
                new Mock<ITextSize>().Object,
                new Mock<IWebChatSettingsService>().Object)
                );
        }


        [Fact]
        public void EastSussexGovUKTemplateRequest_requires_Request()
        {
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new Mock<HttpContext>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

            Assert.Throws<ArgumentNullException>(() => new EastSussexGovUKTemplateRequest(
                httpContextAccessor.Object,
                new Mock<IViewSelector>().Object,
                new Mock<IHtmlControlProvider>().Object,
                new Mock<IBreadcrumbProvider>().Object,
                new Mock<ILibraryCatalogueContext>().Object,
                new Mock<ITextSize>().Object,
                new Mock<IWebChatSettingsService>().Object)
                );
        }

        private static HttpContext CreateHttpContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("www.example.org");
            httpContext.Request.PathBase = new PathString("/");
            httpContext.Request.Path = new PathString(string.Empty);
            httpContext.Request.QueryString = new QueryString(string.Empty);
            return httpContext;
        }

        [Fact]
        public void EastSussexGovUKTemplateRequest_requires_ViewSelector()
        {
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(CreateHttpContext());

            Assert.Throws<ArgumentNullException>(() => new EastSussexGovUKTemplateRequest(
                httpContextAccessor.Object,
                null,
                new Mock<IHtmlControlProvider>().Object,
                new Mock<IBreadcrumbProvider>().Object,
                new Mock<ILibraryCatalogueContext>().Object,
                new Mock<ITextSize>().Object,
                new Mock<IWebChatSettingsService>().Object)
                );
        }


        [Fact]
        public void EastSussexGovUKTemplateRequest_requires_HtmlControlProvider()
        {
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(CreateHttpContext());

            Assert.Throws<ArgumentNullException>(() => new EastSussexGovUKTemplateRequest(
                httpContextAccessor.Object,
                new Mock<IViewSelector>().Object,
                null,
                new Mock<IBreadcrumbProvider>().Object,
                new Mock<ILibraryCatalogueContext>().Object,
                new Mock<ITextSize>().Object,
                new Mock<IWebChatSettingsService>().Object)
                );
        }


        [Fact]
        public void EastSussexGovUKTemplateRequest_requires_BreadcrumbProvider()
        {
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(CreateHttpContext());

            Assert.Throws<ArgumentNullException>(() => new EastSussexGovUKTemplateRequest(
                httpContextAccessor.Object,
                new Mock<IViewSelector>().Object,
                new Mock<IHtmlControlProvider>().Object,
                null,
                new Mock<ILibraryCatalogueContext>().Object,
                new Mock<ITextSize>().Object,
                new Mock<IWebChatSettingsService>().Object)
                );
        }

        [Fact]
        public async Task EastSussexGovUKTemplateRequest_calls_IWebChatSettingsService()
        {
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(CreateHttpContext());
            var webChat = new Mock<IWebChatSettingsService>();
            webChat.Setup(x => x.ReadWebChatSettings()).Returns(Task.FromResult(new WebChatSettings()));
            var templateRequest = new EastSussexGovUKTemplateRequest(
                httpContextAccessor.Object,
                new Mock<IViewSelector>().Object,
                new Mock<IHtmlControlProvider>().Object,
                new Mock<IBreadcrumbProvider>().Object,
                new Mock<ILibraryCatalogueContext>().Object,
                new Mock<ITextSize>().Object,
                webChat.Object
                );

            await templateRequest.RequestWebChatSettingsAsync();

            webChat.Verify(x => x.ReadWebChatSettings());
        }

        [Theory]
        [InlineData("HtmlTag", EsccWebsiteView.Desktop)]
        [InlineData("MetadataDesktop", EsccWebsiteView.Desktop)]
        [InlineData("AboveHeaderDesktop", EsccWebsiteView.Desktop)]
        [InlineData("HeaderDesktop", EsccWebsiteView.Desktop)]
        [InlineData("FooterDesktop", EsccWebsiteView.Desktop)]
        [InlineData("ScriptsDesktop", EsccWebsiteView.Desktop)]
        [InlineData("HtmlTag", EsccWebsiteView.FullScreen)]
        [InlineData("MetadataFullScreen", EsccWebsiteView.FullScreen)]
        [InlineData("HeaderFullScreen", EsccWebsiteView.FullScreen)]
        [InlineData("ScriptsFullScreen", EsccWebsiteView.FullScreen)]
        public async Task EastSussexGovUKTemplateRequest_requests_control(string controlId, EsccWebsiteView view)
        {
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(CreateHttpContext());
            var viewSelector = new Mock<IViewSelector>();
            viewSelector.Setup(x => x.CurrentViewIs(It.IsAny<string>())).Returns(view);
            var htmlProvider = new Mock<IHtmlControlProvider>();
            var breadcrumbProvider = new Mock<IBreadcrumbProvider>();
            var templateRequest = new EastSussexGovUKTemplateRequest(
                httpContextAccessor.Object,
                viewSelector.Object,
                htmlProvider.Object,
                breadcrumbProvider.Object,
                null,
                null,
                null
                );

            await templateRequest.RequestTemplateHtmlAsync();

            htmlProvider.Verify(x => x.FetchHtmlForControl(It.IsAny<string>(), It.IsAny<Uri>(), controlId, breadcrumbProvider.Object, It.IsAny<int>(), It.IsAny<bool>()));
        }
    }
}
