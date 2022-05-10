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
                //using (var connection = new AdomdConnection(config.OlapConnectionString))
                //{
                //    connection.Open();
                //    connection.Execute(viewXml, commandTimeout: 3600);
                //}
            }
        }

        //        public void Process()
        //        {
        //            string fullName = $"Cube_{config.UniqueName}";
        //            using (var connection = new AdomdConnection(config.OlapConnectionString))
        //            {
        //                connection.Open();
        //                string processCommand =
        //                    $@"<Batch xmlns=""http://schemas.microsoft.com/analysisservices/2003/engine"">
        //	<Parallel>
        //		<Process xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:ddl2=""http://schemas.microsoft.com/analysisservices/2003/engine/2"" xmlns:ddl2_2=""http://schemas.microsoft.com/analysisservices/2003/engine/2/2"" xmlns:ddl100_100=""http://schemas.microsoft.com/analysisservices/2008/engine/100/100"" xmlns:ddl200=""http://schemas.microsoft.com/analysisservices/2010/engine/200"" xmlns:ddl200_200=""http://schemas.microsoft.com/analysisservices/2010/engine/200/200"" xmlns:ddl300=""http://schemas.microsoft.com/analysisservices/2011/engine/300"" xmlns:ddl300_300=""http://schemas.microsoft.com/analysisservices/2011/engine/300/300"" xmlns:ddl400=""http://schemas.microsoft.com/analysisservices/2012/engine/400"" xmlns:ddl400_400=""http://schemas.microsoft.com/analysisservices/2012/engine/400/400"" xmlns:ddl500=""http://schemas.microsoft.com/analysisservices/2013/engine/500"" xmlns:ddl500_500=""http://schemas.microsoft.com/analysisservices/2013/engine/500/500"">
        //			<Object>
        //				<DatabaseID>OLAP</DatabaseID>
        //				<CubeID>{fullName}</CubeID>
        //			</Object>
        //			<Type>ProcessFull</Type>
        //			<WriteBackTableCreation>UseExisting</WriteBackTableCreation>
        //		</Process>
        //	</Parallel>
        //</Batch>";
        //                connection.Execute(processCommand, commandTimeout: 3600);
        //            }
        //        }

        private List<MeasureElement> GetMeasures()
        {
            var items = new List<MeasureElement>();
            var properties = ReflectionHelper.GetPropertiesWithAttribute(typeof(T), typeof(MeasureAttribute));
            foreach (var property in properties)
            {
                var element = new MeasureElement
                {
                    //Key = property.Name,
                    DataType = ConvertXmlType(property.PropertyType),
                    Name = property.Name,
                    //NameDataType = "WChar",
                    //OrderBy = "Name"
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