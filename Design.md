# Sitewide design

The design of the [East Sussex County Council website](https://www.eastsussex.gov.uk) is mostly implemented in this project. It consists of a set of page elements which are reused on both WebForms master pages and MVC layouts to avoid duplication. 

## Getting started

### MVC5 projects
For an ASP.NET MVC5 project, you can install our design using the following steps:

1. In Visual Studio create a new ASP.NET Web Application using the "Empty" project template. Tick the box to add folders and core references for MVC.
2. Install `Escc.EastSussexGovUK.Mvc` from our private NuGet feed.
3. Create a view model which inherits from `Escc.EastSussexGovUK.Mvc.BaseViewModel`, add a controller and a view.
4. Any controller action which results in a view using the website template must be marked with the `async` keyword and return `Task<ActionResult>`. It must also load the website template, and usually web chat too, using the following code. You can put this in a base controller class if you have many controllers in your application.
		
		using Escc.EastSussexGovUK.Mvc;
		using Exceptionless;
		using System.Threading.Tasks;
		using System.Web.Mvc

		public class Example : Controller
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

### WebForms projects

All new applications should use MVC rather than WebForms, so this documentation is only for maintaining existing applications. 

For a WebForms project, you can install our design using the following steps:

1. In Visual Studio create a new ASP.NET Web Application using the "Empty" project template. Tick the box to add folders and core references for WebForms.
2. Install `Escc.EastSussexGovUK.WebForms` from our private NuGet feed.
3. Create a WebForms page and use the `<asp:Content />` element with any of the following values for `ContentPlaceholderId`:  
	* `contentExperiment`
	* `metadata`
	* `css`
	* `header`
	* `breadcrumb`
	* `content`
	* `supporting`
	* `afterForm`
	* `footer`
	* `javascript`

	In most cases you'll only need to use `metadata` and `content`.
4. Your WebForms project will usually be deployed under an IIS website root that uses `Escc.EastSussexGovUK.TemplateSource`. You will need to add the following configuration to avoid errors due to the presence of the `ClientDependency-Mvc5` package at the root:
 
		<configuration>
		  <system.web>
		    <pages>
		      <namespaces>
		        <remove namespace="ClientDependency.Core.Mvc" />
		      </namespaces>
		    </pages>
		  </system.web>
		  <system.webServer>
		    <modules>
		      <remove name="ClientDependencyModule" />
		    </modules>
		  </system.webServer>
		</configuration>

	You will also need to ensure the entire `bin\roslyn` folder is copied to your deployed application, because `Escc.EastSussexGovUK.TemplateSource` uses the `Microsoft.CodeDom.Providers.DotNetCompilerPlatform` package which configures an additional Roslyn-based compiler to enable C#6 features.  
5.  You'll usually want the default `CustomerFocusSkin`, but if you need a different one you can adding the following lines to the code-behind of your page:
	
		var skinnable = Master as BaseMasterPage;
        if (skinnable != null)
        {
            skinnable.Skin = new MyCustomSkin(ViewSelector.CurrentViewIs(MasterPageFile));
        }

## How it works

The master pages and MVC layouts themselves are deliberately minimal, with most of the work done by remotely-hosted controls that together make up the website design. 

This allows all parts of the website, including third-party services and other applications hosted separately from the main website, to use a consistent approach to loading the template. The template is kept up-to-date and in sync across all parts of the site without having to keep track of changes and manually update the files in each location.

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

The remote control is loaded from an ASPX page, which is just a host for a usercontrol that is loaded locally.
    
The fetched HTML is saved in a local cache so that it is not requested remotely every time. The cache expires using the `CacheMinutes` setting in `web.config`.

Requesting any page with the querystring `?ForceCacheRefresh=1` will cause the cached template to be updated even if the cache has not expired.

### MVC5 projects

The call to `await templateRequest.RequestTemplateHtmlAsync()` attempts to download the template controls from the remote URL, and populates a `TemplateHtml` property on the model. The use of an asynchronous call is very important as it allows the site to scale successfully using this approach.

### WebForms projects

In WebForms projects most of the work is done by instances of `MasterPageControl` on the page. Each instance of `MasterPageControl` has its `Control` property set to a string identifying the control to load. By default `MasterPageControl` loads usercontrols from a local directory. This could be a `~/masterpages` virtual directory within your application which points at the `MasterPages` folder in this project so, for example, `ExampleControl` would be loaded from `~/masterpages/Controls/ExampleControl.ascx`. However you can configure this path in `web.config`.

	<configuration>
	  <configSections>
	    <sectionGroup name="Escc.EastSussexGovUK">
	      <section name="GeneralSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
        </sectionGroup>
	  </configSections>
	
      <Escc.EastSussexGovUK>
	    <GeneralSettings>
	      <add key="MasterPageControlUrl" value="~/example-folder/{0}.ascx" />
	    </GeneralSettings>
      </Escc.EastSussexGovUK>

	  <system.webServer>
	    <modules>
	      <add name="MasterPageModule" type="Escc.EastSussexGovUK.WebForms.MasterPageModule" />
		</modules>
	  </system.webServer>
	</configuration>

However the recommended approach is to download them from a remote URL and cache them, which typically means loading them direct from `www.eastsussex.gov.uk`. To enable this, complete the `<RemoteMasterPage />` configuration section as explained above. WebForms calls to download the template are synchronous due to limitations of the platform, so updating the application to use MVC will improve its performance and scalability.

In WebForms the master page is set by the `MasterPageModule` which must be registered in `web.config`. This is not required for MVC pages.

### Serving the remote master page
The site serving the remote master page should configure a `BaseUrl` in `web.config` inside the folder where `control.aspx` is hosted. The `BaseUrl` should be the domain where sitewide features like images and text size control are hosted (typically www.eastsussex.gov.uk). This `BaseUrl` is prepended to all relative links and images in the template, to create absolute links back to the central site. This means that the consuming application doesn't need to host its own copy of these files. The `BaseUrl` can include a `{hostname}` token which is replaced with the value of `HttpContext.Current.Request.Url.Authority`.


	<configuration>
	  <configSections>
	    <sectionGroup name="Escc.EastSussexGovUK">
	      <section name="GeneralSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
        </sectionGroup>
	  </configSections>

      <Escc.EastSussexGovUK>
        <GeneralSettings>
          <add key="BaseUrl" value="http://www.eastsussex.gov.uk" />
        </GeneralSettings>
      <Escc.EastSussexGovUK>
	</configuration>

## Varying the design

### Swapping master pages and layouts

Pages on our site can have different designs applied, and the default designs are implemented as both WebForms masterpages and MVC layouts. By default we use a responsive design ("desktop"), but there is also a full-width, minimal design ("fullscreen") which is suitable for applications like maps, and a "plain" design which can be used as an API to request just the content of a page. These are configured in the `Escc.EastSussexGovUK\GeneralSettings` section of `web.config` as shown below. You can make dramatic changes to the layout of the site by applying a different master page or layout.

It is possible to mix WebForms using master pages and MVC pages using layouts in the same project, implementing the same design so that the change is transparent for the user. This allows you to use MVC to add new functions to applications built using WebForms.

	<configuration>
	  <configSections>
	    <sectionGroup name="Escc.EastSussexGovUK">
	      <section name="GeneralSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
        </sectionGroup>
	  </configSections>
	
      <Escc.EastSussexGovUK>
	    <GeneralSettings>
	      <add key="MasterPageParameterName" value="template" />
	      <add key="DesktopMasterPage" value="~/Desktop.master" />
	      <add key="FullScreenMasterPage" value="~/FullScreen.master" />
	      <add key="PlainMasterPage" value="~/Plain.master" />
	      <add key="DesktopMvcLayout" value="~/views/eastsussexgovuk/desktop.cshtml" />
	      <add key="FullScreenMvcLayout" value="~/views/eastsussexgovuk/fullscreen.cshtml" />
	      <add key="PlainMvcLayout" value="~/views/eastsussexgovuk/plain.cshtml" />
	    </GeneralSettings>
      </Escc.EastSussexGovUK>
	</configuration>

Technology-specific implementations of the `ViewSelector` class control which master page or layout is used. For WebForms applications `WebFormsViewSelector` is called from the `MasterPageModule` shown in the configuration sample above. In MVC applications, `MvcViewSelector` is called in the constructor of `EastSussexGovUKTemplateRequest` and in the `~\Views\_ViewStart.cshtml` file. It selects the master page based on a number of criteria, including the `web.config` settings mentioned above.

In an MVC application you can't see the `ViewStart.cshtml` file because it's implemented as an embedded resource.  You can create your own `ViewStart.cshtml` if you need to add additional code, but you'll need to copy [the call to ViewSelector](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/blob/master/Escc.EastSussexGovUK.Mvc/Views/_ViewStart.cshtml) into your new file to continue to apply the correct layout.

If you want to override the master page or view for a specific page in an application, for WebForms you can set `MasterPageFile` in the `Page_PreInit` event:

	protected void Page_PreInit(object sender, EventArgs e)
    {
        MasterPageFile = "~/fullscreen.master";
    }

and for MVC you can set the `Layout` property in a view:

	@{
		Layout = "~/views/eastsussexgovuk/fullscreen.cshtml";
	} 

### Skins

These can be applied on top of master pages or layouts for smaller changes. A skin class inherits `IEsccWebsiteSkin` and can specify JavaScript and CSS files to load, fonts from Typekit and Google, a custom Content Security Policy and a text class to apply to supporting content. Loading custom scripts gives you a lot of flexibility to alter the design and behaviour of the page. One skin can inherit from another by inheriting its class, allowing subtle variations to be created without repeating the settings.

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

## An alternative approach for third-party applications

The approach described above is designed primarily with ASP.NET in mind, and does involve installing minimal master pages or MVC layouts on the consuming site. 

An alternative and usually better approach for third-parties which need to use our design is for us to set up a page on our website using the remote master page, with tokens to indicate where the metadata, content and other elements should go, and then a third-party can download a copy of the page on a regular schedule (or on every request) and use it as a template in their application. The tokens, breadcrumb trail, CSS and other elements can be changed to suit the needs to the consuming application.

This has two main advantages: 

* it can fit in more easily with other technologies
* all of the template elements are controlled by us, which makes them easier (and cheaper) to keep up-to-date.

An example of this approach in use is the [modern.gov template](https://new.eastsussex.gov.uk/moderngov/template.aspx) used by our [democracy pages](https://democracy.eastsussex.gov.uk/).

## CSS, JavaScript and images

CSS, JavaScript and image files used by two or more applications belong in this project. This includes the CSS for our responsive design. Documentation is included within the CSS files, and in our [website style guide](https://github.com/east-sussex-county-council/Escc.WebsiteStyleGuide).

Our sitewide CSS and JavaScript files as part of the remote master page are minified using [YUI Compressor](https://www.nuget.org/packages/YUICompressor.NET.MSBuild) and concatenated on-the-fly using our own [Escc.ClientDependencyFramework.WebForms](https://github.com/east-sussex-county-council/Escc.ClientDependencyFramework/tree/master/Escc.ClientDependencyFramework.WebForms) project. MVC pages use the newer [Escc.ClientDependencyFramework](https://github.com/east-sussex-county-council/Escc.ClientDependencyFramework) for their local CSS and JavaScript.

The printer icon used in our CSS was made by [Yannick](http://yanlu.de). It's from [flaticon.com](http://www.flaticon.com) and licensed under [Creative Commons BY 3.0](http://creativecommons.org/licenses/by/3.0/).

## Common features which can be added to pages

A number of features used frequently throughout the site are installed with the remote master pages and layouts.

### WebForms
WebForms applications can use the following local usercontrol:

* `~/share.ascx` to present social media sharing links. 

ASPX file:

	<%@ Register TagPrefix="EastSussexGovUK" tagName="Share" src="~/share.ascx" %>
	<EastSussexGovUK:Share runat="server" />

### MVC5

MVC pages can load partial views to add the following features:

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