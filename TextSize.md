# Text size control in the sitewide header

When there is enough screen space to show it (above 802px wide) a text size control is included in the sitewide header. This is an accessibility feature. Although browsers all support zooming pages, that feature is not always obvious to people less familiar with the web, who are often the prople that need it most.

Our site supports raising the base text size in two increments. This setting is maintained by writing a cookie to the user's browser. The cookie is valid for any `*.eastsussex.gov.uk` domain, though it is interpreted by code on our site so unrelated subdomains should simply ignore it. If the header is displayed on a domain other than `*.eastsussex.gov.uk` or an internal server, the text size feature will not be shown.

When you set the cookie to one of the larger text sizes, this is picked up by `desktop.master` for WebForms or `desktop.cshtml` for MVC, which inserts an extra CSS class into the page. `small.css` looks for that class and increases the base font size.

By default the link in the header goes to `/textsize/change` (implemented in [Escc.EastSussexGovUK.Umbraco](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK.Umbraco)) but this can be configured to any application-relative URL in `web.config`. 

	<configuration>
	  <configSections>
	    <sectionGroup name="Escc.EastSussexGovUK">
	      <section name="GeneralSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
        </sectionGroup>
	  </configSections>

      <Escc.EastSussexGovUK>
        <GeneralSettings>
          <add key="TextSizeUrl" value="~/textsize/change" />
        </GeneralSettings>
      <Escc.EastSussexGovUK>
	</configuration>

When using the remote master page this is prefixed by the `BaseUrl` setting configured on the site serving the remote template, creating an absolute link back to main site. This means that consumers of the remote template don't need to do anything to support the text size feature. 

However, if a consuming application application needs to disable the text size feature because it's running on a top-level domain other than `*.eastsussex.gov.uk`, it can do so by adding `&enabletextsize=false` to the remote template URL.

	<configuration>
	  <configSections>
	    <sectionGroup name="Escc.EastSussexGovUK">
	      <section name="RemoteMasterPage" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
	    </sectionGroup>
	  </configSections>
	
	  <Escc.EastSussexGovUK>
	    <RemoteMasterPage>
	      <add key="MasterPageControlUrl" value="/masterpages/remote/control.aspx?control={0}&amp;enabletextsize=false" />
	    </RemoteMasterPage>
	  <Escc.EastSussexGovUK>
	</configuration>