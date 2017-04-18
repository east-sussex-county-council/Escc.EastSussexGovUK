# Web chat

Web chat is loaded on a page by the inclusion of [webchat.js](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/blob/master/Escc.EastSussexGovUK/js/webchat.js).

Deciding whether to load web chat needs to happen on the server, so that we can update the content security policy before returning a response to the browser. 

Web chat is configured in Umbraco, and an `IWebChatSettingsService` is used to read the web chat configuration into an instance of `WebChatSettings`. A page that wants to support web chat then needs to use an instance of `WebChat` to read the settings, determine whether web chat is enabled for the page, and identify the CSS, JavaScript and content security policy to load from `web.config`.

	Html.Partial("~/Views/EastSussexGovUK/_FeatureDependencies.cshtml", new List<IClientDependencySet>()
    {
        new WebChat() { WebChatSettings = Model.WebChat }
    });

Web chat is configured in Umbraco using a document type from the [Escc.EastSussexGovUK.Umbraco](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK.Umbraco) project. 

There are two implementations of `IWebChatSettingsService`:

* `WebChatSettingsFromApi` in this project downloads the settings from a URL. This is expected to be JSON data serialised from the `WebChatSettings` class. This data is published by a web API in the `Escc.EastSussexGovUK.Umbraco` project, but it could come from anywhere. 

	This is configured by default in `Escc.EastSussexGovUK.WebForms` on the master pages and `Escc.EastSussexGovUK.Mvc` on the layout views, so that it's available on all pages.

	There are two settings in `web.config` which control the behavior of `WebChatSettingsFromApi`:

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

	If `WebChatSettingsUrl` is missing then web chat support is not loaded. If `WebChatSettingsCacheMinutes` is missing it defaults to 0, which means the JSON data is not cached by the consuming application. 

* `UmbracoWebChatSettingsService` in the `Escc.EastSussexGovUK.Umbraco` project allows web chat to be enabled on Umbraco pages by reading directly from the Umbraco cache rather than via the API. Umbraco layout views in that project use `@section WebChat { }` to disable the built-in support in `Escc.EastSussexGovUK.Mvc` and then use the `UmbracoWebChatSettingsService` instead.

## Why we can't use JavaScript to decide whether to load web chat
Ideally, `webchat.js` would be loaded on every page and read the configuration as JSON data using XHR to determine on the client whether to display the web chat feature. However, our current web chat provider (click4assistance) uses inline styles and inline event handlers which are not allowed by our default content security policy. We can't allow all the inline styles using hashes because Chrome 49 seems to have a limit to the number of hashes it applies, and [inline event handlers can't be enabled selectively at all](https://github.com/w3c/webappsec/issues/468). We don't want to enable a permissive content security policy as standard so we need to make a server-side decision before the page is loaded about whether to allow the content security policy required by web chat.

One possible workaround for this would be to load web chat in an iframe with a permissive content security policy which would be limited to that iframe.