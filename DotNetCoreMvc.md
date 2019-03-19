# Applying the sitewide design with ASP.NET Core MVC

The ASP.NET Core version of the template is not yet feature-complete and is subject to significant changes before it is ready for use on public-facing services.

## Getting started

For an ASP.NET Core MVC project, you can install our design using the following steps:

1. In Visual Studio create a new ASP.NET Core Web Application using the "Empty" project template. 
2. Install `Escc.EastSussexGovUK.Core` from our private NuGet feed.
3. Add the services required for the template to the `Startup` class:

		using Escc.EastSussexGovUK.Core;
		using Microsoft.AspNetCore.Builder;
		using Microsoft.AspNetCore.Hosting;
		using Microsoft.AspNetCore.Mvc;
		using Microsoft.Extensions.Configuration;
		using Microsoft.Extensions.DependencyInjection;

	    public class Startup
	    {
			...
	
	        public void ConfigureServices(IServiceCollection services)
	        {
	            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
	            services.AddEastSussexGovUK(Configuration);
	        }

			...
		}

3. Create a view model which inherits from `Escc.EastSussexGovUK.Core.BaseViewModel`. Its constructor must inject an instance of `IBreadcrumbProvider` since this is required to build the template.

		using Escc.EastSussexGovUK.Core;

	    public class MyCustomModel : BaseViewModel
	    {
	        public MyCustomModel(IViewModelDefaultValuesProvider defaultValuesProvider): base(defaultValuesProvider) { }
	    }

4. Add a controller. The constructor must inject the resources required for the website template. Any controller action which results in a view using the website template must be marked with the `async` keyword and return `Task<IActionResult>`. It must also load the template HTML, and usually web chat too, using the following code. You can put this in a base controller class if you have many controllers in your application.
		
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

			public async Task<IActionResult> Index()
			{
				var model = new MyCustomModel(_defaultModelValues); // inheriting from BaseViewModel

				try
                {
					// Do this if you want your page to support web chat. It should, unless you have a reason not to.
                    model.WebChat = await templateRequest.RequestWebChatSettingsAsync();
                }
                catch (Exception ex)
                {
                    // Catch and report exceptions - don't throw them and cause the page to fail
                    ex.ToExceptionless().Submit();
                }
                try
                {
					// Do this to load the template controls.
                    model.TemplateHtml = await templateRequest.RequestTemplateHtmlAsync();
                }
                catch (Exception ex)
                {
                    // Catch and report exceptions - don't throw them and cause the page to fail
                    ex.ToExceptionless().Submit();
                }

				return View(model);
			}
		}

5. Add a view. The main content of your page should usually use the standard text styles. See the [website style guide](https://github.com/east-sussex-county-council/Escc.WebsiteStyleGuide) for a full guide.

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

	Add a `class` to the opening `<body>` tag:

		@section BodyClass {my-class}

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
			<header>My custom footer</header>
		}

	Include JavaScript at the end of the page (or use the `async` or `defer` attributes when loading it earlier):

		@section JavaScript {
			<script src="~/js/my-script.js?v=@Model.ClientFileVersion"></script>
		}

6. Add configuration for the template elements. For example, if you are using `appsettings.json` for configuration:

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
		  }
		}

	If your application runs behind a proxy server you can configure the proxy URL and authentication details in using the settings documented in the [Escc.Net](https://github.com/east-sussex-county-council/Escc.Net) project.

5. Run the project.

## How it works

The call to `await templateRequest.RequestTemplateHtmlAsync()` attempts to download the template controls from the remote URL, and populates a `TemplateHtml` property on the model. The use of an asynchronous call is very important as it allows the site to scale successfully using this approach.

The URL to download the controls from is set by the `Escc.EastSussexGovUK:Mvc:PartialViewUrl` configuration setting. The template code will try to fetch HTML from that URL, passing a token for the control it wants instead of `{0}`.

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

Using HTTP2 and gzip there is no longer a need to bundle and minify client-side files, so they should be included in the page individually. However it is still useful to include a version number so that the latest version is always loaded after each deployment.

	@section Metadata {
		<link rel="stylesheet" href="~/css/my-styles.css?v=@Model.ClientFileVersion" />
		<script src="~/js/my-script.js?v=@Model.ClientFileVersion" async="async"></script>
	}

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
		      "DesktopMvcLayout": "~/Views/_My_Custom_Desktop.cshtml",
		      "FullScreenMvcLayout": "~/_My_Custom_FullScreen.cshtml",
		      "PlainMvcLayout": "~/_My_Custom_Plain.cshtml"
		    }
		  }
		}

`MvcViewSelector` is called in the constructor of `EastSussexGovUKTemplateRequest` and in the `~\Views\_ViewStart.cshtml` file. It selects the layout based on a number of criteria, including the configuration settings mentioned above.

In an MVC application you can't see the `ViewStart.cshtml` file or any of the layouts because they're implemented as embedded resources.  You can create your own `ViewStart.cshtml` if you need to add additional code, but you'll need to copy [the call to MvcViewSelector](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/blob/master/Escc.EastSussexGovUK.Core/Views/_ViewStart.cshtml) into your new file to continue to apply the correct layout.

If you want to override the master page or view for a specific page in an application, you can set the `Layout` property in a view:

	@{
		Layout = "~/views/_eastsussexgovuk_fullscreen.cshtml";
	} 

### Skins

These can be applied on top of MVC layouts for smaller changes. A skin class inherits `IEsccWebsiteSkin` and can specify JavaScript and CSS files to load, fonts from Google and a custom Content Security Policy. Loading custom scripts gives you a lot of flexibility to alter the design and behaviour of the page. One skin can inherit from another by inheriting its class, allowing subtle variations to be created without repeating the settings.

The skin also contains the rules for when to apply it which might, for example, be based on the URL. 

It is then up to individual templates and pages to name the skins that they support using the `SkinSelector` class from this project. For example, to allow either the `MarriageSkin` or `CoronerSkin` when their conditions are met, but default to the `CustomerFocusSkin` when they are not, use the following command. The skins are tested in order and the first matching skin is applied.

	Model.EsccWebsiteSkin = SkinSelector.SelectSkin(
		new IEsccWebsiteSkin[] 
		{ 
			new MarriageSkin(Request.Url), 
			new CoronerSkin(Request.Url) 
		}, 
		new CustomerFocusSkin()
	);

## Common features which can be added to pages

MVC pages can load partial views to add the following features, which are used frequently throughout the site:

* `~/_EastSussexGovUK_Banners.cshtml`, to support sitewide banners [configured using Umbraco](https://github.com/east-sussex-county-council/Escc.Umbraco.Banners)
* `~/_EastSussexGovUK_EastSussex1Space.cshtml`, for an EastSussex1Space search widget
* `~/_EastSussexGovUK_Escis.cshtml`, for an ESCIS search widget
* `~/_EastSussexGovUK_Facebook.cshtml`, for a Facebook feed known as the 'Page Plugin'
* `~/_EastSussexGovUK_Latest.cshtml`, to show a latest update on a page
* `~/_EastSussexGovUK_Share.cshtml`, for social media sharing and comment on this page links
* `~/_EastSussexGovUK_Twitter.cshtml`, for a Twitter feed

For example, to add an ESCIS search widget to your page:

	// Controller
	Model.ShowEscisWidget  = true;
	
	// View
	@{ await Html.RenderPartialAsync("~/_EastSussexGovUK_Escis.cshtml", Model); }