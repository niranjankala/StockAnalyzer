using StockAnalyzer.Data;
using StockAnalyzer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalyzer.Repositories
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        private MyDBContext dataContext;
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
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }
    }
}
