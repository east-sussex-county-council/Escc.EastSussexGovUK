# RSS feed styles

Different browsers display RSS feeds in different ways. Some (Internet Explorer and Firefox) and have built in template, but we provide styles for others (such as Chrome) which would otherwise show plain XML.

1. For browsers which don't support XSLT `rss.css` styles the raw RSS feed. No major browsers use this any more.

2. For browsers which support XSLT, `display-as-html.xslt` (or one of its variants) transforms the raw RSS feed into HTML, and that HTML loads `rss-xslt.css` (or one of its variants) to style the resulting HTML. Variants exist for different environments, because we don't have a way to pass parameters into this client-side XSLT process. Note that `display-as-html.xslt` must be loaded from the same origin as the RSS feed itself.

An RSS feed should include code similar to the following to support this styling:

	<?xml version="1.0" encoding="utf-8"?>
	<?xml-stylesheet type="text/xsl" href="https://hostname/masterpages/rss/display-as-html.xslt" ?>
    <?xml-stylesheet type="text/css" href="https://www.eastsussex.gov.uk/css/rssfeed.cssx" ?>
	<rss version="2.0" xmlns:atom="http://www.w3.org/2005/Atom">
	...
	</rss>