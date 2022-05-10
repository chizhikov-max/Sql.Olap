using System;

namespace Sql.Olap.Models
{
    public class Firm
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Character_Set { get; set; }
        public string Pass { get; set; }
        public string UserName { get; set; }
        public string DatabaseName { get; set; }
        public string Port { get; set; }
        public string Server { get; set; }
        public string Code { get; set; }
        public int OK { get; set; }
        public int AL { get; set; }

        public string ConnectionString => $"character set={Character_Set};port number={Port};data source={Server};initial catalog={DatabaseName};user id={UserName};password={Pass};Pooling=false;";
        public DateTime? DynamicCalculatedDate { get; set; }
        public DateTime? ActualDate { get; set; }
    }
}