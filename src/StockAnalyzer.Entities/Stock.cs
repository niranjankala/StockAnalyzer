using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalyzer.Entities
{
    public class Stock
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string exchange { get; set; }
        public string exchangeShortName { get; set; }
        public string type { get; set; }
        
    }
}
