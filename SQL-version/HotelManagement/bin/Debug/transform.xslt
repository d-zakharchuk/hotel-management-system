<?xml version="1.0" encoding="windows-1251" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"> 
<xsl:template match="/">  
<xsl:for-each select="Hotel/HotelInvoice1">
   <BODY>
   <p><H2>Invoice for room #<xsl:value-of select="room_num"/></H2></p> 
   </BODY>
</xsl:for-each>
<table border="0" style="width=100%">
<tr>
<td width="30%">
	<xsl:for-each select="Hotel/HotelInvoice1">
	<table border="4">
	<tr><td>Room Num</td><td align="center"><xsl:value-of select="room_num"/></td></tr>
	<tr><td>Room Type</td><td align="center"><xsl:value-of select="room_type"/></td></tr>
	<tr><td>Room Floor</td><td align="center"><xsl:value-of select="room_floor"/></td></tr>
	<tr><td>Residents</td><td align="center"><xsl:value-of select="residents"/></td></tr>
	<tr><td>Date in</td><td align="center"><xsl:value-of select="date_in"/></td></tr>
	<tr><td>Date out</td><td align="center"><xsl:value-of select="date_out"/></td></tr>
	<tr><td>Room Cost per day</td><td align="center"><xsl:value-of select="price1"/></td></tr>
	<tr><td>Room Total Cost</td><td align="center"><xsl:value-of select="price_t"/></td></tr>
	<tr><td>Services Cost per day</td><td align="center"><xsl:value-of select="serv_day"/></td></tr>
	<tr><td>Total Cost of Services</td><td align="center"><xsl:value-of select="serv_total"/></td></tr>
	<tr><td>TOTAL</td><td align="center"><xsl:value-of select="total"/></td></tr>
	</table>  
	</xsl:for-each>
</td>
<td valign ="top" width="70%">
	<p><H3>Services Info</H3></p>
	<table border="4">
	<tr><td>First name</td><td>Middle name</td><td>Last name</td><td>Cost</td></tr>
	<xsl:for-each select="Hotel/HotelInvoice2/Resident">
	<tr>
	<td><xsl:value-of select="first_name"/></td>
	<td><xsl:value-of select="middle_name"/></td>
	<td><xsl:value-of select="last_name"/></td>
	<td allign="center"><xsl:value-of select="cost"/></td>
	</tr>
	</xsl:for-each>
	</table>
</td>
</tr>
</table>
</xsl:template>
</xsl:stylesheet>
