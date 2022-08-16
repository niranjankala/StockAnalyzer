using StockAnalyzer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalyzer.Data
{
    //Dummy Context for faking the code
    [DbConfigurationType(typeof(ResilientDbConfiguration))]
    public partial class MyDBContext : DbContext
    {
        public MyDBContext() : base("name=MyDBContext") { }

        public virtual DbSet<Stock> Stocks { get; set; }        

        

        public virtual void Commit()
        {
            base.SaveChanges();
        }
    }
}
