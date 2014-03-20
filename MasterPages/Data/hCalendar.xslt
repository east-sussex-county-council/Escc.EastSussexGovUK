<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>
  
  <xsl:template match="@* | node()">
      <xsl:choose>
        <!-- xhtml2vcal.xsl doesn't support the HTML5 time element, instead expecting the old inaccessible abbr pattern,
             so convert the HTML to something xhtml2vcal.xsl can process -->
        <xsl:when test="name() = 'time'">
          <xsl:element name="abbr">
            <xsl:attribute name="title">
              <xsl:value-of select="@datetime"/>
            </xsl:attribute>
            <xsl:attribute name="class">
              <xsl:value-of select="@class"/>
            </xsl:attribute>
            <xsl:apply-templates select="node()"/>
          </xsl:element>
        </xsl:when>
        <!-- Fix the output of SimpleAddressControl so that there's a comma after the county. This changes the iCalendar
             data from "East SussexBN7 1UE" to "East Sussex, BN7 1UE" -->
        <xsl:when test="name() = 'span' and @class = 'region'">
          <xsl:copy-of select="node()" />,
        </xsl:when>
        <!-- Fix the output of ClosureListControl so that "is" appears before the status. This changes the iCalendar
             data from "School nameclosed" to "School name is closed" -->
        <xsl:when test="name() = 'span' and @class = 'closed'">
          is <xsl:copy-of select="node()" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
          </xsl:copy>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:template>
</xsl:stylesheet>
