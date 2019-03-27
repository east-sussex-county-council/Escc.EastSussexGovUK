using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Features;
using Escc.Redirects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace Escc.EastSussexGovUK.Core.Tests
{
    public class HttpStatusControllerTests
    {
        [Fact]
        public async Task HttpStatus400_loads_template()
        {
            var templateRequest = new Mock<IEastSussexGovUKTemplateRequest>();
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            defaultValues.Setup(x => x.Breadcrumb).Returns(new Mock<IBreadcrumbProvider>().Object);
            var controller = new HttpStatusController(templateRequest.Object, defaultValues.Object, null, null, null, null);

            var action = await controller.Status400();

            templateRequest.Verify(x => x.RequestTemplateHtmlAsync());
        }

        [Fact]
        public async Task HttpStatus403_loads_template()
        {
            var templateRequest = new Mock<IEastSussexGovUKTemplateRequest>();
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            defaultValues.Setup(x => x.Breadcrumb).Returns(new Mock<IBreadcrumbProvider>().Object);
            var controller = new HttpStatusController(templateRequest.Object, defaultValues.Object, null, null, null, null);

            var action = await controller.Status403();

            templateRequest.Verify(x => x.RequestTemplateHtmlAsync());
        }

        [Fact]
        public async Task HttpStatus404_loads_template()
        {
            var templateRequest = new Mock<IEastSussexGovUKTemplateRequest>();
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            defaultValues.Setup(x => x.Breadcrumb).Returns(new Mock<IBreadcrumbProvider>().Object);
            var controller = new HttpStatusController(templateRequest.Object, defaultValues.Object, null, null, null, null);

            var action = await controller.Status404();

            templateRequest.Verify(x => x.RequestTemplateHtmlAsync());
        }

        [Fact]
        public async Task HttpStatus410_loads_template()
        {
            var templateRequest = new Mock<IEastSussexGovUKTemplateRequest>();
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            defaultValues.Setup(x => x.Breadcrumb).Returns(new Mock<IBreadcrumbProvider>().Object);
            var controller = new HttpStatusController(templateRequest.Object, defaultValues.Object, null, null, null, null);

            var action = await controller.Status410();

            templateRequest.Verify(x => x.RequestTemplateHtmlAsync());
        }

        [Fact]
        public async Task HttpStatus500_loads_template()
        {
            var templateRequest = new Mock<IEastSussexGovUKTemplateRequest>();
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            defaultValues.Setup(x => x.Breadcrumb).Returns(new Mock<IBreadcrumbProvider>().Object);
            var controller = new HttpStatusController(templateRequest.Object, defaultValues.Object, null, null, null, null);

            var action = await controller.Status500();

            templateRequest.Verify(x => x.RequestTemplateHtmlAsync());
        }

        private static HttpContext CreateHttpContextFor404Page()
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
        public async Task HttpStatus404_displays_view_if_no_redirect()
        {
            var templateRequest = new Mock<IEastSussexGovUKTemplateRequest>();
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            defaultValues.Setup(x => x.Breadcrumb).Returns(new Mock<IBreadcrumbProvider>().Object);
            var redirectMatcher = new Mock<IRedirectMatcher>();
            redirectMatcher.Setup(x => x.MatchRedirect(It.IsAny<Uri>())).Returns<Redirect>(null);
            var controller = new HttpStatusController(templateRequest.Object, defaultValues.Object, null, redirectMatcher.Object, null, null);
            controller.ControllerContext.HttpContext = CreateHttpContextFor404Page();

            var actionResult = await controller.Status404();

            Assert.IsType<ViewResult>(actionResult);
        }

        [Fact]
        public async Task HttpStatus404_matching_redirect_returns_redirect_result()
        {
            var templateRequest = new Mock<IEastSussexGovUKTemplateRequest>();
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            defaultValues.Setup(x => x.Breadcrumb).Returns(new Mock<IBreadcrumbProvider>().Object);
            var redirectMatcher = new Mock<IRedirectMatcher>();
            redirectMatcher.Setup(x => x.MatchRedirect(It.IsAny<Uri>())).Returns(new Redirect() { DestinationUrl = new Uri("https://www.example.org") });
            var controller = new HttpStatusController(templateRequest.Object, defaultValues.Object, null, redirectMatcher.Object, null, null);
            controller.ControllerContext.HttpContext = CreateHttpContextFor404Page();

            var actionResult = await controller.Status404();

            Assert.IsType<StatusCodeResult>(actionResult);
        }

        [Fact]
        public async Task HttpStatus404_matching_redirect_converts_destination_to_absolute_Url()
        {
            var templateRequest = new Mock<IEastSussexGovUKTemplateRequest>();
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            defaultValues.Setup(x => x.Breadcrumb).Returns(new Mock<IBreadcrumbProvider>().Object);
            var redirect = new Redirect() { DestinationUrl = new Uri("https://www.example.org") };
            var redirectMatcher = new Mock<IRedirectMatcher>();
            redirectMatcher.Setup(x => x.MatchRedirect(It.IsAny<Uri>())).Returns(redirect);
            var destinationUrlConverter = new Mock<IConvertToAbsoluteUrlHandler>();
            var controller = new HttpStatusController(templateRequest.Object, defaultValues.Object, null, redirectMatcher.Object, destinationUrlConverter.Object, null);
            controller.ControllerContext.HttpContext = CreateHttpContextFor404Page();

            var actionResult = await controller.Status404();

            destinationUrlConverter.Verify(x => x.HandleRedirect(redirect));
        }


        [Fact]
        public async Task HttpStatus404_matching_redirect_preserves_querystring()
        {
            var templateRequest = new Mock<IEastSussexGovUKTemplateRequest>();
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            defaultValues.Setup(x => x.Breadcrumb).Returns(new Mock<IBreadcrumbProvider>().Object);
            var redirect = new Redirect() { DestinationUrl = new Uri("https://www.example.org") };
            var redirectMatcher = new Mock<IRedirectMatcher>();
            redirectMatcher.Setup(x => x.MatchRedirect(It.IsAny<Uri>())).Returns(redirect);
            var queryStringPreserver = new Mock<IPreserveQueryStringHandler>();
            var controller = new HttpStatusController(templateRequest.Object, defaultValues.Object, null, redirectMatcher.Object, null, queryStringPreserver.Object);
            controller.ControllerContext.HttpContext = CreateHttpContextFor404Page();

            var actionResult = await controller.Status404();

            queryStringPreserver.Verify(x => x.HandleRedirect(redirect));
        }


        [Fact]
        public async Task HttpStatus404_matching_redirect_sets_debug_header()
        {
            var templateRequest = new Mock<IEastSussexGovUKTemplateRequest>();
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            defaultValues.Setup(x => x.Breadcrumb).Returns(new Mock<IBreadcrumbProvider>().Object);
            var redirect = new Redirect() { RedirectId = 123, DestinationUrl = new Uri("https://www.example.org") };
            var redirectMatcher = new Mock<IRedirectMatcher>();
            redirectMatcher.Setup(x => x.MatchRedirect(It.IsAny<Uri>())).Returns(redirect);
            var controller = new HttpStatusController(templateRequest.Object, defaultValues.Object, null, redirectMatcher.Object, null, null);
            controller.ControllerContext.HttpContext = CreateHttpContextFor404Page();

            var actionResult = await controller.Status404();

            Assert.True(controller.ControllerContext.HttpContext.Response.Headers.Contains(new KeyValuePair<string, StringValues>("X-ESCC-Redirect", "123")));
        }


        [Fact]
        public async Task HttpStatus404_matching_redirect_sets_location_header()
        {
            var templateRequest = new Mock<IEastSussexGovUKTemplateRequest>();
            var defaultValues = new Mock<IViewModelDefaultValuesProvider>();
            defaultValues.Setup(x => x.Breadcrumb).Returns(new Mock<IBreadcrumbProvider>().Object);
            var redirect = new Redirect() { DestinationUrl = new Uri("https://www.example.org/") };
            var redirectMatcher = new Mock<IRedirectMatcher>();
            redirectMatcher.Setup(x => x.MatchRedirect(It.IsAny<Uri>())).Returns(redirect);
            var controller = new HttpStatusController(templateRequest.Object, defaultValues.Object, null, redirectMatcher.Object, null, null);
            controller.ControllerContext.HttpContext = CreateHttpContextFor404Page();

            var actionResult = await controller.Status404();

            Assert.True(controller.ControllerContext.HttpContext.Response.Headers.Contains(new KeyValuePair<string,StringValues>("Location", "https://www.example.org/")));
        }
    }
}
