using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// Middleware which applies standard security headers for applications using the EastSussexGovUK template.
    /// </summary>
    public class SecurityHeadersMiddleware
    {
        /// <summary>
        /// Creates a new instance of <see cref="SecurityHeadersMiddleware"/>
        /// </summary>
        /// <param name="next">The next middleware in the ASP.NET Core pipeline</param>
        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private readonly RequestDelegate _next;

        public async Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                AddHeaders(context);

                return Task.CompletedTask;
            });

            await _next.Invoke(context);
        }

        /// <remarks>
        /// Extracted to a separate method for unit testing
        /// </remarks>
        internal static void AddHeaders(HttpContext context)
        {
            // Use ExpectCT in report mode only to assess whether it is ready to be enabled
            if (!context.Response.Headers.Keys.Contains("Expect-CT"))
            {
                context.Response.Headers.Add("Expect-CT", "max-age=0, report-uri=\"https://eastsussexgovuk.report-uri.com/r/d/ct/reportOnly\"");
            }

            // Defend against clickjacking: Use SAMEORIGIN rather than DENY to allow the use of SVG images.
            // When Umbraco is in use, it requires same origin framing for preview and template editing.
            if (!context.Response.Headers.Keys.Contains("X-Frame-Options"))
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            }

            // Enable the cross-site scripting filter built into most browsers, blocking any requests detected as XSS.
            if (!context.Response.Headers.Keys.Contains("X-XSS-Protection"))
            {
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
            }

            // Force browsers to stick with the declared MIME type rather than guessing it from the content.
            if (!context.Response.Headers.Keys.Contains("X-Content-Type-Options"))
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            }

            // Allow referrer URL to be passed to sites on the same protocol, but not leaked from HTTPS to HTTP
            if (!context.Response.Headers.Keys.Contains("Referrer-Policy"))
            {
                context.Response.Headers.Add("Referrer-Policy", "no-referrer-when-downgrade");
            }
        }
    }
}
