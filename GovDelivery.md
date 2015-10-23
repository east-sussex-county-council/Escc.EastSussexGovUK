# Newsletter signup in the sitewide header

The 'keep me posted' button reveals a slide-down panel which allows visitors to enter their email address to subscribe to emails using the GovDelivery service. The following process determines how it appears:

1. The page template loads `govdelivery.js`. For example, on WebForms pages `desktop.master` loads `scriptsdesktop.ascx`, which specifies the `Email` script, which resolves to `govdelivery.js` in `web.config`. It loads the `govdelivery-button-*.css` files by a similar method.
2. `govdelivery.js` checks that it is on the https protocol on one of its approved domains. If so, it inserts the 'Keep me posted' button in the header. This picks up the CSS already loaded.
3. When the 'Keep me posted' button is clicked, it makes a request for `govdelivery.html`, inserts it into the page, and slides down the panel to reveal it. However, this request is intercepted by `CorsForStaticFilesHandler` which is registered in `web.config` to handle `*.html` requests in the `controls` folder. `CorsForStaticFilesHandler` checks the origin of the request against a list of approved domains in `web.config` and, if there is a match, inserts the appropriate CORS header. This allows the request to succeed across different domains and subdomains.
4. `govdelivery.html` has a form which posts to the GovDelivery site, at which point that service takes over.
