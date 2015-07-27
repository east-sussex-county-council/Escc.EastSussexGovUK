Escc.EastSussexGovUK
====================

This project provides the template and some sitewide features for www.eastsussex.gov.uk. It includes the following features:

CSS, JavaScript and images
--------------------------

CSS, JavaScript and image files used by two or more applications belong in this project. This includes the CSS for our responsive design. Documentation is included within the CSS files, and in our [website style guide](https://github.com/east-sussex-county-council/Escc.WebsiteStyleGuide).

Our CSS and JavaScript files are minified using [YUI Compressor](https://www.nuget.org/packages/YUICompressor.NET.MSBuild) and concatenated on-the-fly using our own [Escc.ClientDependency.WebForms](https://github.com/east-sussex-county-council/Escc.ClientDependencyFramework/tree/master/Escc.ClientDependencyFramework.WebForms) project. 

The printer icon used in our CSS was made by [Yannick](http://yanlu.de). It's from [flaticon.com](http://www.flaticon.com) and licensed under [Creative Commons BY 3.0](http://creativecommons.org/licenses/by/3.0/).

Master pages
------------

Pages on our site can have different master pages (templates) applied. By default we choose between a responsive design ("desktop") and a cut-down version for small screens ("mobile"). There is also a "plain" design which can be used as an API to request just the content of a page, and other customised versions for specific sections.

The `ViewSelector` class controls which master page is used, and is called either from the `MasterPageModule` or a specific request to `choose.ashx`.

The master pages themselves are deliberately minimal, with most of the work done by usercontrols on the page. This enables a remote master page feature, where a minimal master page on a subdomain can request a copy of the latest template elements from www.eastsussex.gov.uk. This allows us to keep multiple copies of the template in sync automatically, and is documented in the `MasterPageControl` class.

## Google Analytics support

### Loading our tracker code

We use Google Universal Analytics, with multiple subdomains of `eastsussex.gov.uk` that make up the website tracked in a single profile. 

Our tracker code is in our own `analytics.js` in this project:

- For WebForms pages and [Modern.gov](https://democracy.eastsussex.gov.uk) this is loaded from `~\masterpages\controls\Scripts*.ascx`, which in turn is included on our WebForms master pages in this project.
- For Umbraco MVC pages this is loaded from `~\views\layouts\Scripts*.cshtml`, which in turn is included in our layout views, all in the `Escc.EastSussexGovUK.UmbracoViews` project.
- The [E-library](https://e-library.eastsussex.gov.uk) loads the same tracking code our main site using a separate reference in its own template. 

Our tracker code is repeated in `display-as-html.xslt` and `display-as-html-v2.xslt` for RSS feeds.

The following sites also log data to the same Google Analytics property, using their own copies of the tracking code:

- [Have Your Say Hub](https://consultation.eastsussex.gov.uk/) 

### Tracking options

We track the following custom events:

- clicks in the header and footer (in `analytics.js`)
- clicks on external and mailto links (in `Escc.Statistics.js`, via NuGet)
- usage of the EastSussex1Space widget (in `1space.js`) 
- loading the 404 page (in `Error404.ascx.cs`)
- clicks on the news items on the home page (in `homepage.js`, via NuGet)
- clicks on search results (in `search.js` in `Escc.Search.Website`)

We use virtual page views in `Escc.Statistics.js` (via NuGet) to track document downloads.

We track social interactions with the Facebook feed (in `social-media.js`)

iCalendar data
--------------

We encode events on our pages using the hCalendar microformat. Where enabled, you can change the extension of any page to `.calendar` (for example, change `somepage.aspx` to `somepage.calendar`) to get a download page for the calendar data. Change the extension again to `.ics` (for example, change `somepage.calendar` to `somepage.ics`) and you get the calendar data itself, which is parsed directly from the data embedded in `somepage.aspx`.

This works by setting the `.calendar` and `.ics` extensions to be parsed by ASP.NET in `web.config`:

	<system.webServer>
		<handlers>
			<add name="iCalendar pre-download page" path="*.calendar" verb="*" modules="IsapiModule" scriptProcessor="C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll" resourceType="Unspecified" preCondition="integratedMode,runtimeVersionv4.0,bitness32" />
			<add name="iCalendar download" path="*.ics" verb="*" modules="IsapiModule" scriptProcessor="C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll" resourceType="Unspecified" preCondition="integratedMode,runtimeVersionv4.0,bitness32" />
		</handlers>
	</system.webServer>

This enables an HTTP module to run for those requests, looking for the custom extensions:

	<system.webServer>
		<modules>
			<add name="RewriteByExtensionModule" type="EsccWebTeam.EastSussexGovUK.MasterPages.Data.RewriteByExtensionModule, EsccWebTeam.EastSussexGovUK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=06fad7304560ae6f" />
		</modules>
	</system.webServer>

The HTTP module looks to `web.config` to see which extensions it should look for, and the ASP.NET page which should handle the request. These are in the `masterpages\data` folder, and use open-source XSLT files by Brian Suda (and another one customising that result) to handle the request, which are also specified in `web.config`:
	
	<configSections>
		<sectionGroup name="EsccWebTeam.EastSussexGovUK">
			<section name="Data" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
		</sectionGroup>
	</configSections>
	
	<EsccWebTeam.EastSussexGovUK>
		<Data>
			<add key=".calendar" value="~/masterpages/data/calendar.aspx?url={0}" />
			<add key=".ics" value="~/masterpages/data/hcalendar.ashx?url={0}" />
			
			<add key="HCalendarPrepareXslt" value="~/masterpages/data/hcalendar.xslt" />
			<add key="HCalendarXslt" value="~/masterpages/data/xhtml2vcal.xsl" />
		</Data>
	</EsccWebTeam.EastSussexGovUK>

When testing calendar downloads on a local copy of IIS you'll need to use HTTP rather than HTTPS because, when making a web request for the page containing the calendar data to parse, a development machine usually has an SSL certificate which is not sufficiently trusted.

You may also find that requests for `.calendar` pages report a session state error from `MasterPageModule`: 

	"Session state can only be used when enableSessionState is set to true, either in a configuration file or in the Page directive. Please also make sure that System.Web.SessionStateModule or a custom session state module is included in the <configuration>\<system.web>\<httpModules> section in the application configuration."

The solution to this is to re-register the session state module:


	<system.webServer>
		<modules>
			<remove name="Session" />
			<add name="Session" type="System.Web.SessionState.SessionStateModule" preCondition="" />
	    </modules>
	</system.webServer>

## 'Keep me posted' button
The 'keep me posted' button reveals a slide-down panel which allows visitors to enter their email address to subscribe to emails using the GovDelivery service. The following process determines how it appears:

1. The page template loads `govdelivery.js`. For example, on WebForms pages `desktop.master` loads `scriptsdesktop.ascx`, which specifies the `Email` script, which resolves to `govdelivery.js` in `web.config`. It loads the `govdelivery-button-*.css` files by a similar method.
2. `govdelivery.js` checks that it is on the https protocol on one of its approved domains. If so, it inserts the 'Keep me posted' button in the header. This picks up the CSS already loaded.
3. When the 'Keep me posted' button is clicked, it makes a request for `govdelivery.html`, inserts it into the page, and slides down the panel to reveal it. However, this request is intercepted by `CorsForStaticFilesHandler` which is registered in `web.config` to handle `*.html` requests in the `controls` folder. `CorsForStaticFilesHandler` checks the origin of the request against a list of approved domains in `web.config` and, if there is a match, inserts the appropriate CORS header. This allows the request to succeed across different domains and subdomains.
4. `govdelivery.html` has a form which posts to the GovDelivery site, at which point that service takes over.

## RSS feed styles

Different browsers display RSS feeds in different ways. Some (Internet Explorer and Firefox) and have built in template, but we provide styles for others (such as Chrome) which would otherwise show plain XML.

## RDF data home page

Based on an idea suggested in a blog post, `eastsussexcountycouncil.rdf` provides a starting point for exploring the RDF data published by East Sussex County Council, much like the home page does for HTML content.

## Development setup steps

1. From an Administrator command prompt, run `app-setup-dev.cmd` to set up a site in IIS
2. Replace sample values in `web.config` with ones appropriate to your setup
3. Replace sample values in `js\config.js` with ones appropriate to your setup
4. Build the solution