# Web chat

Web chat is loaded on a page by the inclusion of [webchat.js](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/blob/master/Escc.EastSussexGovUK/js/webchat.js). This code is provided by the third-party web chat provider.

Deciding whether to load web chat needs to happen on the server, so that we can update the content security policy before returning a response to the browser. 

An `IWebChatSettingsService` is used to read the web chat configuration into an instance of `WebChatSettings`. A page that wants to support web chat then needs to use an instance of `WebChat` to read the settings, determine whether web chat is enabled for the page, and identify the CSS, JavaScript and content security policy to load from `web.config`.

	Html.Partial("~/Views/EastSussexGovUK/_FeatureDependencies.cshtml", new List<IClientDependencySet>()
    {
        new WebChat() { WebChatSettings = Model.WebChat }
    });

`WebChatSettingsFromApi` in this project implements `IWebChatSettingsService` and downloads the settings from a URL. This is expected to be JSON data serialised from the `WebChatSettings` class. This data is published by a web API in the [Escc.EastSussexGovUK.Umbraco](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK.Umbraco) project, where the URLs that web chat should be enabled on are managed, but it could come from anywhere. 

You should normally enable web chat in all your page controllers as shown in the documentation for [ASP.NET Core MVC](DotNetCoreMvc.md) and [ASP.NET MVC5](DotNetFrameworkMvc.md). In `Escc.EastSussexGovUK.WebForms` it's enabled by default on the master pages. 

There are two settings which control the behavior of `WebChatSettingsFromApi`:

*  `WebChatSettingsUrl` - if this is missing then web chat support is not loaded. 
*  `WebChatSettingsCacheMinutes` - if this is missing it defaults to 0, which means the JSON data is not cached by the consuming application.

For ASP.NET Core MVC these can come from any configuration source. For example `appsettings.json`:

	{
  	  "Escc.EastSussexGovUK": {
	    "WebChat": {
	      "WebChatSettingsUrl": "https://www.eastsussex.gov.uk/umbraco/api/WebChat/GetWebChatUrls",
		  "CacheMinutes": 60
	    }
	  }
	}

For MVC5 and WebForms these are configured in `web.config`:

	<configuration>
	  <configSections>
	    <sectionGroup name="Escc.EastSussexGovUK">
	      <section name="GeneralSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
        </sectionGroup>
	  </configSections>
	
      <Escc.EastSussexGovUK>
	    <GeneralSettings>
	      <add key="WebChatSettingsUrl" value="https://hostname/umbraco/api/WebChat/GetWebChatUrls" />
		  <add key="WebChatSettingsCacheMinutes" value="60" />
	    </GeneralSettings>
      </Escc.EastSussexGovUK>
	</configuration>

## Why we can't use JavaScript to decide whether to load web chat
Ideally, `webchat.js` would be loaded on every page and read the configuration as JSON data using XHR to determine on the client whether to display the web chat feature. However, our current web chat provider (click4assistance) uses inline scripts and the [potentially dangerous `eval` function](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/eval#Do_not_ever_use_eval!) which are not allowed by our default content security policy. We don't want to enable a permissive content security policy as standard so we need to make a server-side decision before the page is loaded about whether to allow the content security policy required by web chat.

One possible workaround for this would be to load web chat in an iframe with a permissive content security policy which would be limited to that iframe.