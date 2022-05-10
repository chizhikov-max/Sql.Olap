<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl xsd xsi ddl2 ddl2_2 ddl100_100 ddl200 ddl200_200 ddl300 ddl300_300 ddl400 ddl400_400 ddl500 ddl500_500"
    xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:ddl2="http://schemas.microsoft.com/analysisservices/2003/engine/2" xmlns:ddl2_2="http://schemas.microsoft.com/analysisservices/2003/engine/2/2" xmlns:ddl100_100="http://schemas.microsoft.com/analysisservices/2008/engine/100/100" xmlns:ddl200="http://schemas.microsoft.com/analysisservices/2010/engine/200" xmlns:ddl200_200="http://schemas.microsoft.com/analysisservices/2010/engine/200/200" xmlns:ddl300="http://schemas.microsoft.com/analysisservices/2011/engine/300" xmlns:ddl300_300="http://schemas.microsoft.com/analysisservices/2011/engine/300/300" xmlns:ddl400="http://schemas.microsoft.com/analysisservices/2012/engine/400" xmlns:ddl400_400="http://schemas.microsoft.com/analysisservices/2012/engine/400/400" xmlns:ddl500="http://schemas.microsoft.com/analysisservices/2013/engine/500" xmlns:ddl500_500="http://schemas.microsoft.com/analysisservices/2013/engine/500/500"
>
  <xsl:output method="xml" indent="yes"/>
  <xsl:variable name="Name" select="/CubeObject/Name"/>
  <xsl:template match="/">
    <Cube>
      <ID>
        <xsl:value-of select="CubeObject/FullName"/>
      </ID>
      <Name>
        <xsl:value-of select="CubeObject/FullName"/>
      </Name>
      <Annotations>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:DiagramLayout</Name>
          <Value>
            <dds xmlns="">
              <diagram fontclsid="{{0BE35203-8F91-11CE-9DE3-00AA004BB851}}" mouseiconclsid="{{0BE35204-8F91-11CE-9DE3-00AA004BB851}}" defaultlayout="MSDDS.Rectilinear" defaultlineroute="MSDDS.Rectilinear" version="7" nextobject="2" scale="100" pagebreakanchorx="0" pagebreakanchory="0" pagebreaksizex="0" pagebreaksizey="0" scrollleft="-8032" scrolltop="-2712" gridx="150" gridy="150" marginx="5000" marginy="5000" zoom="100" x="19579" y="10425" backcolor="16119285" defaultpersistence="2" PrintPageNumbersMode="3" PrintMarginTop="0" PrintMarginBottom="635" PrintMarginLeft="0" PrintMarginRight="0" marqueeselectionmode="0" mousepointer="0" snaptogrid="0" autotypeannotation="1" showscrollbars="0" viewpagebreaks="0" donotforceconnectorsbehindshapes="1" backpictureclsid="{{00000000-0000-0000-0000-000000000000}}">
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
              <ddscontrol controlprogid="DdsShapes.DdsObjectManagedBridge" tooltip="{CubeObject/Name}" left="20" top="0" logicalid="1" controlid="1" masterid="0" hint1="0" hint2="0" width="3475" height="5000" noresize="0" nomove="0" nodefaultattachpoints="0" autodrag="1" usedefaultiddshape="1" selectable="1" showselectionhandles="1" allownudging="1" isannotation="0" dontautolayout="0" groupcollapsed="0" tabstop="1" visible="1" snaptogrid="0">
                <control>
                  <ddsxmlobjectstreaminitwrapper binary="000c0000930d000088130000" />
                </control>
                <layoutobject>
                  <ddsxmlobj>
                    <property name="LogicalObject" value="{CubeObject/Name}" vartype="8" />
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
          <Value>-8032</Value>
        </Annotation>
        <Annotation>
          <Name>http://schemas.microsoft.com/DataWarehouse/Designer/1.0:DiagramViewPortTop</Name>
          <Value>-2712</Value>
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
      <Language>1049</Language>
      <Collation>Cyrillic_General_CI_AS</Collation>
      <Dimensions>
        <Dimension>
          <ID>
            <xsl:value-of select="CubeObject/DimensionName"/>
          </ID>
          <Name>
            <xsl:value-of select="CubeObject/DimensionName"/>
          </Name>
          <DimensionID>
            <xsl:value-of select="CubeObject/DimensionName"/>
          </DimensionID>
          <Attributes>
            <Attribute>
              <AttributeID>Id</AttributeID>
            </Attribute>
            <Attribute>
              <AttributeID>MONTH</AttributeID>
            </Attribute>
            <Attribute>
              <AttributeID>YEAR</AttributeID>
            </Attribute>
            <Attribute>
              <AttributeID>MONTHNAME</AttributeID>
            </Attribute>
            <xsl:for-each select="CubeObject/Dimensions/DimensionElement">
              <Attribute>
                <AttributeID>
                  <xsl:value-of select="Name"/>
                </AttributeID>
              </Attribute>
            </xsl:for-each>
          </Attributes>
        </Dimension>
      </Dimensions>
      <MeasureGroups>
        <MeasureGroup>
          <ID>
            <xsl:value-of select="CubeObject/Name"/>
          </ID>
          <Name>
            <xsl:value-of select="CubeObject/Name"/>
          </Name>
          <Measures>
            <xsl:for-each select="CubeObject/Measures/MeasureElement">
              <Measure>
                <ID>
                  <xsl:value-of select="Name"/>
                </ID>
                <Name>
                  <xsl:value-of select="Name"/>
                </Name>
                <DataType>
                  <xsl:value-of select="DataType"/>
                </DataType>
                <Source>
                  <DataType>
                    <xsl:value-of select="DataType"/>
                  </DataType>
                  <Source xsi:type="ColumnBinding">
                    <TableID>
                      <xsl:value-of select="$Name"/>
                    </TableID>
                    <ColumnID>
                      <xsl:value-of select="Name"/>
                    </ColumnID>
                  </Source>
                </Source>
              </Measure>
            </xsl:for-each>
            <!--<Measure>
                  <ID>Abc Count</ID>
                  <Name>Abc Count</Name>
                  <AggregateFunction>Count</AggregateFunction>
                  <DataType>Integer</DataType>
                  <Source>
                    <DataType>Integer</DataType>
                    <DataSize>4</DataSize>
                    <Source xsi:type="RowBinding">
                      <TableID>Abc</TableID>
                    </Source>
                  </Source>
                </Measure>-->
          </Measures>
          <StorageMode>Molap</StorageMode>
          <ProcessingMode>Regular</ProcessingMode>
          <Dimensions>
            <Dimension xsi:type="RegularMeasureGroupDimension">
              <CubeDimensionID>
                <xsl:value-of select="CubeObject/DimensionName"/>
              </CubeDimensionID>
              <Cardinality>One</Cardinality>
              <Attributes>
                <Attribute>
                  <AttributeID>Id</AttributeID>
                  <KeyColumns>
                    <KeyColumn>
                      <DataType>Integer</DataType>
                      <Source xsi:type="ColumnBinding">
                        <TableID>
                          <xsl:value-of select="$Name"/>
                        </TableID>
                        <ColumnID>Id</ColumnID>
                      </Source>
                    </KeyColumn>
                  </KeyColumns>
                  <Type>Granularity</Type>
                </Attribute>
                <Attribute>
                  <AttributeID>MONTH</AttributeID>
                  <KeyColumns>
                    <KeyColumn>
                      <DataType>Integer</DataType>
                      <Source xsi:type="ColumnBinding">
                        <TableID>
                          <xsl:value-of select="$Name"/>
                        </TableID>
                        <ColumnID>MONTH</ColumnID>
                      </Source>
                    </KeyColumn>
                  </KeyColumns>
                </Attribute>
                <Attribute>
                  <AttributeID>YEAR</AttributeID>
                  <KeyColumns>
                    <KeyColumn>
                      <DataType>Integer</DataType>
                      <Source xsi:type="ColumnBinding">
                        <TableID>
                          <xsl:value-of select="$Name"/>
                        </TableID>
                        <ColumnID>YEAR</ColumnID>
                      </Source>
                    </KeyColumn>
                  </KeyColumns>
                </Attribute>
                <Attribute>
                  <AttributeID>MONTHNAME</AttributeID>
                  <KeyColumns>
                    <KeyColumn>
                      <DataType>Integer</DataType>
                      <Source xsi:type="ColumnBinding">
                        <TableID>
                          <xsl:value-of select="$Name"/>
                        </TableID>
                        <ColumnID>MONTH</ColumnID>
                      </Source>
                    </KeyColumn>
                  </KeyColumns>
                </Attribute>
                <xsl:for-each select="CubeObject/Dimensions/DimensionElement">
                  <Attribute>
                    <AttributeID>
                      <xsl:value-of select="Name"/>
                    </AttributeID>
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
                  </Attribute>
                </xsl:for-each>
              </Attributes>
            </Dimension>
          </Dimensions>
          <Partitions>
            <Partition>
              <ID>
                <xsl:value-of select="CubeObject/Name"/>
              </ID>
              <Name>
                <xsl:value-of select="CubeObject/Name"/>
              </Name>
              <Source xsi:type="DsvTableBinding">
                <DataSourceViewID>
                  <xsl:value-of select="CubeObject/ViewName"/>
                </DataSourceViewID>
                <TableID>
                  <xsl:value-of select="CubeObject/Name"/>
                </TableID>
              </Source>
              <StorageMode>Molap</StorageMode>
              <ProcessingMode>Regular</ProcessingMode>
              <ProactiveCaching>
                <SilenceInterval>-PT1S</SilenceInterval>
                <Latency>-PT1S</Latency>
                <SilenceOverrideInterval>-PT1S</SilenceOverrideInterval>
                <ForceRebuildInterval>-PT1S</ForceRebuildInterval>
                <Source xsi:type="ProactiveCachingInheritedBinding" />
              </ProactiveCaching>
            </Partition>
          </Partitions>
          <ProactiveCaching>
            <SilenceInterval>-PT1S</SilenceInterval>
            <Latency>-PT1S</Latency>
            <SilenceOverrideInterval>-PT1S</SilenceOverrideInterval>
            <ForceRebuildInterval>-PT1S</ForceRebuildInterval>
            <Source xsi:type="ProactiveCachingInheritedBinding" />
          </ProactiveCaching>
        </MeasureGroup>
      </MeasureGroups>
      <Source>
        <DataSourceViewID>
          <xsl:value-of select="CubeObject/ViewName"/>
        </DataSourceViewID>
      </Source>
      <ProactiveCaching>
        <SilenceInterval>-PT1S</SilenceInterval>
        <Latency>-PT1S</Latency>
        <SilenceOverrideInterval>-PT1S</SilenceOverrideInterval>
        <ForceRebuildInterval>-PT1S</ForceRebuildInterval>
        <Source xsi:type="ProactiveCachingInheritedBinding" />
      </ProactiveCaching>
    </Cube>
  </xsl:template>
</xsl:stylesheet>
