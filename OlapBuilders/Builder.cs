using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Dapper;
using Microsoft.AnalysisServices.AdomdClient;
using Oper.Sql.Common;

namespace Oper.Sql.OlapBuilders
{
    public partial class Builder<T> : IBuilder
    {
        public Builder(Config<T> config)
        {
            this.config = config;

            dataSourceGenerator = new DataSourceGenerator<T>(config);
            viewGenerator = new ViewGenerator<T>(config);
            dimensionGenerator = new DimensionGenerator<T>(config);
            cubeGenerator = new CubeGenerator<T>(config);
        }

        private readonly Config<T> config;
        private readonly IXMLAGenerator dataSourceGenerator;
        private readonly IXMLAGenerator viewGenerator;
        private readonly IXMLAGenerator dimensionGenerator;
        private readonly IXMLAGenerator cubeGenerator;

        public void GenerateXMLA()
        {
            string dataSourceXMLA = dataSourceGenerator.Generate();
            string viewXMLA = viewGenerator.Generate();
            string dimensionXMLA = dimensionGenerator.Generate();
            string cubeXMLA = cubeGenerator.Generate();
            var source = new BuilderSource
            {
                FullName = config.UniqueName,
                DataSource = dataSourceXMLA,
                View = viewXMLA,
                Dimension = dimensionXMLA,
                Cube = cubeXMLA
            };
            var data = source.Serialize();
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(data);
            var templatePath = $@"{config.TemplatePath}\OlapBuilders\Templates\Database.xslt";
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
                using (var connection = new AdomdConnection(config.OlapConnectionString))
                {
                    connection.Open();
                    connection.Execute(xml, commandTimeout: 3600);
                }
            }
        }

        public void ClearObsoleteOlapDatabase()
        {
            try
            {
                DeleteOlapDatabase(config.UniqueName);
            }
            catch 
            {
                
            }
            
            using (var connection = new AdomdConnection("Data Source=localhost"))
            {
                connection.Open();
                var command = new AdomdCommand("SELECT [CATALOG_NAME] FROM $system.dbschema_catalogs", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var name = reader.GetString(0);
                    var parts = name.Split(new[] {'_'}, StringSplitOptions.RemoveEmptyEntries);
                    var length = parts.Length;
                    if (length > 3)
                    {
                        string dateStr = $"{parts[length - 3]}_{parts[length - 2]}_{parts[length - 1]}";
                        DateTime date = DateTime.Now;
                        if (DateTime.TryParseExact(dateStr, "yyyyMMdd_HHmmss_fff", null, DateTimeStyles.None,
                                out date) &&
                            (DateTime.Now - date).TotalMinutes > config.ActiveDataTimeout)
                        {
                            DeleteOlapDatabase(name);
                        }
                    }
                }
            }
        }

        private void DeleteOlapDatabase(string name)
        {
            using (var connection = new AdomdConnection("Data Source=localhost"))
            {
                var query = $@"
                    <Delete xmlns=""http://schemas.microsoft.com/analysisservices/2003/engine"">
                      <Object>
                        <DatabaseID>{name}</DatabaseID>
                      </Object>
                    </Delete>";
                connection.Execute(query);
            }
        }

        public void Process()
        {
            using (var connection = new AdomdConnection(config.OlapConnectionString))
            {
                connection.Open();
                string processCommand =
                    $@"<Batch xmlns=""http://schemas.microsoft.com/analysisservices/2003/engine"">
                          <Parallel>
                            <Process xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:ddl2=""http://schemas.microsoft.com/analysisservices/2003/engine/2"" xmlns:ddl2_2=""http://schemas.microsoft.com/analysisservices/2003/engine/2/2"" xmlns:ddl100_100=""http://schemas.microsoft.com/analysisservices/2008/engine/100/100"" xmlns:ddl200=""http://schemas.microsoft.com/analysisservices/2010/engine/200"" xmlns:ddl200_200=""http://schemas.microsoft.com/analysisservices/2010/engine/200/200"" xmlns:ddl300=""http://schemas.microsoft.com/analysisservices/2011/engine/300"" xmlns:ddl300_300=""http://schemas.microsoft.com/analysisservices/2011/engine/300/300"" xmlns:ddl400=""http://schemas.microsoft.com/analysisservices/2012/engine/400"" xmlns:ddl400_400=""http://schemas.microsoft.com/analysisservices/2012/engine/400/400"" xmlns:ddl500=""http://schemas.microsoft.com/analysisservices/2013/engine/500"" xmlns:ddl500_500=""http://schemas.microsoft.com/analysisservices/2013/engine/500/500"">
                              <Object>
                                <DatabaseID>{config.UniqueName}</DatabaseID>
                              </Object>
                              <Type>ProcessFull</Type>
                              <WriteBackTableCreation>UseExisting</WriteBackTableCreation>
                            </Process>
                          </Parallel>
                        </Batch>";
                connection.Execute(processCommand, commandTimeout: 3600);
            }
        }
    }
}