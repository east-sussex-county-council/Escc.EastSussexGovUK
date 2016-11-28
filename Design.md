# Sitewide design

The design of the [East Sussex County Council website](https://www.eastsussex.gov.uk) is mostly implemented in this project. It consists of a set of page elements which are reused on both WebForms master pages and MVC layouts to avoid duplication. 

## Getting started

For an ASP.NET MVC5 project, you can install our design using the following steps:

1. In Visual Studio create a new ASP.NET Web Application using the "Empty" project template. Tick the box to add folders and core references for MVC.
2. Install `Escc.EastSussexGovUK.Mvc` from our private NuGet feed.
3. Create a view model which inherits from `Escc.EastSussexGovUK.Mvc.BaseViewModel`, add a controller and a view and run the project.

Note: When installing `Escc.EastSussexGovUK.Mvc` into a project that also uses Umbraco, you need to select 'No' when you are prompted to overwrite the `~\Views\web.config` file. You will also find that `Escc.ClientDependencyFramework` incorrectly modifies the `<clientDependency />` element in `web.config`, so you will need to reset this to `<clientDependency configSource="config\ClientDependency.config" />`. 

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
4. You'll usually want to apply the `CustomerFocusSkin` by adding the following lines to the code-behind of your page:
	
		var skinnable = Master as BaseMasterPage;
        if (skinnable != null)
        {
            skinnable.Skin = new CustomerFocusSkin(ViewSelector.CurrentViewIs(MasterPageFile));
        }
5. Your WebForms project will usually be deployed under an IIS website root that uses `Escc.EastSussexGovUK.TemplateSource`. You will need to add the following configuration to avoid errors due to the presence of the `ClientDependency-Mvc5` package at the root:
 
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
 
All new applications should use MVC rather than WebForms. 

## How it works

The master pages and MVC layouts themselves are deliberately minimal, with most of the work done by instances of `MasterPageControl` on the page. This enables you to configure the source location for the controls. By default `MasterPageControl` loads usercontrols from a local directory. This could be a `~/masterpages` virtual directory within your application which points at the `MasterPages` folder in this project so, for example, `ExampleControl` would be loaded from `~/masterpages/Controls/ExampleControl.ascx`. However you can configure this path in `web.config`.

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
	</configuration>

Each instance of `MasterPageControl` has its `Control` property set to a string identifying the control to load. By default it will look for a local usercontrol as explained above, but the recommended approach is to download them from a remote URL and cache them, which typically means loading them direct from `www.eastsussex.gov.uk`. This allows all parts of the website, including third-party services and other applications hosted separately from the main website, to use a consistent approach to loading the template. The template is kept up-to-date and in sync across all parts of the site without having to keep track of changes and manually update the files in each location.

You can configure controls to load from a remote URL by setting the `MasterPageControlUrl` in the `Escc.EastSussexGovUK/RemoteMasterPage` section of `web.config`, it will try to fetch HTML from that URL, passing the value of the `Control` property instead of `{0}`. 

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
	      <add key="Timeout" value="4000" />
	    </RemoteMasterPage>
	  </Escc.EastSussexGovUK>
	</configuration>

The remote control is loaded from an ASPX page, which is just a host for the same usercontrol which would otherwise be loaded locally.
    
The fetched HTML is saved in a local cache so that it is not requested remotely every time. The cache expires using the `CacheMinutes` setting in `web.config`. By default requests time out after 4 seconds (4000 milliseconds), but this can be changed using the `Timeout` setting in `web.config`.

Requesting any page with the querystring `?ForceCacheRefresh=1` will cause the cached template to be updated even if it has not expired.

### Serving the remote master page
The site serving the remote master page should configure a `BaseUrl` in `web.config` inside the folder where `control.aspx` is hosted. The `BaseUrl` should be the domain where sitewide features like images and text size control are hosted (typically www.eastsussex.gov.uk). This `BaseUrl` is prepended to all relative links and resources in the template, to create absolute links back to the central site. This means that the consuming application doesn't need to host its own copy of these files. 


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

###Swapping master pages and layouts

Pages on our site can have different designs applied, and the default designs are implemented as both WebForms masterpages and MVC layouts. By default we use a responsive design ("desktop"), but there is also a full-width, minimal design ("fullscreen") which is suitable for applications like maps, and a "plain" design which can be used as an API to request just the content of a page. These are configured in the `Escc.EastSussexGovUK\GeneralSettings` section of `web.config` as shown below.

You can make dramatic changes to the layout of the site by applying a different master page or layout, and you can configure specific master pages or layouts to apply to specific URLs in the `Escc.EastSussexGovUK\DesktopMasterPages` section of  `web.config`. For example, maps often use `FullScreen.master` which has a minimal header and no footer, allowing them to take up most of the screen. MVC applications can specify `.cshtml` layouts in `Escc.EastSussexGovUK\DesktopMvcLayouts` instead.

It is possible to mix WebForms using master pages and MVC pages using layouts in the same project, implementing the same design so that the change is transparent for the user. This allows you to use MVC to add new functions to applications built using WebForms.

	<configuration>
	  <configSections>
	    <sectionGroup name="Escc.EastSussexGovUK">
	      <section name="GeneralSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
	      <section name="DesktopMasterPages" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
	      <section name="DesktopMvcLayouts" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
        </sectionGroup>
	  </configSections>
	
      <Escc.EastSussexGovUK>
	    <GeneralSettings>
	      <add key="MasterPageParameterName" value="template" />
	      <add key="FullScreenMasterPage" value="~/FullScreen.master" />
	      <add key="PlainMasterPage" value="~/Plain.master" />
	      <add key="FullScreenMvcLayout" value="~/views/eastsussexgovuk/fullscreen.cshtml" />
	      <add key="PlainMvcLayout" value="~/views/eastsussexgovuk/plain.cshtml" />
	    </GeneralSettings>
	    <DesktopMasterPages>
	      <add key="/example-application/map.aspx" value="~/masterpages/fullscreen.master" />
		  <add key="/" value="~/desktop.master" />
	    </DesktopMasterPages>
	    <DesktopMvcLayouts>
	      <add key="/example-application/map/" value="~/views/eastsussexgovuk/fullscreen.cshtml" />
		  <add key="/" value="~/views/eastsussexgovuk/desktop.cshtml" />
	    </DesktopMvcLayouts>
      </Escc.EastSussexGovUK>

	  <system.webServer>
	    <modules>
	      <add name="MasterPageModule" type="Escc.EastSussexGovUK.WebForms.MasterPageModule" />
		</modules>
	  </system.webServer>
	</configuration>

The `ViewSelector` class controls which master page or layout is used. For WebForms applications it is called from the `MasterPageModule` shown in the configuration sample above. In MVC applications, `ViewSelector` is called in the `~\Views\_ViewStart.cshtml` file. It selects the master page based on a number of criteria, including the `web.config` settings mentioned above.

In an MVC application you can't see the `ViewStart.cshtml` file because it's implemented as an embedded resource.  You can create your own `ViewStart.cshtml` if you need to add additional code, but you'll need to copy [the call to ViewSelector](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/blob/master/Escc.EastSussexGovUK.Mvc/Views/_ViewStart.cshtml) into your new file to continue to apply the correct layout.

### Skins

These can be applied on top of master pages or layouts for smaller changes. A skin class inherits `IEsccWebsiteSkin` and can specify JavaScript and CSS files to load, fonts from Typekit and Google, a custom Content Security Policy and a text class to apply to supporting content. Loading custom scripts gives you a lot of flexibility to alter the design and behaviour of the page. One skin can inherit from another by inheriting its class, allowing subtle variations to be created without repeating the settings.

The skin also contains the rules for when to apply it which might, for example, be based on the URL. 

It is then up to individual templates and pages to name the skins that they support using the `SkinSelector` class from this project. For example, to allow either the `MarriageSkin` or `CoronerSkin` when their conditions are met, but default to the `CustomerFocusSkin` when they are not, use the following command. The skins are tested in order and the first matching skin is applied.

	Model.EsccWebsiteSkin = SkinSelector.SelectSkin(
		new IEsccWebsiteSkin[] 
		{ 
			new MarriageSkin(Model.EsccWebsiteView, Request.Url), 
			new CoronerSkin(Model.EsccWebsiteView, Request.Url) 
		}, 
		new CustomerFocusSkin(Model.EsccWebsiteView)
	);

## An alternative approach for third-party applications

The approach described above is designed primarily with ASP.NET in mind, and does involve installing minimal master pages or MVC layouts on the consuming site. 

An alternative and usually better approach for third-parties which need to use our design is for us to set up a page on our website using the remote master page, with tokens to indicate where the metadata, content and other elements should go, and then a third-party can download a copy of the page on a regular schedule (or on every request) and use it as a template in their application. The tokens, breadcrumb trail, CSS and other elements can be changed to suit the needs to the consuming application.

This has two main advantages: 

* it can fit in more easily with other technologies
* all of the template elements are controlled by us, which makes them easier (and cheaper) to keep up-to-date.

An example of this approach in use is the [modern.gov template](https://new.eastsussex.gov.uk/moderngov/template.aspx) used by our [democracy pages](https://democracy.eastsussex.gov.uk/).

### Download elements of the template rather than the whole thing
If you would prefer to load our controls to your existing template system you also have the option of copying the instances of `MasterPageControl` from our `*.master` pages onto your own WebForms site template where they will still work. In an MVC application you can wrap `MasterPageControl` in a Razor-compatible ASCX partial view: see [MasterPageControl.ascx](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/blob/master/Escc.EastSussexGovUK.Mvc/Views/EastSussexGovUK/MasterPageControl.ascx) for an example. If you use a different server-side technology such as PHP you can still load our template this way, but you'll need to port `MasterPageControl` into your language.



## CSS, JavaScript and images

CSS, JavaScript and image files used by two or more applications belong in this project. This includes the CSS for our responsive design. Documentation is included within the CSS files, and in our [website style guide](https://github.com/east-sussex-county-council/Escc.WebsiteStyleGuide).

Our sitewide CSS and JavaScript files as part of the remote master page are minified using [YUI Compressor](https://www.nuget.org/packages/YUICompressor.NET.MSBuild) and concatenated on-the-fly using our own [Escc.ClientDependencyFramework.WebForms](https://github.com/east-sussex-county-council/Escc.ClientDependencyFramework/tree/master/Escc.ClientDependencyFramework.WebForms) project. MVC pages use the newer [Escc.ClientDependencyFramework](https://github.com/east-sussex-county-council/Escc.ClientDependencyFramework) for their local CSS and JavaScript.

The printer icon used in our CSS was made by [Yannick](http://yanlu.de). It's from [flaticon.com](http://www.flaticon.com) and licensed under [Creative Commons BY 3.0](http://creativecommons.org/licenses/by/3.0/).

## Common features which can be added to pages

A number of features used frequently throughout the site are installed with the remote master pages and layouts.

### WebForms
WebForms applications can use the following local usercontrols:

* `~/1Space.ascx`, for an EastSussex1Space search widget
* `~/share.ascx` to present social media sharing links. 

ASPX file:

	<%@ Register TagPrefix="EastSussexGovUK" tagName="Share" src="~/share.ascx" %>
	<%@ Register TagPrefix="EastSussexGovUK" tagName="EastSussex1Space" src="~/1space.ascx" %>

	<EastSussexGovUK:EastSussex1Space runat="server"/>
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