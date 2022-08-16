using System;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;

namespace StockAnalyzer.Data
{
    public class ResilientDbConfiguration : DbConfiguration
    {
        public ResilientDbConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy(5, TimeSpan.FromSeconds(10)));
        }
    }
}
