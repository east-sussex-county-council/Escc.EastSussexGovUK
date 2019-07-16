# Applying the sitewide design with ASP.NET Core MVC

The ASP.NET Core version of the template is the current recommended version for new applications on the website.

## Getting started

For an ASP.NET Core MVC project, you can install our design using the following steps:

1. In Visual Studio create a new ASP.NET Core Web Application using the "Empty" project template. 
2. Install `Escc.EastSussexGovUK.Core` from our private NuGet feed.
3. Add the services required for the template to the `Startup` class in the order shown below:

		using Escc.EastSussexGovUK.Core;
		using Microsoft.AspNetCore.Builder;
		using Microsoft.AspNetCore.Hosting;
		using Microsoft.AspNetCore.Mvc;
		using Microsoft.Extensions.Configuration;
		using Microsoft.Extensions.DependencyInjection;

	    public class Startup
	    {
			public Startup(IConfiguration configuration)
        	{
            	Configuration = configuration;
        	}

        	public IConfiguration Configuration { get; }
	
	        public void ConfigureServices(IServiceCollection services)
	        {
	            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
	            services.AddEastSussexGovUK(Configuration);
	        }

			...

			public void Configure(IApplicationBuilder app, IHostingEnvironment env)
			{
			 	app.UseEastSussexGovUK(env);
			    app.UseStaticFiles();
			    app.UseMvc();
			}
		}

4. Create a view model which inherits from `Escc.EastSussexGovUK.Core.BaseViewModel`. Its constructor must inject an instance of `IViewModelDefaultValuesProvider` since this is required to build the template.

		using Escc.EastSussexGovUK.Core;

	    public class MyCustomModel : BaseViewModel
	    {
	        public MyCustomModel(IViewModelDefaultValuesProvider defaultValues): base(defaultValues) { }
	    }

5. Add a controller. The constructor must inject the resources required for the website template. Any controller action which results in a view using the website template must be marked with the `async` keyword and return `Task<IActionResult>`. It must also load the template HTML, and usually web chat too, using the following code. You can put this in a base controller class if you have many controllers in your application.

	Add one or more routes, either using attribute routing as shown here or as part of the call to `app.UseMvc();` in the `Startup` class. 
		
		using System;
		using System.Threading.Tasks;
		using Escc.EastSussexGovUK.Core;
		using Escc.EastSussexGovUK.Features;
		using Exceptionless;
		using Microsoft.AspNetCore.Mvc;

		public class ExampleController : Controller
		{
	        private readonly IEastSussexGovUKTemplateRequest _templateRequest;
     	    private readonly IViewModelDefaultValuesProvider _defaultModelValues;
	
	        public ExampleController(IEastSussexGovUKTemplateRequest templateRequest, IViewModelDefaultValuesProvider defaultModelValues)
	        {
	            _templateRequest = templateRequest;
	            _defaultModelValues = defaultModelValues;
	        }

			[Route("")]
			public async Task<IActionResult> Index()
			{
				var model = new MyCustomModel(_defaultModelValues); // inheriting from BaseViewModel

				try
                {
                    // Do this if you want your page to support web chat. It should, unless you have a reason not to.
                    model.WebChat = await _templateRequest.RequestWebChatSettingsAsync();
                }
                catch (Exception ex)
                {
                    // Catch and report exceptions - don't throw them and cause the page to fail
                    ex.ToExceptionless().Submit();
                }
                try
                {
                    // Do this to load the template controls.
                    model.TemplateHtml = await _templateRequest.RequestTemplateHtmlAsync();
                }
                catch (Exception ex)
                {
                    // Catch and report exceptions - don't throw them and cause the page to fail
                    ex.ToExceptionless().Submit();
                }

				return View(model);
			}
		}

6. Add a `~/Views/_ViewImports.cshtml` file with the following contents. This makes tag helpers in your views work.

		@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers

	You may want to add `@using Escc.MyApplication` in here too, where `Escc.MyApplication` is the default namespace of your application.
 
7. Add a view. The main content of your page should usually use the standard text styles. See the [website style guide](https://github.com/east-sussex-county-council/Escc.WebsiteStyleGuide) for a full guide.

		<div class="full-page">
			<div class="text-content content">
			    <h1>Example page</h1>
			    <p>Page text goes here.</p>
			</div>
		</div>

	In addition there are a number of sections you can override to add your own content at specific places within the template.

	Add content in the `<head>` – useful for metadata and stylesheets:

		@section Metadata {
			<meta name="robots" content="noindex, nofollow" />
			<link rel="stylesheet" href="~/css/my-styles.css?v=@Model.ClientFileVersion" />
		}

	Replace the site header:

		@section Header {
			<header>My custom header</header>
		}

	Replace the breadcrumb trail:

		@section Breadcrumb {
			<p>My custom breadcrumb</p>
		}

	Replace the site footer:

		@section Footer {
			<footer>My custom footer</footer>
		}

	Include JavaScript at the end of the page (or use the `async` or `defer` attributes when loading it earlier):

		@section JavaScript {
			<script src="~/js/my-script.js?v=@Model.ClientFileVersion"></script>
		}

8. Add configuration for the template elements and exception handling. For example, if you are using `appsettings.json` for configuration:

   		{
	  	  "Escc.EastSussexGovUK": {
		    "BreadcrumbTrail": [
		      { "Name": "Home", "Url": "https://www.eastsussex.gov.uk/" },
		      { "Name": "Example section", "Url": "https://www.eastsussex.gov.uk/example/" }
		    ],
		    "Mvc": {
		      "PartialViewUrl": "https://www.eastsussex.gov.uk/masterpages/remote/control.aspx?control={0}"
		    },
		    "WebChat": {
		      "WebChatSettingsUrl": "https://www.eastsussex.gov.uk/umbraco/api/WebChat/GetWebChatUrls"
		    }
		  },
		  "Exceptionless": {
		    "ApiKey": "API_KEY_HERE",
		    "ServerUrl": "https://hostname/"
		  }
		}

	If your application runs behind a proxy server you can configure the proxy URL and authentication details in using the settings documented in the [Escc.Net](https://github.com/east-sussex-county-council/Escc.Net) project.

9. Run the project.

	If you are using Visual Studio and targeting IIS for development, you'll need to update `~/Properties/launchSettings.json` which is configured for IIS Express by default. A minimal `launchSettings.json` for IIS looks like this:

		{
		  "iisSettings": {
		    "windowsAuthentication": false,
		    "anonymousAuthentication": true,
		    "iis": {
		      "applicationUrl": "https://localhost"
		    }
		  },
		  "profiles": {
		    "IIS": {
		      "commandName": "IIS",
		      "launchBrowser": true,
		      "environmentVariables": {
		        "ASPNETCORE_ENVIRONMENT": "Development"
		      },
		      "ancmHostingModel": "OutOfProcess"
		    }
		  }
		}

	You can change `https://localhost` to any site or application URL in IIS. You can also change most of these settings in `Project Properties > Debug`.

	IIS is set to run in `OutOfProcess` mode, meaning it acts as a reverse proxy to the built-in web server, Kestrel. Changing this to `InProcess` should be more efficient but, at the time of writing, in development the `ASPNETCORE_ENVIRONMENT` setting automatically has `Production;` prepended, which doesn’t pass the `IHostingEnvironment.IsDevelopment()` test. Use `InProcess` when deploying to production.

10.  Publish the project. Before you publish you need to add a `<MvcRazorExcludeRefAssembliesFromPublish />` line to the `.csproj` file, which is required because the EastSussexGovUK template includes embedded views. This causes a `refs` folder to be included in the publish. Without this you will see the error "`Cannot find compilation library location for package [package name]`".

        <Project Sdk="Microsoft.NET.Sdk.Web">
          <PropertyGroup>
            <TargetFramework>netcoreapp2.2</TargetFramework>
            <AspNetCoreHostingModel>InProcess</AspNetCoreHostingModel>
            <MvcRazorExcludeRefAssembliesFromPublish>false</MvcRazorExcludeRefAssembliesFromPublish>
          </PropertyGroup>
          ...
        </Project>

## How it works

### Startup

The call to `services.AddEastSussexGovUK(Configuration)` sets up:

*  [EmbeddedFileProvider](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.fileproviders.embeddedfileprovider?view=aspnetcore-2.2)s so that views included in NuGet packages can be used (it's the only was to include files in a NuGet package built using .NET Core)
*  dependency injection for the services used by the template, including calling `services.AddOptions()` to enable configuration
*  configuration for enforcing TLS using redirection to HTTPS and [HSTS](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Strict-Transport-Security). 

The call to `app.UseEastSussexGovUK(env)` sets up:

*  enables the TLS support configured earlier
*  branded error pages
*  the Content Security Policy (which can be customised - see [Varying the Content Security Policy](#csp))
*  other headers which make the site more secure

### Loading the template HTML

The call to `await templateRequest.RequestTemplateHtmlAsync()` attempts to download the template controls from a remote URL set by the `Escc.EastSussexGovUK:Mvc:PartialViewUrl` configuration setting. The template code will try to fetch HTML from that URL, passing a token for the control it wants instead of `{0}`. It then populates a `TemplateHtml` property on the model. The use of an asynchronous call is very important as it allows the site to scale successfully using this approach.

The fetched HTML is saved in a local cache so that it is not requested remotely every time. The cache expires after an hour by default, but this can be changed using the optional `CacheMinutes` setting:

   		{
	  	  "Escc.EastSussexGovUK": {
		    "Mvc": {
		      "CacheMinutes": 60
		    }
		  }
		}

There is a similar cache for the web chat settings, which also defaults to an hour and can be changed in configuration:

   		{
	  	  "Escc.EastSussexGovUK": {
		    "WebChat": {
		      "CacheMinutes": 60
		    }
		  }
		}

Requesting any page with the querystring `?ForceCacheRefresh=1` will cause the cached template to be updated even if the cache has not expired.

## Varying the design

### Loading CSS and JavaScript

Using HTTP2 there is no longer a need to bundle client-side files, so they should be included in the page individually. However it is still useful to include a version number so that the latest version is always loaded after each deployment.

	@section Metadata {
		<link rel="stylesheet" href="~/css/my-styles.css?v=@Model.ClientFileVersion" />
		<script src="~/js/my-script.js?v=@Model.ClientFileVersion" async="async"></script>
	}

It is also still useful to minify files, and this can be done using [YUI Compressor](https://www.nuget.org/packages/YUICompressor.NET.MSBuild). See the `YUICompressor.msbuild` file in `Escc.EastSussexGovUK.TemplateSource` for an example of how to configure this. The process is triggered by a post-build action in the properties for that project.

Some CSS and JavaScript is used sitewide, managed in the `Escc.EastSussexGovUK.TemplateSource` project. To use this you will usually want a different URL in development and production. While ASP.NET Core has tag helpers to do this, they still require the base URL to be in the view where it may be a problem if it's different for each developer. A better solution is to specify the base URL in configuration and use it as shown below.

	@section Metadata {
		<link rel="stylesheet" href="@Model.ClientFileBaseUrl/css/forms-small.css?v=@Model.ClientFileVersion" />
	}

If you are using `appsettings.json` for configuration:

	{
	  "Escc.EastSussexGovUK": {
	    "Mvc": {
	      "ClientFileBaseUrl": "https://www.eastsussex.gov.uk/escc.eastsussexgovuk",
	      "ClientFileVersion": "1234"
	    }
	  }
	}

Often you will want to use the standard media queries for our site, and these can easily be accessed from the view model:

	@section Metadata {
		<link rel="stylesheet" href="~/css/my-styles-small.css?v=@Model.ClientFileVersion" />
		<link rel="stylesheet" href="~/css/my-styles-medium.css?v=@Model.ClientFileVersion" media="@Model.MediaQueryMedium" />
		<link rel="stylesheet" href="~/css/my-styles-large.css?v=@Model.ClientFileVersion" media="@Model.MediaQueryLarge" />
	}

### Swapping master pages and layouts

Pages on our site can have different designs applied. By default we use a responsive design ("desktop"), but there is also a full-width, minimal design ("fullscreen") which is suitable for applications like maps, and a "plain" design which can be used as an API to request just the content of a page. You can make dramatic changes to the layout of the site by updating the configuration to point to a different layout:

	{
  	  "Escc.EastSussexGovUK": {
	    "Mvc": {
	      "DesktopViewPath": "~/Views/_My_Custom_Desktop.cshtml",
	      "FullScreenViewPath": "~/Views/_My_Custom_FullScreen.cshtml",
	      "PlainViewPath": "~/Views/_My_Custom_Plain.cshtml"
	    }
	  }
	}

`MvcViewSelector` is called (assuming it is injected as `IViewSelector`) in the constructor of `EastSussexGovUKTemplateRequest` and in the `~/Views/_ViewStart.cshtml` file. It selects the layout based on a number of criteria, including the configuration settings mentioned above.

In an MVC application you can't see the `ViewStart.cshtml` file or any of the layouts because they're implemented as embedded resources.  You can create your own `ViewStart.cshtml` if you need to add additional code, but you'll need to copy [the call to MvcViewSelector](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/blob/master/Escc.EastSussexGovUK.Core/Views/_ViewStart.cshtml) into your new file to continue to apply the correct layout.

If you want to override the layout view for a specific page in an application, you can set the `Layout` property in a view:

	@{
		Layout = "~/views/_eastsussexgovuk_fullscreen.cshtml";
	} 

If you want to override the layout view for a single request, you can specify a token in the querystring:

	https://hostname/my-application/my-page?template=Plain

The parameter name defaults to `template` but you can change it using the configuration below. 

	{
  	  "Escc.EastSussexGovUK": {
	    "Mvc": {
	      "ViewParameterName": "custom"
	    }
	  }
	}

The value (for example `Plain` in `?template=Plain`) replaces the `{0}` in `{0}ViewPath`, and the result is used to look up the layout view path.

### Skins

These can be applied on top of MVC layouts for smaller changes. A skin class inherits `IEsccWebsiteSkin` and can specify JavaScript and CSS files to load, fonts from Google and a custom Content Security Policy. Loading custom scripts gives you a lot of flexibility to alter the design and behaviour of the page. One skin can inherit from another by inheriting its class, allowing subtle variations to be created without repeating the settings.

The skin also contains the rules for when to apply it which might, for example, be based on the URL. 

It is then up to individual templates and pages to name the skins that they support using the `SkinSelector` class from this project. For example, to allow either the `ExampleSkin` or `CoronerSkin` when their conditions are met, but default to the `CustomerFocusSkin` when they are not, use the following command. The skins are tested in order and the first matching skin is applied.

	Model.EsccWebsiteSkin = SkinSelector.SelectSkin(
		new IEsccWebsiteSkin[] 
		{ 
			new ExampleSkin(Request.Url), 
			new CoronerSkin(Request.Url) 
		}, 
		new CustomerFocusSkin()
	);


<a name="csp"></a>
## Varying the Content Security Policy

The [Content Security Policy](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy) protects pages from attacks including [XSS](https://www.owasp.org/index.php/Cross-site_Scripting_(XSS)). 

The default policy allows scripts, styles, images and objects from and connections to our own site, plus the use of  `https://ajax.googleapis.com` for script libraries (JQuery), [Google Fonts](https://fonts.google.com/), [Google Analytics](https://analytics.google.com/analytics/web/) and [Crazy Egg](https://www.crazyegg.com/). It also allows resources from any port on `localhost` when `ASPNETCORE_ENVIRONMENT` is set to `Development`.

Predefined policies including the default are maintained in a separate project, `Escc.EastSussexGovUK.ContentSecurityPolicy`, so that the package can be updated by consuming applications without needing to update the whole website template. 

### Varying the policy for the application

Often applications will need to add additional rules to the standard Content Security Policy, and they can do this by providing an instance of `CspOptions` during `Startup`:

	using PeterJuhasz.AspNetCore.Extensions.Security;

 	public class Startup
    {
		...

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
		 	app.UseEastSussexGovUK(env, new CspOptions { 
				ImgSrc = CspDirective.Empty.AddSource("https://www.example.org") 
			});
		    app.UseStaticFiles();
		    app.UseMvc();
		}
	}

The most common cases are preconfigured. For example, to add support for displaying Google Maps and YouTube:

	using PeterJuhasz.AspNetCore.Extensions.Security;

 	public class Startup
    {
		...

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
		 	app.UseEastSussexGovUK(env, new CspOptions().AddGoogleMaps().AddYouTube());
		    app.UseStaticFiles();
		    app.UseMvc();
		}
	}

### Varying the policy for a page

You can also add additional Content Security Policies from controllers and views by registering a `ContentSecurityPolicyDependency` in an `IClientDependencySet`. The `.Alias` property should match the name of one of the preconfigured methods. For example, an alias of `YouTube` causes the `.AddYouTube()` method to be run.

Dependency class:

	using Escc.EastSussexGovUK;

	public class MyCustomDependency : IClientDependencySet
    {
		public IEnumerable<ContentSecurityPolicyDependency> RequiresContentSecurityPolicy()
        {
            return new ContentSecurityPolicyDependency[1]
            {
                new ContentSecurityPolicyDependency(){ Alias = "YouTube" }
            };
        }

		...
	}

View that registers the dependency:

	@inject Escc.EastSussexGovUK.Core.IClientDependencySetEvaluator dependencySetEvaluator;
    dependencySetEvaluator.EvaluateDependencySet(new MyCustomDependency());

## Add common features to pages using an `IClientDependencySet`

An `IClientDependencySet` is a set of Content Security Policy changes, CSS and JavaScript files that together allow a feature to be added to a page. It also includes a method of deciding whether that feature is appropriate for a given page. A skin is an example of an `IClientDependencySet`.

Some features included with `Escc.EastSussexGovUK` are examples of an `IClientDependencySet`:

*  `EmbeddedGoogleMaps`
*  `EmbeddedYouTubeVideos`
*  `WebChat`

Loading resources using an `IClientDependencySet` is supported by `ClientDependencySetEvaluator`. For example, to embed YouTube videos in a view:


	@inject Escc.EastSussexGovUK.Core.IClientDependencySetEvaluator dependencySetEvaluator;
    dependencySetEvaluator.EvaluateDependencySet(new EmbeddedYouTubeVideos() { Html = new [] 
	{ 
		Model.MyRichTextField.ToString(), 
		Model.MyOtherRichTextField.ToString() 
	}});

CSS and JavaScripts are loaded from the [Escc.EastSussexGovUK.TemplateSource](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK) project using the `CssRelativeUrl` and `JsRelativeUrl` properties of the dependency. Content Security Policies are loaded by matching the name of a predefined method in `Escc.EastSussexGovUK.ContentSecurityPolicy.ContentSecurityPolicyExtensions` (see 'Varying the policy for a page' above).

Other examples of `IClientDependencySet` are used internally by the partial views listed below.

## Add common features to pages using partial views

MVC pages can load partial views to add the following features, which are used frequently throughout the site:

* `~/_EastSussexGovUK_Banners.cshtml`, to support sitewide banners (see [Promotional banners](Banners.md))
* `~/_EastSussexGovUK_EastSussex1Space.cshtml`, for an EastSussex1Space search widget
* `~/_EastSussexGovUK_Escis.cshtml`, for an ESCIS search widget
* `~/_EastSussexGovUK_Facebook.cshtml`, for a Facebook feed known as the 'Page Plugin'
* `~/_EastSussexGovUK_Latest.cshtml`, to show a latest update on a page
* `~/_EastSussexGovUK_Share.cshtml`, for social media sharing and comment on this page links
* `~/_EastSussexGovUK_Twitter.cshtml`, for a Twitter feed

For example, to add an ESCIS search widget to your page:

	// Controller
	Model.ShowEscisWidget = true;
	
	// View
	<partial name="~/_EastSussexGovUK_Escis.cshtml" />