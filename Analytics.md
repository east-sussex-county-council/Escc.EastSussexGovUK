# Google Analytics support

We use Google Universal Analytics, with multiple subdomains of `eastsussex.gov.uk` that make up the website tracked in a single profile. 

## Loading our tracker code

Our tracker code is in our own `analytics.js` in this project:

- For WebForms pages and [Modern.gov](https://democracy.eastsussex.gov.uk) this is loaded from `~\masterpages\controls\Scripts*.ascx`, which in turn is included on our WebForms master pages in this project.
- For Umbraco MVC pages this is loaded from `~\views\layouts\Scripts*.cshtml`, which in turn is included in our layout views, all in the `Escc.EastSussexGovUK.UmbracoViews` project.
- The [E-library](https://e-library.eastsussex.gov.uk) loads the same tracking code our main site using a separate reference in its own template. 

Our tracker code is repeated in `display-as-html.xslt` and `display-as-html-v2.xslt` for RSS feeds.

The following sites also log data to the same Google Analytics property, using their own copies of the tracking code:

- [Have Your Say Hub](https://consultation.eastsussex.gov.uk/) 

## Tracking options

We track the following custom events:

- clicks in the header and footer (in `analytics.js`)
- clicks on external and mailto links (in `Escc.Statistics.js`, via NuGet)
- usage of the EastSussex1Space widget (in `1space.js`) 
- loading the 404 page (in `Error404.ascx.cs`)
- clicks on the news items on the home page (in `homepage.js`, via NuGet)
- clicks on search results (in `search.js` in `Escc.Search.Website`)

We use virtual page views in `Escc.Statistics.js` (via NuGet) to track document downloads.

We track social interactions with the Facebook feed (in `social-media.js`)
