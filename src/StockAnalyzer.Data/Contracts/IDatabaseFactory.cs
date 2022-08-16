using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalyzer.Data
{
    public interface IDatabaseFactory : IDisposable
    {
        MyDBContext Get();
    }
}
