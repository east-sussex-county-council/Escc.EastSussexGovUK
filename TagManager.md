# Google Tag Manager support

The Google Tag Manager container ids are stored in `web.config` and, because we don't want to use the production container in a test environment, the relevant one is selected based on the hostname of the request. 

The container id is published in the page by `HtmlTag.ascx` as an `<html data-gtm="GTM-xxxxxx">` attribute, which is picked up by the Google Tag Manager JavaScript in `google-tag-manager.js`. It's also published in an `iframe` element by `FooterDesktop.ascx` as a fallback for browsers with JavaScript disabled. 

In both cases the container id is published by controls which can be requested from other hosts as part of the [remote master page](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/blob/master/Design.md#remote-master-page), therefore it is not enough simply to store a single container id for the current host in `web.config`. A host which serves up the remote master page needs to select the appropriate container id for any host that could request the template, including itself. It does this by checking the incoming hostname against a series of regular expressions. The first matching expression determines the container id to be used.

    <configuration>
	  <configSections>
	    <sectionGroup name="EsccWebTeam.EastSussexGovUK">
	      <section name="GoogleTagManagerIdRules" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
	    </sectionGroup>
      </configSections>

      <EsccWebTeam.EastSussexGovUK>
	    <GoogleTagManagerIdRules>
      	  <add key="\.eastsussex\.gov\.uk$" value="GTM-xxxxxx" />
          <add key="azurewebsitename[-a-z0-9]*\.azurewebsites.net" value="GTM-xxxxxx" />
          <add key=".*" value="GTM-xxxxxx" />
    	</GoogleTagManagerIdRules>
      </EsccWebTeam.EastSussexGovUK>
 	</configuration>	   

`google-tag-manager.js` is loaded by `Scripts*.ascx` for WebForms pages and any pages using the remote master page. Umbraco pages have their own equivalent `Scripts*.cshtml` files in [Escc.EastSussexGovUK.Umbraco](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK.Umbraco). (The `iframe` element for Google Tag Manager is published in `FooterDesktop.ascx` rather `Scripts*.acsx` to avoid duplicating it for Umbraco.)
