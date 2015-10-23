# Promotional banners

Promotional banners are configured in Umbraco using templates from the `Escc.Umbraco.Banners` project. They normally appear on the right-hand side in the desktop layout, but not on mobile. Banners are not part of the template by default, so any page that wants to support banners needs to opt-in with the following steps: 

* Load `banners.js`. This is dependent on other scripts (JQuery, JQuery Retry and `cascading-content.js`) but these are loaded by default.
* Have a `<span class="escc-banners"></span>` element where the banners should be added on the page. The `Banners.ascx` control can be used to add this in a consistent, future-proofed way.
* Ensure `config.js` includes an `esccConfig.BannersUrl` property which points to JSON data published by the `Escc.Umbraco.Banners` project.
* Have a content security policy with a `connect-src` which allows the domain specified in `esccConfig.BannersUrl`, and a `img-src` that allows the banner images to display.
* Have its domain registered with the site hosting `Escc.Umbraco.Banners` as an allowed domain for CORS requests. 

