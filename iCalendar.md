# iCalendar data

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