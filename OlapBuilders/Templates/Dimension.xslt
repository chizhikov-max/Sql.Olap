<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl xsd xsi ddl2 ddl2_2 ddl100_100 ddl200 ddl200_200 ddl300 ddl300_300 ddl400 ddl400_400 ddl500 ddl500_500"
    xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:ddl2="http://schemas.microsoft.com/analysisservices/2003/engine/2" xmlns:ddl2_2="http://schemas.microsoft.com/analysisservices/2003/engine/2/2" xmlns:ddl100_100="http://schemas.microsoft.com/analysisservices/2008/engine/100/100" xmlns:ddl200="http://schemas.microsoft.com/analysisservices/2010/engine/200" xmlns:ddl200_200="http://schemas.microsoft.com/analysisservices/2010/engine/200/200" xmlns:ddl300="http://schemas.microsoft.com/analysisservices/2011/engine/300" xmlns:ddl300_300="http://schemas.microsoft.com/analysisservices/2011/engine/300/300" xmlns:ddl400="http://schemas.microsoft.com/analysisservices/2012/engine/400" xmlns:ddl400_400="http://schemas.microsoft.com/analysisservices/2012/engine/400/400" xmlns:ddl500="http://schemas.microsoft.com/analysisservices/2013/engine/500" xmlns:ddl500_500="http://schemas.microsoft.com/analysisservices/2013/engine/500/500"
>
  <xsl:output method="xml" indent="yes"/>
  <!--<xsl:variable name="ViewName" select="/DimensionObject/ViewName"/>-->
  <xsl:variable name="Name" select="/DimensionObject/Name"/>
  <xsl:template match="/">
    <Dimension>
      <ID>
        <xsl:value-of select="DimensionObject/FullName"/>
      </ID>
      <Name>
        <xsl:value-of select="DimensionObject/FullName"/>
      </Name>
      <Annotations>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:DiagramLayout</Name>
          <Value>
            <dds xmlns="">
              <diagram fontclsid="{{0BE35203-8F91-11CE-9DE3-00AA004BB851}}" mouseiconclsid="{{0BE35204-8F91-11CE-9DE3-00AA004BB851}}" defaultlayout="MSDDS.Rectilinear" defaultlineroute="MSDDS.Rectilinear" version="7" nextobject="3" scale="100" pagebreakanchorx="0" pagebreakanchory="0" pagebreaksizex="0" pagebreaksizey="0" scrollleft="-2000" scrolltop="-3030" gridx="150" gridy="150" marginx="5000" marginy="5000" zoom="100" x="7514" y="11060" backcolor="16119285" defaultpersistence="2" PrintPageNumbersMode="3" PrintMarginTop="0" PrintMarginBottom="635" PrintMarginLeft="0" PrintMarginRight="0" marqueeselectionmode="0" mousepointer="0" snaptogrid="0" autotypeannotation="1" showscrollbars="0" viewpagebreaks="0" donotforceconnectorsbehindshapes="1" backpictureclsid="{{00000000-0000-0000-0000-000000000000}}">
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
              <ddscontrol controlprogid="DdsShapes.DdsObjectManagedBridge" tooltip="{DimensionObject/Name}" left="20" top="0" logicalid="2" controlid="1" masterid="0" hint1="0" hint2="0" width="3475" height="5000" noresize="0" nomove="0" nodefaultattachpoints="0" autodrag="1" usedefaultiddshape="1" selectable="1" showselectionhandles="1" allownudging="1" isannotation="0" dontautolayout="0" groupcollapsed="0" tabstop="1" visible="1" snaptogrid="0">
                <control>
                  <ddsxmlobjectstreaminitwrapper binary="000c0000930d000088130000" />
                </control>
                <layoutobject>
                  <ddsxmlobj>
                    <property name="LogicalObject" value="{DimensionObject/Name}" vartype="8" />
                  </ddsxmlobj>
                </layoutobject>
                <shape groupshapeid="0" groupnode="0" />
              </ddscontrol>
            </dds>
          </Value>
        </Annotation>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:ShowFriendlyNames</Name>
          <Value>true</Value>
        </Annotation>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:ShowRelationshipNames</Name>
          <Value>false</Value>
        </Annotation>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:UseDiagramDefaultLayout</Name>
          <Value>true</Value>
        </Annotation>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:DiagramViewPortLeft</Name>
          <Value>-2000</Value>
        </Annotation>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:DiagramViewPortTop</Name>
          <Value>-3030</Value>
        </Annotation>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:DiagramBoundingLeft</Name>
          <Value>20</Value>
        </Annotation>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:DiagramBoundingTop</Name>
          <Value>0</Value>
        </Annotation>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:DiagramZoom</Name>
          <Value>100</Value>
        </Annotation>
      </Annotations>
      <Source xsi:type="DataSourceViewBinding">
        <DataSourceViewID>
          <xsl:value-of select="DimensionObject/ViewName"/>
        </DataSourceViewID>
      </Source>
      <ErrorConfiguration>
        <KeyNotFound>ReportAndStop</KeyNotFound>
        <KeyDuplicate>ReportAndStop</KeyDuplicate>
        <NullKeyNotAllowed>ReportAndStop</NullKeyNotAllowed>
      </ErrorConfiguration>
      <Language>1049</Language>
      <Collation>Cyrillic_General_CI_AS</Collation>
      <UnknownMemberName>Unknown</UnknownMemberName>
      <Attributes>
        <Attribute>
          <ID>Id</ID>
          <Name>Id</Name>
          <Usage>Key</Usage>
          <KeyColumns>
            <KeyColumn>
              <DataType>Integer</DataType>
              <Source xsi:type="ColumnBinding">
                <TableID>
                  <xsl:value-of select="DimensionObject/Name"/>
                </TableID>
                <ColumnID>Id</ColumnID>
              </Source>
            </KeyColumn>
          </KeyColumns>
          <NameColumn>
            <DataType>WChar</DataType>
            <Source xsi:type="ColumnBinding">
              <TableID>
                <xsl:value-of select="DimensionObject/Name"/>
              </TableID>
              <ColumnID>Id</ColumnID>
            </Source>
          </NameColumn>
        </Attribute>
        <Attribute>
          <ID>YEAR</ID>
          <Name>YEAR</Name>
          <KeyColumns>
            <KeyColumn>
              <DataType>Integer</DataType>
              <Source xsi:type="ColumnBinding">
                <TableID>
                  <xsl:value-of select="DimensionObject/Name"/>
                </TableID>
                <ColumnID>YEAR</ColumnID>
              </Source>
            </KeyColumn>
          </KeyColumns>
          <NameColumn>
            <DataType>WChar</DataType>
            <Source xsi:type="ColumnBinding">
              <TableID>
                <xsl:value-of select="DimensionObject/Name"/>
              </TableID>
              <ColumnID>YEAR</ColumnID>
            </Source>
          </NameColumn>
        </Attribute>
        <Attribute>
          <ID>MONTH</ID>
          <Name>MONTH</Name>
          <KeyColumns>
            <KeyColumn>
              <DataType>Integer</DataType>
              <Source xsi:type="ColumnBinding">
                <TableID>
                  <xsl:value-of select="DimensionObject/Name"/>
                </TableID>
                <ColumnID>MONTH</ColumnID>
              </Source>
            </KeyColumn>
          </KeyColumns>
          <NameColumn>
            <DataType>WChar</DataType>
            <Source xsi:type="ColumnBinding">
              <TableID>
                <xsl:value-of select="DimensionObject/Name"/>
              </TableID>
              <ColumnID>MONTH</ColumnID>
            </Source>
          </NameColumn>
        </Attribute>
        <Attribute>
          <ID>MONTHNAME</ID>
          <Name>MONTHNAME</Name>
          <KeyColumns>
            <KeyColumn>
              <DataType>Integer</DataType>
              <Source xsi:type="ColumnBinding">
                <TableID>
                  <xsl:value-of select="DimensionObject/Name"/>
                </TableID>
                <ColumnID>MONTH</ColumnID>
              </Source>
            </KeyColumn>
          </KeyColumns>
          <NameColumn>
            <DataType>WChar</DataType>
            <Source xsi:type="ColumnBinding">
              <TableID>
                <xsl:value-of select="DimensionObject/Name"/>
              </TableID>
              <ColumnID>MONTHNAME</ColumnID>
            </Source>
          </NameColumn>
          <OrderBy>Key</OrderBy>
        </Attribute>

        <xsl:for-each select="DimensionObject/Elements/DimensionElement">
          <Attribute>
            <ID>
              <xsl:value-of select="Name"/>
            </ID>
            <Name>
              <xsl:value-of select="Name"/>
            </Name>
            <KeyColumns>
              <KeyColumn>
                <DataType>
                  <xsl:value-of select="KeyDataType"/>
                </DataType>
                <Source xsi:type="ColumnBinding">
                  <TableID>
                    <xsl:value-of select="$Name"/>
                  </TableID>
                  <ColumnID>
                    <xsl:value-of select="Name"/>
                  </ColumnID>
                </Source>
              </KeyColumn>
            </KeyColumns>
            <NameColumn>
              <DataType>
                <xsl:value-of select="NameDataType"/>
              </DataType>
              <Source xsi:type="ColumnBinding">
                <TableID>
                  <xsl:value-of select="$Name"/>
                </TableID>
                <ColumnID>
                  <xsl:value-of select="Name"/>
                </ColumnID>
              </Source>
            </NameColumn>
          </Attribute>
        </xsl:for-each>
      </Attributes>
      <ProactiveCaching>
        <SilenceInterval>-PT1S</SilenceInterval>
        <Latency>-PT1S</Latency>
        <SilenceOverrideInterval>-PT1S</SilenceOverrideInterval>
        <ForceRebuildInterval>-PT1S</ForceRebuildInterval>
        <Source xsi:type="ProactiveCachingInheritedBinding" />
      </ProactiveCaching>
    </Dimension>
  </xsl:template>
</xsl:stylesheet>
