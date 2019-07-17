# Escc.EastSussexGovUK

This project provides the template and some sitewide features for www.eastsussex.gov.uk. It includes the following features:

* [Sitewide design](Design.md)
	- Applying the sitewide design with [ASP.NET Core MVC](DotNetCoreMvc.md)
	- Applying the sitewide design with [ASP.NET MVC5](DotNetFrameworkMvc.md)
	- Applying the sitewide design with [ASP.NET WebForms](DotNetFrameworkWebForms.md)
* [Newsletter signup in the sitewide header](GovDelivery.md)
* [Text size control in the sitewide header](TextSize.md)
* [Google Analytics support](Analytics.md)
* [Google Tag Manager support](TagManager.md)
* [Google Maps support](GoogleMaps.md)
* [Promotional banners](Banners.md)
* [Web chat](WebChat.md)
* [YouTube support](YouTube.md)
* [Redirects and custom errors](RedirectsAndCustomErrors.md)
* [RSS feed styles](RSS.md)
* [Pop-up tips for form controls](Tips.md) (used for GDPR compliance)
* [Rewriting links to email addresses](Email.md)
* [Privacy banner](Privacy.md) (used for GDPR compliance)
* [Catalogue machines in public libraries](PublicLibraries.md)

## The role of Escc.EastSussexGovUK.TemplateSource

Our website is made up of several subdomains. `Escc.EastSussexGovUK.TemplateSource` is the root application of our website on each subdomain, except where the subdomain is a run by a third-party application.

However, the main part of our site runs Umbraco which also needs to be configured to be the root application. To resolve this conflict the Umbraco site integrates the `Escc.EastSussexGovUK.TemplateSource` application in two ways:

*  `Escc.EastSussexGovUK.TemplateSource` publishes a NuGet package which is installed in the Umbraco application, containing the static files which must by convention appear at the root of the website, and containing `Escc.EastSussexGovUK.TemplateSource.dll` so that pages from `Escc.EastSussexGovUK.TemplateSource` can run within the context of the Umbraco application and find the code they expect.
*  The Umbraco application has virtual directories set up which point to folders within `Escc.EastSussexGovUK.TemplateSource` (`masterpages`, `img` and `css\images`, and `escc.eastsussexgovuk` to point to the root of `Escc.EastSussexGovUK.TemplateSource`), so that those folders appear to be within the Umbraco application. The Umbraco application also adds the URLs `~/escc.eastsussexgovuk/` and `~/masterpages` to its `umbracoReservedPaths` setting in `web.config`. 
  
This makes it possible for the Umbraco application to [serve the remote master page](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK/blob/master/Design.md#serving-the-remote-master-page) using code and configuration from `Escc.EastSussexGovUK.TemplateSource`, preserving some of the URLs expected by the files in `Escc.EastSussexGovUK.TemplateSource` even though they are running from a different application. These URLs are also preserved from a previous implementation of the website, which aided migration.

## Development setup steps

1. From an Administrator command prompt, run `app-setup-dev.cmd` to set up a site in IIS
2. Replace sample values in `web.config` with ones appropriate to your setup
3. Replace sample values in `js\config.js` with ones appropriate to your setup
4. Build the solution