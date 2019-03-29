using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.ContentSecurityPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PeterJuhasz.AspNetCore.Extensions.Security;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// Middleware which applies a Content Security Policy to each text/html response, building it up from sitewide defaults, 
    /// from a policy for the application defined at startup, and from instances of <see cref="IClientDependencySet"/> registered 
    /// by individual pages in controllers and/or views.
    /// </summary>
    public class ContentSecurityPolicyMiddleware
    {
        /// <summary>
        /// Creates a new instance of <see cref="ContentSecurityPolicyMiddleware"/>
        /// </summary>
        /// <param name="next">The next middleware in the ASP.NET Core pipeline</param>
        /// <param name="environment">The <see cref="IHostingEnvironment"/> instance provided by ASP.NET Core to the Startup.Configure() method</param>
        /// <param name="cspOptions">A Content Security Policy defined at application startup to be applied in addition to sitewide defaults</param>
        public ContentSecurityPolicyMiddleware(RequestDelegate next, IHostingEnvironment environment, CspOptions cspOptions)
        {
            _next = next;
            _environment = environment;
            _cspOptionsFromStartup = cspOptions;
        }

        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _environment;
        private readonly CspOptions _cspOptionsFromStartup;

        public async Task Invoke(HttpContext context, IClientDependencySetEvaluator clientDependencySet)
        {
            context.Response.OnStarting((contentSecurityPoliciesFromPage) =>
            {
                AddHeader(context, _environment, _cspOptionsFromStartup, contentSecurityPoliciesFromPage as IList<ContentSecurityPolicyDependency>);

                return Task.CompletedTask;
            },

            // Content Security Policies from the page are passed into the Response.OnStarting method as a state object 
            clientDependencySet?.RequiredContentSecurityPolicy);

            await _next.Invoke(context);
        }

        /// <remarks>
        /// Extracted to a separate method for unit testing
        /// </remarks>
        internal static void AddHeader(HttpContext context, IHostingEnvironment environment, CspOptions cspOptionsFromStartup, IList<ContentSecurityPolicyDependency> contentSecurityPoliciesFromPage)
        {
            // Apply a Content Security Policy for the site, but allow additional directives to be supplied as an argument.
            // The code is based on https://github.com/Peter-Juhasz/aspnetcoresecurity/blob/master/src/ContentSecurityPolicy/ContentSecurityPolicyMiddleware.cs,
            // but does not set the obsolete X- versions to save bandwidth, as it's a big header, and adds the IClientDependencySet support.
            if (context.Response.GetTypedHeaders().ContentType?.MediaType.Equals("text/html", StringComparison.OrdinalIgnoreCase) ?? false)
            {
                // Start by combining the sitewide Content Security Policy with the Content Security Policy defined at application startup
                var cspToApply = cspOptionsFromStartup ?? new CspOptions();
                cspToApply = cspToApply.AddEastSussexGovUKDefaultPolicy().AddGoogleFonts().AddGoogleAnalytics().AddCrazyEgg();

                // If additional Content Security Policies are registered on the page, it will be in the form of aliases represented by a 
                // ContentSecurityPolicyDependency. This representation is inherited from the .NET Framework implementation where standard 
                // Content Security Policies were defined in config. In .NET Core they are defined as extension methods, so use reflection
                // to turn the alias into a method we can run to update the policy.
                var policiesFromPage = contentSecurityPoliciesFromPage as IList<ContentSecurityPolicyDependency>;
                if (policiesFromPage != null)
                {
                    var cspExtensionMethods = typeof(ContentSecurityPolicyExtensions);
                    foreach (var policy in policiesFromPage)
                    {
                        var updatePolicyMethod = cspExtensionMethods.GetMethod($"Add{policy.Alias}", (BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase));
                        if (updatePolicyMethod != null)
                        {
                            cspToApply = (CspOptions)updatePolicyMethod.Invoke(null, new[] { cspToApply });
                        }
                    }
                }

                if (environment.IsDevelopment())
                {
                    // allow any resources from localhost in development
                    cspToApply = cspToApply.AddLocalhost();

                    // allow inline styles and scripts for developer exception page
                    if (context.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
                    {
                        var developerOptions = cspToApply.Clone();
                        developerOptions.StyleSrc = developerOptions.StyleSrc.AddUnsafeInline();
                        developerOptions.ScriptSrc = developerOptions.ScriptSrc.AddUnsafeInline();
                        cspToApply = developerOptions;
                    }
                }

                context.Response.Headers["Content-Security-Policy"] = cspToApply.ToString();
            }
        }
    }
}
