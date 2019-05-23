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
* [Redirects and custom errors](RedirectsAndCustomErrors.md)
* [RSS feed styles](RSS.md)
* [Pop-up tips for form controls](Tips.md) (used for GDPR compliance)

## Development setup steps

1. From an Administrator command prompt, run `app-setup-dev.cmd` to set up a site in IIS
2. Replace sample values in `web.config` with ones appropriate to your setup
3. Replace sample values in `js\config.js` with ones appropriate to your setup
4. Build the solution