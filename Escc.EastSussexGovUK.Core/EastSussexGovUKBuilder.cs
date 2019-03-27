using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Escc.EastSussexGovUK.Features;
using Escc.EastSussexGovUK.Views;
using Escc.Net;
using Escc.Redirects;
using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using PeterJuhasz.AspNetCore.Extensions.Security;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// The EastSussexGovUK template requires a number of service registrations inside the Startup class. <see cref="EastSussexGovUKBuilder"/> makes this easier to handle and more consistent.
    /// </summary>
    public static class EastSussexGovUKBuilder
    {
        /// <summary>
        /// Adds the necessary services and configuration binders for using the EastSussexGovUK template. Includes <c>services.AddOptions()</c> to register the configuration service.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns>The service collection for chaining in a fluent interface</returns>
        public static IServiceCollection AddEastSussexGovUK(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            // The views, including _ViewStart.cshtml, are included as embedded resources as that is the only way to distribute content in a NuGet package.
            // Register a provider that allows these to be used just like ordinary files. Note that it must reference a specific namespace, so it only works 
            // for files in the Views folder. Another instance would be required to include files in a different folder.
            services.Configure<RazorViewEngineOptions>(options => 
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(BaseViewModel).Assembly, "Escc.EastSussexGovUK.Core.Views"));
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(Metadata.Metadata).Assembly, "Escc.Metadata.Views.Shared"));
            });

            // Set up the global configuration service and add the configuration sections that are relevant to the template
            services.AddOptions();
            services.Configure<ConfigurationSettings>(options => configuration.GetSection("Escc.Net").Bind(options));
            services.Configure<RemoteMasterPageSettings>(options => configuration.GetSection("Escc.EastSussexGovUK:Mvc").Bind(options));
            services.Configure<MvcSettings>(options => configuration.GetSection("Escc.EastSussexGovUK:Mvc").Bind(options));
            services.Configure<BreadcrumbSettings>(options => configuration.GetSection("Escc.EastSussexGovUK:BreadcrumbTrail").Bind(options));
            services.Configure<WebChatApiSettings>(options => configuration.GetSection("Escc.EastSussexGovUK:WebChat").Bind(options));
            services.Configure<Metadata.Metadata>(options => configuration.GetSection("Escc.Metadata").Bind(options));
            services.Configure<ExceptionlessSettings>(options => configuration.GetSection("Exceptionless").Bind(options));
            services.Configure<RedirectSettings>(options => configuration.GetSection("Escc.Redirects").Bind(options));

            // Register the classes that need to be injected to build up the template
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IViewSelector, MvcViewSelector>();
            services.TryAddSingleton<IBreadcrumbProvider, BreadcrumbTrailFromConfig>();
            services.TryAddSingleton<IViewModelDefaultValuesProvider, ViewModelDefaultValuesProvider>();
            services.TryAddSingleton<IProxyProvider, ProxyFromConfiguration>();
            services.TryAddSingleton<IHttpClientProvider, HttpClientProvider>();
            services.TryAddSingleton<ICacheStrategy<WebChatSettings>, ApplicationCacheStrategy<WebChatSettings>>();
            services.TryAddSingleton<IWebChatSettingsService, WebChatSettingsFromApi>();
            services.TryAddSingleton<IRemoteMasterPageCacheProvider, RemoteMasterPageMemoryCacheProvider>();
            services.TryAddSingleton<INotFoundRequestPathResolver, NotFoundRequestPathResolver>();
            services.TryAddSingleton<IRedirectMatcher, SqlServerRedirectMatcher>();
            services.TryAddSingleton<IConvertToAbsoluteUrlHandler, ConvertToAbsoluteUrlHandler>();
            services.TryAddSingleton<IPreserveQueryStringHandler, PreserveQueryStringHandler>();
            services.TryAddScoped<IEastSussexGovUKTemplateRequest, EastSussexGovUKTemplateRequest>();
            services.TryAddScoped<IClientDependencySetEvaluator, ClientDependencySetEvaluator>();
            services.TryAddScoped<ILibraryCatalogueContext, LibraryCatalogueContext>();
            services.TryAddScoped<ITextSize, TextSize>();
            services.TryAddScoped<IHtmlControlProvider, RemoteMasterPageHtmlProvider>();

            // Ensure TLS is used
            services.AddHsts(options => {
                options.MaxAge = TimeSpan.FromDays(365);
                options.IncludeSubDomains = false;
                options.Preload = false;
                });
            services.AddHttpsRedirection(options => options.RedirectStatusCode = 301);

            return services;
        }

        /// <summary>
        /// Configures security settings for applications using the EastSussexGovUK template, including enforcing HTTPS/TLS connections.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance provided by ASP.NET Core to the Startup.Configure() method</param>
        /// <param name="environment">The <see cref="IHostingEnvironment"/> instance provided by ASP.NET Core to the Startup.Configure() method</param>
        /// <param name="cspOptions">An optional Content Security Policy for the application in addition to the default policy for the site</param>
        /// <returns></returns>
        public static IApplicationBuilder UseEastSussexGovUK(this IApplicationBuilder app, IHostingEnvironment environment, CspOptions cspOptions = null)
        {
            // Ensure TLS is used
            app.UseHttpsRedirection();
            app.UseHsts();

            // Configure error handling
            var exceptionlessSettings = app.ApplicationServices.GetRequiredService<IOptions<ExceptionlessSettings>>();
            if (exceptionlessSettings != null && exceptionlessSettings.Value != null && 
                !string.IsNullOrEmpty(exceptionlessSettings.Value.ApiKey) && exceptionlessSettings.Value.ServerUrl != null)
            {
                ExceptionlessClient.Default.Configuration.ApiKey = exceptionlessSettings.Value.ApiKey;
                ExceptionlessClient.Default.Configuration.ServerUrl = exceptionlessSettings.Value.ServerUrl.ToString();
            }

            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/HttpStatus/Status500");
                app.UseStatusCodePagesWithReExecute("/HttpStatus/Status{0}");
            }

            // Use security headers recommended by securityheaders.io
            app.UseEastSussexGovUKContentSecurityPolicy(cspOptions);
            app.UseEastSussexGovUKSecurityHeaders();

            return app;
        }

        /// <summary>
        /// Configures the Content Security Policy for applications using the EastSussexGovUK template. Call <see cref="UseEastSussexGovUK"/> instead unless you are trying to override the default behaviour.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance provided by ASP.NET Core to the Startup.Configure() method</param>
        /// <param name="cspOptions">An optional Content Security Policy for the application in addition to the default policy for the site</param>
        /// <returns></returns>
        public static IApplicationBuilder UseEastSussexGovUKContentSecurityPolicy(this IApplicationBuilder app, CspOptions cspOptions = null)
        {
            app.UseMiddleware<ContentSecurityPolicyMiddleware>(cspOptions);
            return app;
        }

        /// <summary>
        /// Configures standard security headers for applications using the EastSussexGovUK template. Call <see cref="UseEastSussexGovUK"/> instead unless you are trying to override the default behaviour.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance provided by ASP.NET Core to the Startup.Configure() method</param>
        /// <returns></returns>
        public static IApplicationBuilder UseEastSussexGovUKSecurityHeaders(this IApplicationBuilder app)
        {
            app.Use((context, next) =>
            {
                context.Response.OnStarting(() =>
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

                    return Task.CompletedTask;
                });

                return next();
            });

            return app;
        }
    }
}
