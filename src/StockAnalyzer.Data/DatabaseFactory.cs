using System;
using System.Linq;

namespace StockAnalyzer.Data
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private MyDBContext dataContext;
        public MyDBContext Get()
        {
            return dataContext ?? (dataContext = new MyDBContext());
        }
        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}
