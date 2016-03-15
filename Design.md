# Sitewide design

The design of the [East Sussex County Council website](https://www.eastsussex.gov.uk) is mostly implemented in this project. It consists of a set of page elements which are reused on both WebForms master pages and MVC layouts to avoid duplication. 

## Getting started

For an ASP.NET MVC5 project, you can install our design using the following steps:

1. In Visual Studio create a new ASP.NET Web Application using the "Empty" project template. Tick the box to add folders and core references for MVC.
2. Install `Escc.EastSussexGovUK.Mvc` from our private NuGet feed.
3. Add the `EsccWebTeam.EastSussexGovUK`, `EsccWebTeam.Data.Web` and `EsccWebTeam.NavigationControls` projects to your solution. Add project references for the first two from your MVC project. If these projects don't build you may need to run `nuget restore` from the command line inside each of these project folders to restore their dependencies. (This step won't be needed once these projects have been converted to NuGet.)
4. Add a controller and a view and run the project. 

For WebForms projects there is not yet a NuGet package, so you will need to read the documentation below to see how to configure your application. 

## Loading the master page from a URL

It is possible to load the main elements of the master page as local usercontrols. However, the recommended approach is to download them from a remote URL and cache them, which typically means loading them direct from `www.eastsussex.gov.uk`. This allows all parts of the website, including third-party services and other applications hosted separately from the main website, to use a consistent approach to loading the template. The template is kept up-to-date and in sync across all parts of the site without having to keep track of changes and manually update the files in each location.

* If the consuming application can be adapted to use our WebForms master pages, then copies of `*.master` are hosted on the consuming site. 
* If the consuming application uses MVC5 it can use our `Escc.EastSussexGovUK.Mvc` NuGet package to install and configure the MVC layouts.
* If you have an ASP.NET application but can't adapt it to our master pages, you can copy the instances of `MasterPageControl` from our `*.master` pages onto your own WebForms site template where they will still work. In an MVC application you can wrap `MasterPageControl` in a Razor-compatible ASCX partial view: see [MasterPageControl.ascx](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/blob/master/Escc.EastSussexGovUK.Mvc/Views/EastSussexGovUK/MasterPageControl.ascx) for an example.
* If you use a different server-side technology such as PHP you can still load our template this way, but you'll need to port `MasterPageControl` into your language.

### How it works

The master pages and MVC layouts themselves are deliberately minimal, with most of the work done by instances of `MasterPageControl` on the page. This enables you to configure the source location for the controls. By default `MasterPageControl` loads usercontrols from a local directory. This is typically a `~/masterpages` virtual directory within your application which points at the `MasterPages` folder in this project so, for example, `ExampleControl` would be loaded from `~/masterpages/Controls/ExampleControl.ascx`. However you can configure this path in `web.config`.

	<configuration>
	  <configSections>
	    <sectionGroup name="EsccWebTeam.EastSussexGovUK">
	      <section name="GeneralSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
        </sectionGroup>
	  </configSections>
	
      <EsccWebTeam.EastSussexGovUK>
	    <GeneralSettings>
	      <add key="MasterPageControlUrl" value="~/example-folder/{0}.ascx" />
	    </GeneralSettings>
      </EsccWebTeam.EastSussexGovUK>
	</configuration>

Each instance of `MasterPageControl` has its `Control` property set to a string identifying the control to load. By default it will look for a local usercontrol as explained above but, if `MasterPageControlUrl` is set in the `EsccWebTeam.EastSussexGovUK/RemoteMasterPage` section of `web.config`, it will try to fetch HTML from that URL, passing the value of the `Control` property instead of `{0}`. 

In the following example, `ExampleControl` would be loaded from `https://www.eastsussex.gov.uk/masterpages/remote/control.aspx?control=ExampleControl`. If your application runs behind a proxy server you can configure the proxy URL and authentication details in `web.config` using the format documented in the [Escc.Net](https://github.com/east-sussex-county-council/Escc.Net) project.

	<configuration>
	  <configSections>
	    <sectionGroup name="EsccWebTeam.EastSussexGovUK">
	      <section name="RemoteMasterPage" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
	    </sectionGroup>
	  </configSections>

	  <EsccWebTeam.EastSussexGovUK>
	    <RemoteMasterPage>
	      <add key="CacheMinutes" value="60" />
	      <add key="MasterPageControlUrl" value="https://www.eastsussex.gov.uk/masterpages/remote/control.aspx?control={0}" />
	    </RemoteMasterPage>
	  </EsccWebTeam.EastSussexGovUK>
	</configuration>

The remote control is loaded from an ASPX page, which is just a host for the same usercontrol which would otherwise be loaded locally.
    
The fetched HTML is saved in a local cache so that it is not requested remotely every time. The cache expires using the `CacheMinutes` setting in `web.config`.

Requesting any page with the querystring `?ForceCacheRefresh=1` will cause the cached template to be updated even if it has not expired.

### Serving the remote master page
The site serving the remote master page should configure a `BaseUrl` in `web.config` inside the folder where `control.aspx` is hosted. The `BaseUrl` should be the domain where sitewide features like images and text size control are hosted (typically www.eastsussex.gov.uk). This `BaseUrl` is prepended to all relative links and resources in the template, to create absolute links back to the central site. This means that the consuming application doesn't need to host its own copy of these files. 


	<configuration>
	  <configSections>
	    <sectionGroup name="EsccWebTeam.EastSussexGovUK">
	      <section name="GeneralSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
        </sectionGroup>
	  </configSections>

      <EsccWebTeam.EastSussexGovUK>
        <GeneralSettings>
          <add key="BaseUrl" value="http://www.eastsussex.gov.uk" />
        </GeneralSettings>
      <EsccWebTeam.EastSussexGovUK>
	</configuration>

## Varying the design

###Swapping master pages and layouts

Pages on our site can have different designs applied, and the default designs are implemented as both WebForms masterpages and MVC layouts. By default we use a responsive design ("desktop"), but there is also a full-width, minimal design ("fullscreen") which is suitable for applications like maps, and a "plain" design which can be used as an API to request just the content of a page. These are configured in the `EsccWebTeam.EastSussexGovUK\GeneralSettings` section of `web.config` as shown below.

You can make dramatic changes to the layout of the site by applying a different master page or layout, and you can configure specific master pages or layouts to apply to specific URLs in the `EsccWebTeam.EastSussexGovUK\DesktopMasterPages` section of  `web.config`. For example, maps often use `FullScreen.master` which has a minimal header and no footer, allowing them to take up most of the screen. MVC applications can specify `.cshtml` layouts instead of master pages.

It is possible to mix WebForms using master pages and MVC pages using layouts in the same project, implementing the same design so that the change is transparent for the user. This allows you to use MVC to add new functions to applications built using WebForms.

	<configuration>
	  <configSections>
	    <sectionGroup name="EsccWebTeam.EastSussexGovUK">
	      <section name="GeneralSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
	      <section name="DesktopMasterPages" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
	      <section name="DesktopMvcLayouts" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
        </sectionGroup>
	  </configSections>
	
      <EsccWebTeam.EastSussexGovUK>
	    <GeneralSettings>
	      <add key="MasterPageParameterName" value="template" />
	      <add key="FullScreenMasterPage" value="~/masterpages/FullScreen.master" />
	      <add key="PlainMasterPage" value="~/masterpages/Plain.master" />
	      <add key="FullScreenMvcLayout" value="~/views/eastsussexgovuk/fullscreen.cshtml" />
	      <add key="PlainMvcLayout" value="~/views/eastsussexgovuk/plain.cshtml" />
	    </GeneralSettings>
	    <DesktopMasterPages>
	      <add key="/example-application/map.aspx" value="~/masterpages/fullscreen.master" />
		  <add key="/" value="~/masterpages/desktop.master" />
	    </DesktopMasterPages>
	    <DesktopMvcLayouts>
	      <add key="/example-application/map/" value="~/views/eastsussexgovuk/fullscreen.cshtml" />
		  <add key="/" value="~/views/eastsussexgovuk/desktop.cshtml" />
	    </DesktopMvcLayouts>
      </EsccWebTeam.EastSussexGovUK>

	  <system.webServer>
	    <modules>
	      <add name="MasterPageModule" type="EsccWebTeam.EastSussexGovUK.MasterPages.MasterPageModule, EsccWebTeam.EastSussexGovUK" />
		</modules>
	  </system.webServer>
	</configuration>

The `ViewSelector` class controls which master page or layout is used. For WebForms applications it is called from the `MasterPageModule` shown in the configuration sample above. In MVC applications, `ViewSelector` is called in the `~\Views\_ViewStart.cshtml` file. It selects the master page based on a number of criteria, including the `web.config` settings mentioned above. It can also be called by a specific request to `choose.ashx` (deprecated).

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


## CSS, JavaScript and images

CSS, JavaScript and image files used by two or more applications belong in this project. This includes the CSS for our responsive design. Documentation is included within the CSS files, and in our [website style guide](https://github.com/east-sussex-county-council/Escc.WebsiteStyleGuide).

Our sitewide CSS and JavaScript files as part of the remote master page are minified using [YUI Compressor](https://www.nuget.org/packages/YUICompressor.NET.MSBuild) and concatenated on-the-fly using our own [Escc.ClientDependencyFramework.WebForms](https://github.com/east-sussex-county-council/Escc.ClientDependencyFramework/tree/master/Escc.ClientDependencyFramework.WebForms) project. MVC pages use the newer [Escc.ClientDependencyFramework](https://github.com/east-sussex-county-council/Escc.ClientDependencyFramework) for their local CSS and JavaScript.

The printer icon used in our CSS was made by [Yannick](http://yanlu.de). It's from [flaticon.com](http://www.flaticon.com) and licensed under [Creative Commons BY 3.0](http://creativecommons.org/licenses/by/3.0/).