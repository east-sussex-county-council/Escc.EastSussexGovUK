using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Escc.EastSussexGovUK.Core.Tests
{
    public partial class BreadcrumbTrailFromConfigTests
    {
        [Fact]
        public void BreadcrumbTrailFromConfig_requires_BreadcrumbSettings()
        {
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            Assert.Throws<ArgumentNullException>(() => new BreadcrumbTrailFromConfig(null, httpContextAccessor.Object));
        }

        [Fact]
        public void BreadcrumbTrailFromConfig_requires_HttpContextAccessor()
        {
            Assert.Throws<ArgumentNullException>(() => new BreadcrumbTrailFromConfig(Options.Create(new BreadcrumbSettings()), null));
        }

        [Fact]
        public void BreadcrumbTrailFromConfig_requires_HttpContext()
        {
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            Assert.Throws<ArgumentNullException>(() => new BreadcrumbTrailFromConfig(Options.Create(new BreadcrumbSettings()), httpContextAccessor.Object));
        }

        [Fact]
        public void BreadcrumbTrail_includes_Url_for_other_pages()
        {
            var request = new DefaultHttpContext().Request;
            request.Scheme = "https";
            request.Host = new HostString("www.example.org");
            request.PathBase = new PathString("/");
            request.Path = new PathString("/example2");
            request.QueryString = new QueryString(string.Empty);
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request).Returns(request);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

            var settings = new BreadcrumbSettings {
                { new BreadcrumbLevel { Name = "level1", Url = new Uri("/", UriKind.Relative) } },
                { new BreadcrumbLevel { Name = "level2", Url = new Uri("/example", UriKind.Relative) } }
            };

            var trail = new BreadcrumbTrailFromConfig(Options.Create(settings), httpContextAccessor.Object).BuildTrail();

            Assert.Equal("/example", trail["level2"]);
        }

        [Fact]
        public void BreadcrumbTrail_removes_Url_for_current_page()
        {
            var request = new DefaultHttpContext().Request;
            request.Scheme = "https";
            request.Host = new HostString("www.example.org");
            request.PathBase = new PathString(string.Empty);
            request.Path = new PathString("/example");
            request.QueryString = new QueryString(string.Empty);
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request).Returns(request);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

            var settings = new BreadcrumbSettings {
                { new BreadcrumbLevel { Name = "level1", Url = new Uri("/", UriKind.Relative) } },
                { new BreadcrumbLevel { Name = "level2", Url = new Uri("/example", UriKind.Relative) } }
            };

            var trail = new BreadcrumbTrailFromConfig(Options.Create(settings), httpContextAccessor.Object).BuildTrail();

            Assert.Equal(string.Empty, trail["level2"]);
        }
    }
}
