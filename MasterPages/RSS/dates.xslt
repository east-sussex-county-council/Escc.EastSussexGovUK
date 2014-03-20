<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>

  <!-- When presented with a machine-readable date, translate it into a human readable date matching our house style-->
  <xsl:template name="HouseStyleDate">
    <xsl:param name="Date" />
    <xsl:choose>
      <xsl:when test="substring($Date,9,1) = '0'">
        <xsl:value-of select="substring($Date,10,1)" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="substring($Date,9,2)" />
      </xsl:otherwise>
    </xsl:choose>
    <xsl:text> </xsl:text>
    <xsl:choose>
      <xsl:when test="substring($Date,6,2) = '01'">January</xsl:when>
      <xsl:when test="substring($Date,6,2) = '02'">February</xsl:when>
      <xsl:when test="substring($Date,6,2) = '03'">March</xsl:when>
      <xsl:when test="substring($Date,6,2) = '04'">April</xsl:when>
      <xsl:when test="substring($Date,6,2) = '05'">May</xsl:when>
      <xsl:when test="substring($Date,6,2) = '06'">June</xsl:when>
      <xsl:when test="substring($Date,6,2) = '07'">July</xsl:when>
      <xsl:when test="substring($Date,6,2) = '08'">August</xsl:when>
      <xsl:when test="substring($Date,6,2) = '09'">September</xsl:when>
      <xsl:when test="substring($Date,6,2) = '10'">October</xsl:when>
      <xsl:when test="substring($Date,6,2) = '11'">November</xsl:when>
      <xsl:when test="substring($Date,6,2) = '12'">December</xsl:when>
    </xsl:choose>
    <xsl:text> </xsl:text>
    <xsl:value-of select="substring($Date,1,4)"/>
  </xsl:template>
  
  
  <!-- When presented with a machine-readable date, translate it into a human readable time matching our house style-->
  <xsl:template name="HouseStyleTime">
    <xsl:param name="Date" />

    <!-- Put the hour and minutes into variables we can work with -->
    <xsl:variable name="hour">
      <xsl:value-of select="substring($Date,12,2)" />
    </xsl:variable>

    <xsl:variable name="minutes">
      <xsl:value-of select="substring($Date,15,2)" />
    </xsl:variable>

    <xsl:choose>
      <!-- First deal with special cases of midnight and midday -->
      <xsl:when test="$hour = '00' and $minutes = '00'">12 midnight </xsl:when>
      <xsl:when test="$hour = '12' and $minutes = '00'">12 noon</xsl:when>
      <xsl:otherwise>

        <!-- Any other time: convert hour to 12-hour clock -->
        <xsl:choose>
          <xsl:when test="$hour = '00'">12</xsl:when>
          <xsl:when test="$hour &gt; 12">
            <xsl:value-of select="$hour - 12"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$hour"/>
          </xsl:otherwise>
        </xsl:choose>

        <!-- Display minutes only if they're not 00 -->
        <xsl:choose>
          <xsl:when test="$minutes != '00'">
            <xsl:text>.</xsl:text>
            <xsl:value-of select="$minutes" />
          </xsl:when>
        </xsl:choose>

        <!-- Display am or pm -->
        <xsl:choose>
          <xsl:when test="$hour &lt;= 12">am</xsl:when>
          <xsl:otherwise>pm</xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <!-- When presented with a ISO8601 date, translate it into an RFC822 date-->
  <xsl:template name="ISO8601DatetoRFC822Date">
    <xsl:param name="Date" />
    <xsl:choose>
      <xsl:when test="substring($Date,9,1) = '0'">
        <xsl:value-of select="substring($Date,10,1)" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="substring($Date,9,2)" />
      </xsl:otherwise>
    </xsl:choose>
    <xsl:text> </xsl:text>
    <xsl:choose>
      <xsl:when test="substring($Date,6,2) = '01'">Jan</xsl:when>
      <xsl:when test="substring($Date,6,2) = '02'">Feb</xsl:when>
      <xsl:when test="substring($Date,6,2) = '03'">Mar</xsl:when>
      <xsl:when test="substring($Date,6,2) = '04'">Apr</xsl:when>
      <xsl:when test="substring($Date,6,2) = '05'">May</xsl:when>
      <xsl:when test="substring($Date,6,2) = '06'">Jun</xsl:when>
      <xsl:when test="substring($Date,6,2) = '07'">Jul</xsl:when>
      <xsl:when test="substring($Date,6,2) = '08'">Aug</xsl:when>
      <xsl:when test="substring($Date,6,2) = '09'">Sep</xsl:when>
      <xsl:when test="substring($Date,6,2) = '10'">Oct</xsl:when>
      <xsl:when test="substring($Date,6,2) = '11'">Nov</xsl:when>
      <xsl:when test="substring($Date,6,2) = '12'">Dec</xsl:when>
    </xsl:choose>
    <xsl:text> </xsl:text>
    <xsl:value-of select="substring($Date,1,4)"/>
    <xsl:text> </xsl:text>
    <xsl:value-of select="substring($Date,12,8)" />
    <xsl:text> UT</xsl:text>
  </xsl:template>
</xsl:stylesheet>
