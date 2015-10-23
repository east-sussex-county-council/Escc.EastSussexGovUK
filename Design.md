# Sitewide design

The design of the [East Sussex County Council website](https://www.eastsussex.gov.uk) is mostly implemented in this project. An MVC version of the template is implemented separately in [Escc.EastSussexGovUK.UmbracoViews](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK.UmbracoViews), though it re-uses elements from this project as much as possible.

## CSS, JavaScript and images

CSS, JavaScript and image files used by two or more applications belong in this project. This includes the CSS for our responsive design. Documentation is included within the CSS files, and in our [website style guide](https://github.com/east-sussex-county-council/Escc.WebsiteStyleGuide).

Our CSS and JavaScript files are minified using [YUI Compressor](https://www.nuget.org/packages/YUICompressor.NET.MSBuild) and concatenated on-the-fly using our own [Escc.ClientDependency.WebForms](https://github.com/east-sussex-county-council/Escc.ClientDependencyFramework/tree/master/Escc.ClientDependencyFramework.WebForms) project. 

The printer icon used in our CSS was made by [Yannick](http://yanlu.de). It's from [flaticon.com](http://www.flaticon.com) and licensed under [Creative Commons BY 3.0](http://creativecommons.org/licenses/by/3.0/).

## Master pages

Pages on our site can have different master pages (templates) applied. By default we choose between a responsive design ("desktop") and a cut-down version for small screens ("mobile"). There is also a "plain" design which can be used as an API to request just the content of a page, and other customised versions for specific sections.

You can make dramatic changes to the layout of the site by applying a different master page, and you can configure specific master pages to apply to specifc URLs in the `EsccWebTeam.EastSussexGovUK\DesktopMasterPages` and `EsccWebTeam.EastSussexGovUK\MobileMasterPages` sections of  `web.config`. For example, maps often use `FullScreen.master` which has a minimal header and no footer, allowing them to take up most of the screen.

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

The `ViewSelector` class controls which master page is used, and is called either from the `MasterPageModule` or a specific request to `choose.ashx`. It selects the master page based on a number of criteria, including the `web.config` settings mentioned above.

The master pages themselves are deliberately minimal, with most of the work done by usercontrols on the page. This enables a remote master page feature, where a minimal master page on a subdomain can request a copy of the latest template elements from www.eastsussex.gov.uk. This allows us to keep multiple copies of the template in sync automatically, and is documented in the `MasterPageControl` class.

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
