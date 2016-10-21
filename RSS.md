# RSS feed styles

East Sussex County Council RSS feeds should be styled by installing the `Escc.EastSussexGovUK.Rss` NuGet package from our private feed. This makes CSS, image and XSLT resources available to your application as embedded resources. The only external resource you will depend upon is a script which runs our Google Analytics tracking code if the feed is displayed in a browser. This is loaded from `www.eastsussex.gov.uk` to avoid repeating our Google Analytics configuration settings.

Different browsers display RSS feeds in different ways. Some (Internet Explorer and Firefox) and have built in template, but we provide styles for others (such as Chrome) which would otherwise show plain XML.

1. For browsers which don't support XSLT `rss.css` styles the raw RSS feed. No major browsers use this any more.

2. For browsers which support XSLT, `rss-to-html.xslt` transforms the raw RSS feed into HTML, and that HTML loads `rss-xslt.css` to style the resulting HTML. Note that `rss-to-html.xslt` must be loaded from the same origin as the RSS feed itself.

An RSS feed should include code similar to the following to support this styling:

	<?xml version="1.0" encoding="utf-8"?>
	<?xml-stylesheet type="text/xsl" href="https://hostname/path-to-application/eastsussexgovuk-rss/rss-to-html.ashx" ?>
    <?xml-stylesheet type="text/css" href="https://hostname/path-to-application/eastsussexgovuk-rss/rss.css" ?>
	<rss version="2.0" xmlns:atom="http://www.w3.org/2005/Atom">
		<channel>
			<title>
		      Your feed title - East Sussex County Council
		    </title>
			<image>
		      <title>East Sussex County Council logo</title>
		      <url>https://hostname/path-to-application/eastsussexgovuk-rss/escc-logo-for-feed.gif</url>
		      <width>90</width>
		      <height>65</height>
		      <link>https://www.eastsussex.gov.uk/</link>
		    </image>
			...
		</channel>
		...
	</rss>

