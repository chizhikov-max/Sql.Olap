using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using Sql.Olap.Common;

namespace Sql.Olap.OlapBuilders
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
            }
        }

        private string GetQuery()
        {
            var properties = typeof(T).GetProperties();
            var fields = string.Join(", ", properties.Select(x => x.Name));
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