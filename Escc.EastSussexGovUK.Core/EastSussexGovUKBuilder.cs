using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Escc.EastSussexGovUK.Features;
using Escc.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;

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
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(Metadata.Metadata).Assembly, "Escc.Metadata.Views"));
            });

            // Set up the global configuration service and add the configuration sections that are relevant to the template
            services.AddOptions();
            services.Configure<ConfigurationSettings>(options => configuration.GetSection("Escc.Net").Bind(options));
            services.Configure<MvcSettings>(options => configuration.GetSection("Escc.EastSussexGovUK:Mvc").Bind(options));
            services.Configure<BreadcrumbSettings>(options => configuration.GetSection("Escc.EastSussexGovUK:BreadcrumbTrail").Bind(options));
            services.Configure<WebChatApiSettings>(options => configuration.GetSection("Escc.EastSussexGovUK:WebChat").Bind(options));
            services.Configure<Metadata.Metadata>(options => configuration.GetSection("Escc.Metadata").Bind(options));

            // Register the classes that need to be injected to build up the template
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IViewSelector, MvcViewSelector>();
            services.TryAddSingleton<IBreadcrumbProvider, BreadcrumbTrailFromConfig>();
            services.TryAddSingleton<IViewModelDefaultValuesProvider, ViewModelDefaultValuesProvider>();
            services.TryAddSingleton<IProxyProvider, ProxyFromConfiguration>();
            services.TryAddSingleton<IHttpClientProvider, HttpClientProvider>();
            services.TryAddScoped<IEastSussexGovUKTemplateRequest, EastSussexGovUKTemplateRequest>();

            return services;
        }
    }
}
