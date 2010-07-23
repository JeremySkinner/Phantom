<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
	<xsl:output method="html" indent="no" />

	<xsl:template match="/">
		<xsl:for-each select="/someData/someRecord">
			<div>
				<xsl:value-of select="./@foo"/>
			</div>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>