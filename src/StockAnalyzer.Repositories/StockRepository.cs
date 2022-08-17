using StockAnalyzer.Data;
using StockAnalyzer.Entities;
using StockAnalyzer.FileStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StockAnalyzer.Repositories
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        private MyDBContext _dataContext;        
        protected new IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        public StockRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            DatabaseFactory = databaseFactory;
        }

        protected new MyDBContext DataContext
        {
            get { return _dataContext ?? (_dataContext = DatabaseFactory.Get()); }
        }        
    }
}
