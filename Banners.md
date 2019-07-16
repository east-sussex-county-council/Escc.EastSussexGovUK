# Promotional banners

Promotional banners are [configured in Umbraco](https://github.com/east-sussex-county-council/Escc.EastSussexGovUK.Umbraco/blob/master/Banners.md) using a document type in the `Escc.EastSussexGovUK.Umbraco.Web` project, but can be displayed on any page using the website template. They normally appear on the right-hand side in the desktop layout, but not on mobile. Banners are not part of the template by default, so any page that wants to support banners needs to opt-in with the following steps: 

* Load `~\Escc.EastSussexGovUK.TemplateSource\js\min\banners.js`. This is dependent on other scripts (JQuery, JQuery Retry and `cascading-content.js`) but these are loaded by default. The recommended way to load `banners.js` varies depending on whether your project is using [ASP.NET Core MVC](DotNetCoreMvc.md), [ASP.NET MVC5](DotNetFrameworkMvc.md) or [ASP.NET WebForms](DotNetFrameworkWebForms.md).
* Have a `<span class="escc-banners"></span>` element where the banners should be added on the page. The ASP.NET Core MVC `~\Views\_EastSussexGovUK_Banners.cshtml` can be used to add this in a consistent, future-proofed way. In ASP.NET MVC5 `~\Views\EastSussexGovUK\Features\_Banners.cshtml` can be used. 
* Ensure `config.js` includes an `esccConfig.BannersUrl` property which points to JSON data published by the `Escc.EastSussexGovUK.Umbraco.Web` project.
* Have a content security policy with a `connect-src` which allows the domain specified in `esccConfig.BannersUrl`, and a `img-src` that allows the banner images to display.
* Have your application's domain listed in `web.config` for the site hosting `Escc.EastSussexGovUK.Umbraco.Web` as an allowed domain for CORS requests. 

