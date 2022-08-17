using StockAnalyzer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalyzer.Services
{
    public interface IStockService
    {       
        Task<StockModel> GetStock(string tickerSymbol, DateTime date);

        Task<List<StockModel>> GetStockTickerSymbols();

    }
}
