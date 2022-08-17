using Newtonsoft.Json;
using StockAnalyzer.Logging;
using StockAnalyzer.Models;
using StockAnalyzer.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace StockAnalyzer.API.Controllers
{
    /// <summary>
    /// The main class <c>StocksController</c>.
    /// Handles request related stocks and generates 2d and 3d views
    /// </summary>
    [RoutePrefix("api/stocks")]
    public class StocksController : BaseController
    {
        readonly IStockService _stockService;
        /// <summary>
        /// Initializes a new instance of the <see cref="stocksController"/> class.
        /// </summary>
        /// <param name="stockService">The stock service instance.</param>       
        /// <param name="logger">An ILogger instance to log</param>
        public StocksController(IStockService stockService)
        {
            this._stockService = stockService;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }
        /// <summary>
        /// Get all stocks
        /// </summary>
        /// <returns>List of all stocks</returns>
        [HttpGet]
        public async Task<List<StockModel>> GetStockTickerSymbols()
        {
            return (await _stockService.GetStockTickerSymbols()).OrderBy(p => p.Symbol).ToList();
            
        }
        /// <summary>
        /// Get stock using ticker symbol and date
        /// </summary>
        /// <param name="tickerSymbol">Stock ticker symbol</param>
        /// <param name="date"></param>
        /// <returns>The stock with the given ticker symbol.</returns>
        [HttpGet]
        [Route("{tickerSymbol}")]

        public async Task<StockModel> GetStock(string tickerSymbol, DateTime? date)
        {

            StockModel stock = await _stockService.GetStock(tickerSymbol, date ?? DateTime.MinValue);
            if (stock == null)
            {
                string message = date.HasValue ? "Stock does not exists for the given date" : "Stock does not exists";
                throw new KeyNotFoundException(message);
            }
            return stock;
        }
    }
}
