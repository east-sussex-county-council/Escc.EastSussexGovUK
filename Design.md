# Sitewide design

The design of the [East Sussex County Council website](https://www.eastsussex.gov.uk) is mostly implemented in this project. An MVC version of the template is implemented separately in [Escc.EastSussexGovUK.UmbracoViews](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK.UmbracoViews), though it re-uses elements from this project as much as possible.

## Master pages

Pages on our site can have different master pages (templates) applied. By default we choose between a responsive design ("desktop") and a cut-down version for small screens ("mobile"). There is also a "plain" design which can be used as an API to request just the content of a page, and other customised versions for specific sections.

You can make dramatic changes to the layout of the site by applying a different master page, and you can configure specific master pages to apply to specifc URLs in the `EsccWebTeam.EastSussexGovUK\DesktopMasterPages` and `EsccWebTeam.EastSussexGovUK\MobileMasterPages` sections of  `web.config`. For example, maps often use `FullScreen.master` which has a minimal header and no footer, allowing them to take up most of the screen. MVC applications can specify `.cshtml` layouts instead of master pages.

	<configuration>
	  <configSections>
	    <sectionGroup name="EsccWebTeam.EastSussexGovUK">
	      <section name="GeneralSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
	      <section name="DesktopMasterPages" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
	      <section name="MobileMasterPages" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
        </sectionGroup>
	  </configSections>
	
      <EsccWebTeam.EastSussexGovUK>
	    <GeneralSettings>
	      <add key="MasterPageParameterName" value="template" />
	      <add key="FullScreenMasterPage" value="~/masterpages/FullScreen.master" />
	      <add key="PlainMasterPage" value="~/masterpages/Plain.master" />
	    </GeneralSettings>
	    <DesktopMasterPages>
	      <add key="/example-application/map.aspx" value="~/masterpages/fullscreen.master" />
		  <add key="/" value="~/masterpages/desktop.master" />
	    </DesktopMasterPages>
	    <MobileMasterPages>
	  	  <add key="/example-application/map.aspx" value="~/masterpages/fullscreen.master" />
	      <add key="/" value="~/masterpages/mobile.master" />
	    </MobileMasterPages>
      </EsccWebTeam.EastSussexGovUK>
	</configuration>

The `ViewSelector` class controls which master page is used, and is called either from the `MasterPageModule`, a specific request to `choose.ashx` or, in MVC applications, the `~\Views\_ViewStart.cshtml` file. It selects the master page based on a number of criteria, including the `web.config` settings mentioned above.

The master pages themselves are deliberately minimal, with most of the work done by instances of `MasterPageControl` on the page. This enables you to choose configure the source location for the controls. By default `MasterPageControl` loads usercontrols from a local directory. This is typically a `~/masterpages` virtual directory within your application which points at the `MasterPages` folder in this project so, for example, `ExampleControl` would be loaded from `~/masterpages/Controls/ExampleControl.ascx`. However you can configure this path in `web.config`.

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

## Remote master page

You can load all the main elements of the master page from a remote URL, which typically means loading them direct from `www.eastsussex.gov.uk`. This is useful for applications hosted separately from the main website, including third-party services, as it allows them to keep their copy of the template up-to-date and in sync with the rest of the site without having to keep track of changes and manually update the files.

* If the consuming application can be adapted to use our master pages, then copies of `*.master` are hosted on the consuming site. 
* If you have an ASP.NET application but can't adapt it to our master pages, you can copy the instances of `MasterPageControl` from our `*.master` pages onto your own site template where they will still work. In an MVC application you can wrap `MasterPageControl` in a Razor-compatible ASCX partial view: see `MasterPageControl.ascx` in the [Escc.EastSussexGovUK.UmbracoViews](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK.UmbracoViews) project for an example.
* If you use a different server-side technology such as PHP you can still load our template this way, but you'll need to port `MasterPageControl` into your language.

Each instance of `MasterPageControl` has its `Control` property set to a string identifying the control to load. By default it will look for a local usercontrol as explained above, but if `MasterPageControlUrl` is set in the `EsccWebTeam.EastSussexGovUK/RemoteMasterPage` section of `web.config`, it will try to fetch HTML from that URL, passing the value of the `Control` property instead of `{0}`. 

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

## Skins

These can be applied on top of master pages for smaller changes. A skin class inherits `IEsccWebsiteSkin` and can specify JavaScript and CSS files to load, fonts from Typekit and Google, a custom Content Security Policy and a text class to apply to supporting content. Loading custom scripts gives you a lot of flexibility to alter the design and behaviour of the page. One skin can inherit from another by inheriting its class, allowing subtle variations to be created without repeating the settings.

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

Our CSS and JavaScript files are minified using [YUI Compressor](https://www.nuget.org/packages/YUICompressor.NET.MSBuild) and concatenated on-the-fly using our own [Escc.ClientDependency.WebForms](https://github.com/east-sussex-county-council/Escc.ClientDependencyFramework/tree/master/Escc.ClientDependencyFramework.WebForms) project. 

The printer icon used in our CSS was made by [Yannick](http://yanlu.de). It's from [flaticon.com](http://www.flaticon.com) and licensed under [Creative Commons BY 3.0](http://creativecommons.org/licenses/by/3.0/).