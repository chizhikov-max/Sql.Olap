using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Oper.Sql.Common;

namespace Oper.Sql.OlapBuilders
{
    public class DataSourceGenerator<T> : IXMLAGenerator
    {
        public DataSourceGenerator(Config<T> config)
        {
            this.config = config;
        }

        private readonly Config<T> config;

        public string Generate()
        {
            var obj = new DataSourceObject
            {
                Name = config.UniqueName,
                FullName = $"DataSource",
                ConnectionString = $"Provider=SQLNCLI11.1;{config.ConnectionString}"
            };
            var data = obj.Serialize();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(data);
            
            var templatePath = $@"{config.TemplatePath}\OlapBuilders\Templates\DataSource.xslt";
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
    }
}