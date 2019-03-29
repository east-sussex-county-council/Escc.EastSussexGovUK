using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using Xunit;

namespace Escc.EastSussexGovUK.Core.Tests
{
    public class SecurityHeadersMiddlewareTests
    {
        [Fact]
        public void SecurityHeadersMiddleware_adds_ExpectCT()
        {
            var context = new DefaultHttpContext();

            SecurityHeadersMiddleware.AddHeaders(context);

            Assert.True(context.Response.Headers.TryGetValue("Expect-CT", out var someHeader));
        }

        [Fact]
        public void SecurityHeadersMiddleware_adds_XFrameOptions()
        {
            var context = new DefaultHttpContext();

            SecurityHeadersMiddleware.AddHeaders(context);

            Assert.True(context.Response.Headers.TryGetValue("X-Frame-Options", out var someHeader));
        }

        [Fact]
        public void SecurityHeadersMiddleware_adds_XXSSProtection()
        {
            var context = new DefaultHttpContext();

            SecurityHeadersMiddleware.AddHeaders(context);

            Assert.True(context.Response.Headers.TryGetValue("X-XSS-Protection", out var someHeader));
        }

        [Fact]
        public void SecurityHeadersMiddleware_adds_XContentTypeOptions()
        {
            var context = new DefaultHttpContext();

            SecurityHeadersMiddleware.AddHeaders(context);

            Assert.True(context.Response.Headers.TryGetValue("X-Content-Type-Options", out var someHeader));
        }

        [Fact]
        public void SecurityHeadersMiddleware_adds_ReferrerPolicy()
        {
            var context = new DefaultHttpContext();

            SecurityHeadersMiddleware.AddHeaders(context);

            Assert.True(context.Response.Headers.TryGetValue("Referrer-Policy", out var someHeader));
        }
    }
}
