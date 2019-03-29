using System;
using System.Collections.Generic;
using System.Text;
using Escc.EastSussexGovUK.ContentSecurityPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using PeterJuhasz.AspNetCore.Extensions.Security;
using Xunit;

namespace Escc.EastSussexGovUK.Core.Tests
{
    public class ContentSecurityPolicyMiddlewareTests
    {
        [Fact]
        public void ContentSecurityPolicyMiddleware_adds_header_to_HTML_response()
        {
            var context = new DefaultHttpContext();
            context.Response.ContentType = "text/html";
            var environment = new Mock<IHostingEnvironment>();

            ContentSecurityPolicyMiddleware.AddHeader(context, environment.Object, new CspOptions(), new List<ContentSecurityPolicyDependency>());

            Assert.True(context.Response.Headers.TryGetValue("Content-Security-Policy", out var someHeader));
        }

        [Fact]
        public void ContentSecurityPolicyMiddleware_does_not_add_header_to_text_response()
        {
            var context = new DefaultHttpContext();
            context.Response.ContentType = "text/plain";
            var environment = new Mock<IHostingEnvironment>();

            ContentSecurityPolicyMiddleware.AddHeader(context, environment.Object, new CspOptions(), new List<ContentSecurityPolicyDependency>());

            Assert.False(context.Response.Headers.TryGetValue("Content-Security-Policy", out var someHeader));
        }

        [Fact]
        public void ContentSecurityPolicyMiddleware_adds_EastSussexGovUK_defaults()
        {
            var context = new DefaultHttpContext();
            context.Response.ContentType = "text/html";
            var environment = new Mock<IHostingEnvironment>();

            ContentSecurityPolicyMiddleware.AddHeader(context, environment.Object, new CspOptions(), new List<ContentSecurityPolicyDependency>());

            Assert.Contains("https://www.eastsussex.gov.uk", context.Response.Headers["Content-Security-Policy"].ToString());
        }

        [Fact]
        public void ContentSecurityPolicyMiddleware_adds_GoogleFonts()
        {
            var context = new DefaultHttpContext();
            context.Response.ContentType = "text/html";
            var environment = new Mock<IHostingEnvironment>();

            ContentSecurityPolicyMiddleware.AddHeader(context, environment.Object, new CspOptions(), new List<ContentSecurityPolicyDependency>());

            Assert.Contains("https://fonts.gstatic.com", context.Response.Headers["Content-Security-Policy"].ToString());
        }

        [Fact]
        public void ContentSecurityPolicyMiddleware_adds_GoogleAnalytics()
        {
            var context = new DefaultHttpContext();
            context.Response.ContentType = "text/html";
            var environment = new Mock<IHostingEnvironment>();

            ContentSecurityPolicyMiddleware.AddHeader(context, environment.Object, new CspOptions(), new List<ContentSecurityPolicyDependency>());

            Assert.Contains("https://www.google-analytics.com", context.Response.Headers["Content-Security-Policy"].ToString());
        }

        [Fact]
        public void ContentSecurityPolicyMiddleware_adds_CrazyEgg()
        {
            var context = new DefaultHttpContext();
            context.Response.ContentType = "text/html";
            var environment = new Mock<IHostingEnvironment>();

            ContentSecurityPolicyMiddleware.AddHeader(context, environment.Object, new CspOptions(), new List<ContentSecurityPolicyDependency>());

            Assert.Contains("https://*.crazyegg.com", context.Response.Headers["Content-Security-Policy"].ToString());
        }


        [Fact]
        public void ContentSecurityPolicyMiddleware_adds_policy_from_startup()
        {
            var context = new DefaultHttpContext();
            context.Response.ContentType = "text/html";
            var environment = new Mock<IHostingEnvironment>();
            var policyFromStartup = new CspOptions().AddYouTube();

            ContentSecurityPolicyMiddleware.AddHeader(context, environment.Object, policyFromStartup, new List<ContentSecurityPolicyDependency>());

            Assert.Contains("https://www.youtube-nocookie.com", context.Response.Headers["Content-Security-Policy"].ToString());
        }

        [Fact]
        public void ContentSecurityPolicyMiddleware_adds_policy_from_page()
        {
            var context = new DefaultHttpContext();
            context.Response.ContentType = "text/html";
            var environment = new Mock<IHostingEnvironment>();
            var policyFromPage = new List<ContentSecurityPolicyDependency>
            {
                new ContentSecurityPolicyDependency
                {
                    Alias = "YouTube"
                }
            };

            ContentSecurityPolicyMiddleware.AddHeader(context, environment.Object, new CspOptions(), policyFromPage);

            Assert.Contains("https://www.youtube-nocookie.com", context.Response.Headers["Content-Security-Policy"].ToString());
        }

        [Fact]
        public void ContentSecurityPolicyMiddleware_adds_localhost_in_dev()
        {
            var context = new DefaultHttpContext();
            context.Response.ContentType = "text/html";
            var environment = new Mock<IHostingEnvironment>();
            environment.Setup(x => x.EnvironmentName).Returns(EnvironmentName.Development);

            ContentSecurityPolicyMiddleware.AddHeader(context, environment.Object, new CspOptions(), new List<ContentSecurityPolicyDependency>());

            Assert.Contains("https://localhost", context.Response.Headers["Content-Security-Policy"].ToString());
        }


        [Fact]
        public void ContentSecurityPolicyMiddleware_excludes_localhost_in_production()
        {
            var context = new DefaultHttpContext();
            context.Response.ContentType = "text/html";
            var environment = new Mock<IHostingEnvironment>();
            environment.Setup(x => x.EnvironmentName).Returns(EnvironmentName.Production);

            ContentSecurityPolicyMiddleware.AddHeader(context, environment.Object, new CspOptions(), new List<ContentSecurityPolicyDependency>());

            Assert.DoesNotContain("https://localhost", context.Response.Headers["Content-Security-Policy"].ToString());
        }
    }
}
