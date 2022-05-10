<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl xsd xsi ddl2 ddl2_2 ddl100_100 ddl200 ddl200_200 ddl300 ddl300_300 ddl400 ddl400_400 ddl500 ddl500_500"
    xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:ddl2="http://schemas.microsoft.com/analysisservices/2003/engine/2" xmlns:ddl2_2="http://schemas.microsoft.com/analysisservices/2003/engine/2/2" xmlns:ddl100_100="http://schemas.microsoft.com/analysisservices/2008/engine/100/100" xmlns:ddl200="http://schemas.microsoft.com/analysisservices/2010/engine/200" xmlns:ddl200_200="http://schemas.microsoft.com/analysisservices/2010/engine/200/200" xmlns:ddl300="http://schemas.microsoft.com/analysisservices/2011/engine/300" xmlns:ddl300_300="http://schemas.microsoft.com/analysisservices/2011/engine/300/300" xmlns:ddl400="http://schemas.microsoft.com/analysisservices/2012/engine/400" xmlns:ddl400_400="http://schemas.microsoft.com/analysisservices/2012/engine/400/400" xmlns:ddl500="http://schemas.microsoft.com/analysisservices/2013/engine/500" xmlns:ddl500_500="http://schemas.microsoft.com/analysisservices/2013/engine/500/500"
>
  <xsl:output method="xml" indent="yes"/>
  <xsl:template match="/">
    <DataSource xsi:type="RelationalDataSource">
      <ID>
        <xsl:value-of select="DataSourceObject/FullName"/>
      </ID>
      <Name>
        <xsl:value-of select="DataSourceObject/FullName"/>
      </Name>
      <ConnectionString>
        <xsl:value-of select="DataSourceObject/ConnectionString"/>
      </ConnectionString>
      <ImpersonationInfo>
        <ImpersonationMode>ImpersonateServiceAccount</ImpersonationMode>
      </ImpersonationInfo>
      <Timeout>PT0S</Timeout>
    </DataSource>
  </xsl:template>
</xsl:stylesheet>
