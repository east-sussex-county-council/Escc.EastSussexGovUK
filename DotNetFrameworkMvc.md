# Applying the sitewide design with ASP.NET MVC5

MVC5 is current recommended technology for new applications on the website, but it will soon be replaced by ASP.NET Core once that version of the template has all the features of this one. You should consider using the ASP.NET Core version instead and keeping it up-to-date with template changes.

## Getting started

For an ASP.NET MVC5 project, you can install our design using the following steps:

1. In Visual Studio create a new ASP.NET Web Application using the "Empty" project template. Tick the box to add folders and core references for MVC.
2. Install `Escc.EastSussexGovUK.Mvc` from our private NuGet feed.
3. Create a view model which inherits from `Escc.EastSussexGovUK.Mvc.BaseViewModel`, add a controller and a view.
4. Any controller action which results in a view using the website template must be marked with the `async` keyword and return `Task<ActionResult>`. It must also load the website template, and usually web chat too, using the following code. You can put this in a base controller class if you have many controllers in your application.
		
		using Escc.EastSussexGovUK.Mvc;
		using Exceptionless;
		using System.Threading.Tasks;
		using System.Web.Mvc

		public class ExampleController : Controller
		{
			public async Task<ActionResult> Index()
			{
				var model = new MyCustomModel(); // inheriting from BaseViewModel
				var templateRequest = new EastSussexGovUKTemplateRequest(Request);
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
5. Run the project.

Note: When installing `Escc.EastSussexGovUK.Mvc` into a project that also uses Umbraco, you need to select 'No' when you are prompted to overwrite the `~\Views\web.config` file. You will also find that `Escc.ClientDependencyFramework` incorrectly modifies the `<clientDependency />` element in `web.config`, so you will need to reset this to `<clientDependency configSource="config\ClientDependency.config" />`. 

## How it works

The call to `await templateRequest.RequestTemplateHtmlAsync()` attempts to download the template controls from the remote URL, and populates a `TemplateHtml` property on the model. The use of an asynchronous call is very important as it allows the site to scale successfully using this approach.

The URL to download the controls from is set by the `MasterPageControlUrl` setting in the `Escc.EastSussexGovUK/RemoteMasterPage` section of `web.config`. The template code will try to fetch HTML from that URL, passing a token for the control it wants instead of `{0}`.

In the following example, `ExampleControl` would be loaded from `https://www.eastsussex.gov.uk/masterpages/remote/control.aspx?control=ExampleControl`. If your application runs behind a proxy server you can configure the proxy URL and authentication details in `web.config` using the format documented in the [Escc.Net](https://github.com/east-sussex-county-council/Escc.Net) project.

	<configuration>
	  <configSections>
	    <sectionGroup name="Escc.EastSussexGovUK">
	      <section name="RemoteMasterPage" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
	    </sectionGroup>
	  </configSections>

	  <Escc.EastSussexGovUK>
	    <RemoteMasterPage>
	      <add key="CacheMinutes" value="60" />
	      <add key="MasterPageControlUrl" value="https://www.eastsussex.gov.uk/masterpages/remote/control.aspx?control={0}" />
	    </RemoteMasterPage>
	  </Escc.EastSussexGovUK>
	</configuration>

The fetched HTML is saved in a local cache so that it is not requested remotely every time. The cache expires using the `CacheMinutes` setting in `web.config`.

Requesting any page with the querystring `?ForceCacheRefresh=1` will cause the cached template to be updated even if the cache has not expired.

## Varying the design

### Loading CSS and JavaScript

MVC pages use [ClienDependency Framework](https://github.com/shazwazza/clientdependency) for their local CSS and JavaScript. The paths for sitewide CSS and JavaScripts should be loaded from configuration using [Escc.ClientDependencyFramework](https://github.com/east-sussex-county-council/Escc.ClientDependencyFramework).

### Swapping master pages and layouts

Pages on our site can have different designs applied. By default we use a responsive design ("desktop"), but there is also a full-width, minimal design ("fullscreen") which is suitable for applications like maps, and a "plain" design which can be used as an API to request just the content of a page. These are configured in the `Escc.EastSussexGovUK\GeneralSettings` section of `web.config` as shown below. You can make dramatic changes to the layout of the site by applying a different master page or layout.

	<configuration>
	  <configSections>
	    <sectionGroup name="Escc.EastSussexGovUK">
	      <section name="GeneralSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
        </sectionGroup>
	  </configSections>
	
      <Escc.EastSussexGovUK>
	    <GeneralSettings>
	      <add key="MasterPageParameterName" value="template" />
	      <add key="DesktopMvcLayout" value="~/views/eastsussexgovuk/desktop.cshtml" />
	      <add key="FullScreenMvcLayout" value="~/views/eastsussexgovuk/fullscreen.cshtml" />
	      <add key="PlainMvcLayout" value="~/views/eastsussexgovuk/plain.cshtml" />
	    </GeneralSettings>
      </Escc.EastSussexGovUK>
	</configuration>

`MvcViewSelector` is called in the constructor of `EastSussexGovUKTemplateRequest` and in the `~\Views\_ViewStart.cshtml` file. It selects the layout based on a number of criteria, including the `web.config` settings mentioned above.

In an MVC application you can't see the `ViewStart.cshtml` file because it's implemented as an embedded resource.  You can create your own `ViewStart.cshtml` if you need to add additional code, but you'll need to copy [the call to MvcViewSelector](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/blob/master/Escc.EastSussexGovUK.Mvc/Views/_ViewStart.cshtml) into your new file to continue to apply the correct layout.

If you want to override the master page or view for a specific page in an application, you can set the `Layout` property in a view:

	@{
		Layout = "~/views/eastsussexgovuk/fullscreen.cshtml";
	} 


It is possible to mix WebForms using master pages and MVC pages using layouts in the same project, implementing the same design so that the change is transparent for the user. This allows you to use MVC to add new functions to applications built using WebForms.

### Skins

These can be applied on top of MVC layouts for smaller changes. A skin class inherits `IEsccWebsiteSkin` and can specify JavaScript and CSS files to load, fonts from Typekit and Google and a custom Content Security Policy. Loading custom scripts gives you a lot of flexibility to alter the design and behaviour of the page. One skin can inherit from another by inheriting its class, allowing subtle variations to be created without repeating the settings.

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

* `_Banners.cshtml`, to support sitewide banners [configured using Umbraco](https://github.com/east-sussex-county-council/Escc.Umbraco.Banners)
* `_EastSussex1Space.cshtml`, for an EastSussex1Space search widget
* `_Escis.cshtml`, for an ESCIS search widget
* `_Facebook.cshtml`, for a Facebook feed known as the 'Page Plugin'
* `_Latest.cshtml`, to show a latest update on a page
* `_Share.cshtml`, for social media sharing and comment on this page links
* `_Twitter.cshtml`, for a Twitter feed
* `_SupportingContentDesktop.cshtml` to load the most common right-column features in one go

For example, to add an ESCIS search widget to your page:

	// Controller
	Model.ShowEscisWidget  = true;
	
	// View
	@{ Html.RenderPartial("~/Views/EastSussexGovUK/Features/_Escis.cshtml", Model); }