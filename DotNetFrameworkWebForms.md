# Applying the sitewide design with ASP.NET WebForms

All new applications should use MVC rather than WebForms, so this documentation is only for maintaining existing applications. 

## Getting started

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

However the recommended approach is to download them from a remote URL and cache them, which typically means loading them direct from `www.eastsussex.gov.uk`. The URL to download the controls from is set by the `MasterPageControlUrl` setting in the `Escc.EastSussexGovUK/RemoteMasterPage` section of `web.config`. The template code will try to fetch HTML from that URL, passing a token for the control it wants instead of `{0}`. WebForms calls to download the template are synchronous due to limitations of the platform, so updating the application to use MVC will improve its performance and scalability.

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

## Varying the design

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
	      <add key="DesktopMasterPage" value="~/Desktop.master" />
	      <add key="FullScreenMasterPage" value="~/FullScreen.master" />
	      <add key="PlainMasterPage" value="~/Plain.master" />
	    </GeneralSettings>
      </Escc.EastSussexGovUK>
	</configuration>

`WebFormsViewSelector` controls which master page is used. It is called from the `MasterPageModule` which must be registered in `web.config` as shown in the previous section. It selects the master page based on a number of criteria, including the `web.config` settings mentioned above.

If you want to override the master page or view for a specific page in an application, you can set `MasterPageFile` in the `Page_PreInit` event:

	protected void Page_PreInit(object sender, EventArgs e)
    {
        MasterPageFile = "~/fullscreen.master";
    }

## Common features which can be added to pages

WebForms applications can use the following local usercontrol:

* `~/share.ascx` to present social media sharing links. 

ASPX file:

	<%@ Register TagPrefix="EastSussexGovUK" tagName="Share" src="~/share.ascx" %>
	<EastSussexGovUK:Share runat="server" />
