Escc.EastSussexGovUK
====================

This project provides the template and some sitewide features for www.eastsussex.gov.uk. It includes the following features:

CSS, JavaScript and images
--------------------------

CSS, JavaScript and image files used by two or more applications belong in this project. This includes the CSS for our responsive design. Documentation is included within the CSS files, and in our [website style guide](https://github.com/east-sussex-county-council/Escc.WebsiteStyleGuide).

Our CSS and JavaScript files are minified using [YUI Compressor](https://github.com/yui/yuicompressor) and concatenated on-the-fly using our own EsccWebTeam.Egms project. 

The printer icon used in our CSS was made by [Yannick](http://yanlu.de). It's from [flaticon.com](http://www.flaticon.com) and licensed under [Creative Commons BY 3.0](http://creativecommons.org/licenses/by/3.0/).

Master pages
------------

Pages on our site can have different master pages (templates) applied. By default we choose between a responsive design ("desktop") and a cut-down version for small screens ("mobile"). There is also a "plain" design which can be used as an API to request just the content of a page, and other customised versions for specific sections.

The `ViewSelector` class controls which master page is used, and is called either from the `MasterPageModule` or a specific request to `choose.ashx`.

The master pages themselves are deliberately minimal, with most of the work done by usercontrols on the page. This enables a remote master page feature, where a minimal master page on a subdomain can request a copy of the latest template elements from www.eastsussex.gov.uk. This allows us to keep multiple copies of the template in sync automatically, and is documented in the `MasterPageControl` class.

iCalendar data
--------------

We encode events on our pages using the hCalendar microformat, and files in the `masterpages\data` folder enable that data to be read and served as iCalendar downloads.

RSS feed styles
---------------

Different browsers display RSS feeds in different ways. Some (Internet Explorer and Firefox) and have built in template, but we provide styles for others (such as Chrome) which would otherwise show plain XML.

RDF data home page
------------------

Based on an idea suggested in a blog post, `eastsussexcountycouncil.rdf` provides a starting point for exploring the RDF data published by East Sussex County Council, much like the home page does for HTML content.

## Development setup steps

1. From an Administrator command prompt, run `app-setup-dev.cmd` to set up a site in IIS
2. Replace sample values in `web.config` with ones appropriate to your setup
3. Replace sample values in `js\config.js` with ones appropriate to your setup
4. Build the solution