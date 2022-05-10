using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Sql.Olap.Common;

namespace Sql.Olap.OlapBuilders
{
    public class CubeGenerator<T> : IXMLAGenerator
    {
        public CubeGenerator(Config<T> config)
        {
            this.config = config;
        }

        private readonly Config<T> config;

        public string Generate()
        {
            var dimensions = GetDimensions();
            var measures = GetMeasures();
            var obj = new CubeObject
            {
                Name = config.UniqueName,
                FullName = $"Cube",
                DimensionName = $"Dimension",
                ViewName = $"View",
                Dimensions = dimensions,
                Measures = measures
            };
            var data = obj.Serialize();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(data);
            
            var templatePath = $@"{config.TemplatePath}\OlapBuilders\Templates\Cube.xslt";
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

        private List<MeasureElement> GetMeasures()
        {
            var items = new List<MeasureElement>();
            var properties = ReflectionHelper.GetPropertiesWithAttribute(typeof(T), typeof(MeasureAttribute));
            foreach (var property in properties)
            {
                var element = new MeasureElement
                {
                    DataType = ConvertXmlType(property.PropertyType),
                    Name = property.Name,
                };
                items.Add(element);
            }
            return items;
        }

        private List<DimensionElement> GetDimensions()
        {
            var items = new List<DimensionElement>();
            var properties = ReflectionHelper.GetPropertiesWithAttribute(typeof(T), typeof(DimensionAttribute));
            foreach (var property in properties)
            {
                var element = new DimensionElement
                {
                    Key = property.Name,
                    KeyDataType = ConvertXmlType(property.PropertyType),
                    Name = property.Name,
                    NameDataType = "WChar",
                    OrderBy = "Name"
                };
                items.Add(element);
            }
            return items;
        }

        private string ConvertXmlType(Type type)
        {
            if (type == typeof(int) || type == typeof(int?)) return "Integer";
            if (type == typeof(DateTime) || type == typeof(DateTime?)) return "Date";
            if (type == typeof(decimal) || type == typeof(decimal?)) return "Double";
            if (type == typeof(float) || type == typeof(float?)) return "Double";
            return "WChar";
        }
    }
}