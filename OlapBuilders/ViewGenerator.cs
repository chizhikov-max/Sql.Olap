using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using Oper.Sql.Common;

namespace Oper.Sql.OlapBuilders
{
    public class ViewGenerator<T> : IXMLAGenerator
    {
        public ViewGenerator(Config<T> config)
        {
            this.config = config;
        }

        private readonly Config<T> config;

        public string Generate()
        {
            //var odbcConnection = new OdbcConnection("DSN=FB_ALL");
            //odbcConnection.Open();

            var elements = GetElements();
            var obj = new ViewObject
            {
                Name = config.UniqueName,
                FullName = $"View",
                DataSourceName = $"DataSource",
                Query = GetQuery(),
                Elements = elements
            };
            var data = obj.Serialize();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(data);

            var templatePath = $@"{config.TemplatePath}\OlapBuilders\Templates\View.xslt";
            XslCompiledTransform transform = new XslCompiledTransform();
            XsltSettings settings = new XsltSettings { EnableScript = true };
            transform.Load(templatePath, settings, null);

            using (var sw = new StringWriter())
            {
                using (var writer = new XmlTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    transform.Transform(xmlDocument, writer);

                }
                var xml = sw.ToString();
                return xml;
                //using (var connection = new AdomdConnection(config.OlapConnectionString))
                //{
                //    connection.Open();
                //    connection.Execute(viewXml, commandTimeout: 3600);
                //    //command.CommandText =
                //    //    @"SELECT *
                //    //      FROM SYSTEMRESTRICTSCHEMA (
                //    //        $SYSTEM.DISCOVER_INSTANCES,
                //    //        INSTANCE_NAME='OLAP'
                //    //      )";
                //    //AdomdRestrictionCollection restrictions = new AdomdRestrictionCollection();
                //    //restrictions.Add("DataSourceViewID", "Test666666");
                //    //var cs = connection.GetSchemaDataSet("DISCOVER_XML_METADATA", restrictions);
                //    //AdomdRestrictionCollection restrictions = new AdomdRestrictionCollection();
                //    //restrictions.Add("DISCOVER_DATASOURCES", Cube);
                //    //restrictions.Add("COORDINATE", Coordinate);
                //    //restrictions.Add("COORDINATE_TYPE", CoordinateType); //6 = Cell coordinate

                //    ////Open and return a schema rowset, given the correct restictions
                //    //var ds = connection.GetSchemaDataSet("MDSCHEMA_ACTIONS", restrictions);
                //}
            }
        }

        private string GetQuery()
        {
            var properties = typeof(T).GetProperties();
            var fields = string.Join(", ", properties.Select(x => x.Name));
            //string sql = $"select Id, MONTH(MOVEDATE) AS MONTH, YEAR(MOVEDATE) AS YEAR, DATENAME([month], MOVEDATE) AS MONTHNAME, { fields} from {config.Name} where Constract='{config.Constract.ToString("yyyyMMdd HH:mm:ss.fff")}'";
            string sql = string.Empty;
            if (properties.Any(x => x.Name == "MOVEDATE"))
                sql = $"select Id, MONTH(MOVEDATE) AS MONTH, YEAR(MOVEDATE) AS YEAR, DATENAME([month], MOVEDATE) AS MONTHNAME, { fields} from {config.UniqueName}";
            else
                sql = $"select Id, null AS MONTH, null AS YEAR, null AS MONTHNAME, { fields} from {config.UniqueName}";
            return sql;
        }
        private List<ViewElement> GetElements()
        {
            var items = new List<ViewElement>();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var element = new ViewElement { Name = property.Name, Type = ConvertXmlType(property.PropertyType) };
                items.Add(element);
            }

            return items;
        }

        private string ConvertXmlType(Type type)
        {
            if (type == typeof(int)) return "xs:int";
            if (type == typeof(DateTime)) return "xs:dateTime";
            if (type == typeof(decimal)) return "xs:decimal";
            if (type == typeof(float)) return "xs:float";
            return "xs:string";
        }
    }
}