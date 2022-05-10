```c#
    public class Item
    {
        [Dimension]
        public int Dimension1 { get; set; }

        [Dimension]
        public string Dimension2 { get; set; }

        //... other dimesions


        [Measure]
        public decimal Amount { get; set; }

        [Measure]
        public decimal Acount { get; set; }

        //... other measures
    }

    private IEnumerable<T> GetData<T>()
    {
        using (var connection = new FbConnection(ConnectionString))
        {
            connection.Open();
            var now = DateTime.Parse(Request["Constr"]);
            var items = connection.Query<T>("select * from [SampleTable]",
                new
                {
                   /*
                       declare parameters 
                   */
                },
                commandTimeout: 3600);
            return items;
        }
    }

    var config = new Config<Item>
    {
        DataReader = GetData<Item>,
        Name = ReportName,
        IsCreateCube = true,
        IsTruncateData = false,
        TemplatePath = Server.MapPath("~/bin"),
        ActiveDataTimeout = 1440,
        ConnectionString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString,
        OlapConnectionString = ConfigurationManager.ConnectionStrings["OlapConnectionString"].ConnectionString
    };
    var executor = new Executor<Item>(config);
    executor.Run();
```