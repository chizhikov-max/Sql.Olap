<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl xsd xsi ddl2 ddl2_2 ddl100_100 ddl200 ddl200_200 ddl300 ddl300_300 ddl400 ddl400_400 ddl500 ddl500_500"
    xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:ddl2="http://schemas.microsoft.com/analysisservices/2003/engine/2" xmlns:ddl2_2="http://schemas.microsoft.com/analysisservices/2003/engine/2/2" xmlns:ddl100_100="http://schemas.microsoft.com/analysisservices/2008/engine/100/100" xmlns:ddl200="http://schemas.microsoft.com/analysisservices/2010/engine/200" xmlns:ddl200_200="http://schemas.microsoft.com/analysisservices/2010/engine/200/200" xmlns:ddl300="http://schemas.microsoft.com/analysisservices/2011/engine/300" xmlns:ddl300_300="http://schemas.microsoft.com/analysisservices/2011/engine/300/300" xmlns:ddl400="http://schemas.microsoft.com/analysisservices/2012/engine/400" xmlns:ddl400_400="http://schemas.microsoft.com/analysisservices/2012/engine/400/400" xmlns:ddl500="http://schemas.microsoft.com/analysisservices/2013/engine/500" xmlns:ddl500_500="http://schemas.microsoft.com/analysisservices/2013/engine/500/500"
>
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/">
    <DataSourceView>
      <ID>
        <xsl:value-of select="ViewObject/FullName"/>
      </ID>
      <Name>
        <xsl:value-of select="ViewObject/FullName"/>
      </Name>
      <Annotations>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:ShowFriendlyNames</Name>
          <Value>true</Value>
        </Annotation>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:CurrentLayout</Name>
          <Value>_ALL_TABLES_</Value>
        </Annotation>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:NameMatchingCriteria</Name>
          <Value>1</Value>
        </Annotation>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:SchemaRestriction</Name>
        </Annotation>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:RetrieveRelationships</Name>
          <Value>true</Value>
        </Annotation>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:Layouts</Name>
          <Value>
            <Layouts xmlns="">
              <Diagram>
                <Name>_ALL_TABLES_</Name>
                <DiagramLayout>
                  <dds>
                    <diagram fontclsid="{{0BE35203-8F91-11CE-9DE3-00AA004BB851}}"
                             mouseiconclsid="{{0BE35204-8F91-11CE-9DE3-00AA004BB851}}"
                             defaultlayout="MSDDS.Rectilinear"
                             defaultlineroute="MSDDS.Rectilinear" version="7"
                             nextobject="2" scale="100" pagebreakanchorx="0"
                             pagebreakanchory="0" pagebreaksizex="0" pagebreaksizey="0"
                             scrollleft="1345" scrolltop="1688" gridx="150" gridy="150" marginx="5000"
                             marginy="5000" zoom="100" x="20144" y="11624" backcolor="16119285"
                             defaultpersistence="2" PrintPageNumbersMode="3" PrintMarginTop="0"
                             PrintMarginBottom="635" PrintMarginLeft="0" PrintMarginRight="0"
                             marqueeselectionmode="0" mousepointer="0" snaptogrid="0"
                             autotypeannotation="1" showscrollbars="0" viewpagebreaks="0"
                             donotforceconnectorsbehindshapes="1"
                             backpictureclsid="{{00000000-0000-0000-0000-000000000000}}">
                      <font>
                        <ddsxmlobjectstreamwrapper binary="01cc0000900180380100065461686f6d61" />
                      </font>
                      <mouseicon>
                        <ddsxmlobjectstreamwrapper binary="6c74000000000000" />
                      </mouseicon>
                    </diagram>
                    <layoutmanager>
                      <ddsxmlobj />
                    </layoutmanager>
                    <ddscontrol controlprogid="DdsShapes.DdsObjectManagedBridge"
                                tooltip="{ViewObject/Name}" left="9786" top="5000" logicalid="1" controlid="1"
                                masterid="0" hint1="0" hint2="0" width="3263" height="5000" noresize="0"
                                nomove="0" nodefaultattachpoints="0" autodrag="1" usedefaultiddshape="1"
                                selectable="1" showselectionhandles="1" allownudging="1" isannotation="0"
                                dontautolayout="0" groupcollapsed="0" tabstop="1" visible="1" snaptogrid="0">
                      <control>
                        <ddsxmlobjectstreaminitwrapper binary="000c0000bf0c000088130000" />
                      </control>
                      <layoutobject>
                        <ddsxmlobj>
                          <property name="LogicalObject" value="{ViewObject/Name}" vartype="8" />
                        </ddsxmlobj>
                      </layoutobject>
                      <shape groupshapeid="0" groupnode="0" />
                    </ddscontrol>
                  </dds>
                </DiagramLayout>
                <ShowRelationshipNames>False</ShowRelationshipNames>
                <UseDiagramDefaultLayout>True</UseDiagramDefaultLayout>
                <DiagramViewPortLeft>1345</DiagramViewPortLeft>
                <DiagramViewPortTop>1688</DiagramViewPortTop>
                <DiagramBoundingLeft>0</DiagramBoundingLeft>
                <DiagramBoundingTop>0</DiagramBoundingTop>
                <DiagramZoom>100</DiagramZoom>
              </Diagram>
            </Layouts>
          </Value>
        </Annotation>
      </Annotations>
      <DataSourceID>
        <xsl:value-of select="ViewObject/DataSourceName"/>
      </DataSourceID>
      <Schema>
        <xs:schema id="{ViewObject/FullName}" xmlns="" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata" xmlns:msprop="urn:schemas-microsoft-com:xml-msprop">
          <xs:element name="{ViewObject/FullName}" msdata:IsDataSet="true" msdata:UseCurrentLocale="true">
            <xs:complexType>
              <xs:choice minOccurs="0" maxOccurs="unbounded">
                <xs:element name="{ViewObject/Name}" msprop:IsLogical="True" msprop:FriendlyName="{ViewObject/Name}" msprop:DbTableName="{ViewObject/Name}" msprop:TableType="View" msprop:Description=""
                            msprop:QueryDefinition="{ViewObject/Query}"
                            msprop:QueryBuilder="SpecificQueryBuilder">
                  <xs:complexType>
                    <xs:sequence>
                      <xs:element name="Id" msdata:ReadOnly="true" msprop:DbColumnName="Id" msprop:FriendlyName="Id" type="xs:int" minOccurs="0" />
                      <xs:element name="MONTH" msprop:DbColumnName="MONTH" msprop:FriendlyName="MONTH" type="xs:int" minOccurs="0" />
                      <xs:element name="YEAR" msprop:DbColumnName="YEAR" msprop:FriendlyName="YEAR" type="xs:int" minOccurs="0" />
                      <xs:element name="MONTHNAME" msprop:DbColumnName="MONTHNAME" msprop:FriendlyName="MONTHNAME" type="xs:string" minOccurs="0" />
                      <xsl:for-each select="ViewObject/Elements/ViewElement">
                        <xs:element name="{Name}" msprop:DbColumnName="{Name}" msprop:FriendlyName="{Name}" type="{Type}" minOccurs="0" />
                      </xsl:for-each>
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
              </xs:choice>
            </xs:complexType>
          </xs:element>
        </xs:schema>
        <xsl:element name="{ViewObject/FullName}" namespace="">
        </xsl:element>
      </Schema>
    </DataSourceView>
  </xsl:template>
</xsl:stylesheet>
