<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
    xmlns:atom="http://www.w3.org/2005/Atom"
>
  <!-- Include processing instructions for browsers to display the RSS feed nicely, rather than as raw XML -->
  <xsl:template name="HtmlFormatting">
    <xsl:param name="CurrentUrl" />

    <!-- Only use an absolute URL when on the public site, because it doesn't work otherwise (proxy?) -->
    <xsl:choose>
      <xsl:when test="contains($CurrentUrl, 'eastsussex.gov.uk')">
        <xsl:text disable-output-escaping="yes">&lt;?xml-stylesheet  type="text/xsl" href="https://new.eastsussex.gov.uk/masterpages/rss/display-as-html-v2.xslt" ?&gt;
          &lt;?xml-stylesheet  type="text/css" href="https://new.eastsussex.gov.uk/escc.eastsussexgovuk/css/rssfeed.cssx" ?&gt;</xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text disable-output-escaping="yes">&lt;?xml-stylesheet  type="text/xsl" href="/masterpages/rss/display-as-html-v2.xslt" ?&gt;
          &lt;?xml-stylesheet  type="text/css" href="/masterpages/rss.css" ?&gt;</xsl:text>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- Use a consistent header style for all RSS feeds -->
  <xsl:template name="ChannelHeader">
    <xsl:param name="Title" />
    <xsl:param name="Description" />
    <xsl:param name="Rfc822Date" />
    <xsl:param name="HtmlVersionUrl" />
    <xsl:param name="CurrentUrl" />

    <title>
      <xsl:value-of select="$Title"/>
    </title>
    <description>
      <xsl:value-of select="$Description"/>
    </description>
    <link>
      <xsl:value-of select="$HtmlVersionUrl"/>
    </link>
    <language>en-GB</language>
    <copyright>
      <xsl:value-of select="substring($Rfc822Date,13,4)"  />
      <xsl:text> East Sussex County Council</xsl:text>
    </copyright>
    <pubDate>
      <xsl:value-of select="$Rfc822Date"/>
    </pubDate>
    <lastBuildDate>
      <xsl:value-of select="$Rfc822Date"/>
    </lastBuildDate>
    <generator>East Sussex County Council at www.eastsussex.gov.uk</generator>
    <image>
      <title>East Sussex County Council logo</title>
      <url>https://new.eastsussex.gov.uk/masterpages/rss/escc-logo.gif</url>
      <width>90</width>
      <height>65</height>
      <link>
        <xsl:value-of select="$HtmlVersionUrl"/>
      </link>
    </image>

    <!-- Add the URL of the current feed -->
    <xsl:if test="$CurrentUrl != ''">
      <atom:link rel="self" type="application/rss+xml">
        <xsl:attribute xmlns="http://www.w3.org/2001/XMLSchema" name="href">
          <xsl:value-of select="$CurrentUrl"/>
        </xsl:attribute>
      </atom:link>
    </xsl:if>
  </xsl:template>
  
</xsl:stylesheet>
