<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:dc="http://purl.org/dc/elements/1.1/" version="1.0">
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
      <head>
        <meta charset="UTF-8" />
        <title>
          <xsl:value-of select="rss/channel/title"/>
        </title>
        <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:400,600" rel="stylesheet" type="text/css" />
        <link rel="stylesheet" type="text/css" href="{rss-xslt.css}" />
      </head>
      <body xmlns="http://www.w3.org/1999/xhtml">

        <header>

          <div class="header" role="banner">
            <a href="https://www.eastsussex.gov.uk">
              <img alt="Go to the East Sussex County Council home page" src="{escc-logo}" width="118" height="85" class="logo-large" />
            </a>
          </div>

        </header>

        <div class="body" role="main">
          <h1>
            <xsl:value-of select="rss/channel/title"/>
          </h1>
          <p>
            You are looking at an RSS feed. To subscribe to this feed, simply copy the address of this page into your feed reader.
            You can get email alerts when this feed is updated using
            <xsl:text disable-output-escaping="yes">&lt;a href="http://blogtrottr.com/?subscribe=</xsl:text>
            <xsl:value-of select="rss/channel/item[1]/source/@url"/>
            <xsl:text disable-output-escaping="yes">&quot;&gt;BlogTrottr&lt;/a&gt;.</xsl:text>
          </p>

          <dl>
            <xsl:for-each select="rss/channel/item">
              <dt class="rss-item">
                <a>
                  <xsl:attribute name="href">
                    <xsl:value-of select="link"/>
                  </xsl:attribute>
                  <xsl:value-of select="title"/>
                </a>
              </dt>
              <dd>
                <xsl:value-of select="description" disable-output-escaping="yes"/>
                <p class="published">
                  <xsl:value-of select="pubDate"/>
                </p>
              </dd>
            </xsl:for-each>
          </dl>

          <script src="https://www.eastsussex.gov.uk/js/config-analytics.jsx"></script>
        </div>
      </body>
    </html>

  </xsl:template>
</xsl:stylesheet>