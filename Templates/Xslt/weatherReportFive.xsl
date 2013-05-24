<?xml version="1.0" encoding="ISO-8859-1"?>

<!-- this xslt is now longer required, however its left in the module incase it maybe
useful in the future. 

the rss feed no longer contain a 5 day forecast. Change for version 1.4 8/5/2008
 -->

<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:yweather="http://xml.weather.yahoo.com/ns/rss/1.0" xmlns:geo="http://www.w3.org/2003/01/geo/wgs84_pos#">
<xsl:output method="html" indent="yes"/>

<xsl:template match="/">

<xsl:variable name="scale">
	<xsl:value-of select="rss/channel/yweather:units/@temperature"/>
</xsl:variable>

<table width="100%" border="0" cellspacing="0" cellpadding="3" class="Normal">
  <tr bgcolor="#CCCCCC"> 
    <td colspan="2"><strong>Weather Report - <xsl:value-of select="rss/channel/item/title"/></strong></td>
  </tr>
  <tr> 
    <td><strong>Currently <font size="4"><xsl:value-of select="rss/channel/item/yweather:condition/@temp"/><xsl:text>&#176;</xsl:text><xsl:copy-of select="$scale" /></font></strong><br/>
      High <xsl:value-of select="rss/channel/item/yweather:forecast/@high"/> <xsl:text>&#176;</xsl:text><xsl:copy-of select="$scale" /> <br/>
      Low <xsl:value-of select="rss/channel/item/yweather:forecast/@low"/> <xsl:text>&#176;</xsl:text><xsl:copy-of select="$scale" /> </td>
    <td>
	  <xsl:text disable-output-escaping="yes">&lt;img src="http://us.i1.yimg.com/us.yimg.com/i/us/we/52/</xsl:text>
	  <xsl:value-of select="rss/channel/item/yweather:condition/@code"/>
	  <xsl:text disable-output-escaping="yes">.gif"/&gt;</xsl:text>
	  <br/>
      <xsl:value-of select="rss/channel/item/yweather:condition/@text"/></td>
  </tr>
  <tr bgcolor="#CCCCCC"> 
    <td colspan="2"><strong>5 Day Forecast</strong></td>
  </tr>
  <tr> 
    <td colspan="2"><table width="100%" border="0" cellspacing="0" cellpadding="3" class="Normal">
      <xsl:for-each select="(rss/channel/item/yweather:forecast)[position() &lt; 6]">
        <tr>
          <td><xsl:value-of select="@day"/></td>
          <td>
			<xsl:text disable-output-escaping="yes">&lt;img src="http://us.i1.yimg.com/us.yimg.com/i/us/we/52/</xsl:text>
		<xsl:value-of select="@code"/>
		<xsl:text disable-output-escaping="yes">.gif"/&gt;</xsl:text>
          </td>
          <td><xsl:value-of select="@text"/><br/>High: <xsl:value-of select="@high"/>
			<xsl:text>&#176;</xsl:text><xsl:copy-of select="$scale" /> 
			 Low: <xsl:value-of select="@low"/><xsl:text>&#176;</xsl:text><xsl:copy-of select="$scale" /> 
		  </td>
        </tr>
       </xsl:for-each>
      </table></td>
  </tr>
  <tr bgcolor="#CCCCCC"> 
    <td colspan="2"><strong>More Current Condition Details</strong></td>
  </tr>
  <tr> 
    <td colspan="2"><table border="0" cellspacing="0" cellpadding="3" class="Normal">
        <tr> 
          <td>High </td>
          <td><xsl:value-of select="rss/channel/item/yweather:forecast/@high"/> <xsl:text>&#176;</xsl:text><xsl:copy-of select="$scale" /></td>
        </tr>
        <tr> 
          <td>Low </td>
          <td><xsl:value-of select="rss/channel/item/yweather:forecast/@low"/> <xsl:text>&#176;</xsl:text><xsl:copy-of select="$scale" /></td>
        </tr>
        <tr> 
          <td>Wind Chill </td>
          <td><xsl:value-of select="rss/channel/yweather:wind/@chill"/> <xsl:text>&#176;</xsl:text><xsl:copy-of select="$scale" /></td>
        </tr>
        <tr> 
          <td>Wind Speed </td>
          <td>
			<xsl:value-of select="rss/channel/yweather:wind/@speed"/>
			<xsl:text> </xsl:text>
			<xsl:value-of select="rss/channel/yweather:units/@speed"/>
          </td>
        </tr>
        <tr> 
          <td>Wind Direction </td>
          <td>
			<xsl:value-of select="rss/channel/yweather:wind/@direction"/>
          </td>
        </tr>
        <tr> 
          <td>Sunrise </td>
          <td>
			<xsl:value-of select="rss/channel/yweather:astronomy/@sunrise"/>
		  </td>
        </tr>
        <tr> 
          <td>Sunset </td>
          <td>
			<xsl:value-of select="rss/channel/yweather:astronomy/@sunset"/>
          </td>
        </tr>
        <tr> 
          <td>Latitude </td>
          <td><xsl:value-of select="rss/channel/item/geo:lat"/></td>
        </tr>
        <tr> 
          <td>Longitude </td>
          <td><xsl:value-of select="rss/channel/item/geo:long"/></td>
        </tr>
        <tr>
          <td>
            <p><xsl:text> </xsl:text></p>
            <p>
              <a href="http://weather.yahoo.com" target="_blank">Powered by Yahoo Weather</a>
            </p>
          </td>
        </tr>
      </table></td>
  </tr>
</table>

</xsl:template>

</xsl:stylesheet>