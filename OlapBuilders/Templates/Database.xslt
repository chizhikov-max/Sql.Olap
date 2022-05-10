<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="xml" indent="yes"/>
  <xsl:template match="/">
    <Create xmlns="http://schemas.microsoft.com/analysisservices/2003/engine">
      <ObjectDefinition>
        <Database xmlns:xsd       ="http://www.w3.org/2001/XMLSchema" 
                  xmlns:xsi       ="http://www.w3.org/2001/XMLSchema-instance" 
                  xmlns:ddl2      ="http://schemas.microsoft.com/analysisservices/2003/engine/2" 
                  xmlns:ddl2_2    ="http://schemas.microsoft.com/analysisservices/2003/engine/2/2" 
                  xmlns:ddl100_100="http://schemas.microsoft.com/analysisservices/2008/engine/100/100" 
                  xmlns:ddl200    ="http://schemas.microsoft.com/analysisservices/2010/engine/200" 
                  xmlns:ddl200_200="http://schemas.microsoft.com/analysisservices/2010/engine/200/200" 
                  xmlns:ddl300    ="http://schemas.microsoft.com/analysisservices/2011/engine/300" 
                  xmlns:ddl300_300="http://schemas.microsoft.com/analysisservices/2011/engine/300/300" 
                  xmlns:ddl400    ="http://schemas.microsoft.com/analysisservices/2012/engine/400" 
                  xmlns:ddl400_400="http://schemas.microsoft.com/analysisservices/2012/engine/400/400" 
                  xmlns:ddl500    ="http://schemas.microsoft.com/analysisservices/2013/engine/500" 
                  xmlns:ddl500_500="http://schemas.microsoft.com/analysisservices/2013/engine/500/500">
          <ID>
            <xsl:value-of select="BuilderSource/FullName"/>
          </ID>
          <Name>
            <xsl:value-of select="BuilderSource/FullName"/>
          </Name>
          <ddl200:CompatibilityLevel>1100</ddl200:CompatibilityLevel>
          <Language>1049</Language>
          <Collation>Cyrillic_General_CI_AS</Collation>
          <DataSourceImpersonationInfo>
            <ImpersonationMode>Default</ImpersonationMode>
          </DataSourceImpersonationInfo>
          <Dimensions>
            <xsl:value-of select="BuilderSource/Dimension" disable-output-escaping="yes"/>    
          </Dimensions>
          <Cubes>
            <xsl:value-of select="BuilderSource/Cube" disable-output-escaping="yes" />
          </Cubes>
          <DataSources>
            <xsl:value-of select="BuilderSource/DataSource" disable-output-escaping="yes" />
          </DataSources>
          <DataSourceViews>
            <xsl:value-of select="BuilderSource/View" disable-output-escaping="yes" />
          </DataSourceViews>
        </Database>
      </ObjectDefinition>
    </Create>
  </xsl:template>
</xsl:stylesheet>
