using Newtonsoft.Json;
using StockAnalyzer.Entities;
using StockAnalyzer.FileStorage;
using StockAnalyzer.Models;
using StockAnalyzer.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StockAnalyzer.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IFileStore _mediaFileStore;
        public StockService(IStockRepository stockRepository, IFileStore fileStore)
        {
            _stockRepository = stockRepository;
            _mediaFileStore = fileStore;
        }

        public async Task<StockModel> GetStock(string tickerSymbol, DateTime date)
        {
            StockModel result = null;
            List<StockModel> stocks = await GetStocks();
            if(date != DateTime.MinValue)
            {
                result = stocks.Where(p => p.Symbol == tickerSymbol && p.Date == date).OrderByDescending(p=> p.Date).FirstOrDefault();
            }
            else
            {
                result = stocks.Where(p => p.Symbol == tickerSymbol).OrderByDescending(p => p.Date).FirstOrDefault();
            }
            return result;
        }

        public async Task<List<StockModel>> GetStockTickerSymbols()
        {
            return (await GetStocks());
        }

        async Task<List<StockModel>> GetStocks()
        {   
            List<Stock> stocks = new List<Stock>();

            var mediaFile = await _mediaFileStore.GetFileInfoAsync("stock_symbols.json");
            if (mediaFile != null)
            {
                string json = string.Empty;
                var stream = await _mediaFileStore.GetFileStreamAsync(mediaFile);
                if (stream != null)
                {
                    stream.Position = 0;
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        json = reader.ReadToEnd();
                    }
                }

                if (!string.IsNullOrEmpty(json))
                {
                    stocks = JsonConvert.DeserializeObject<List<Stock>>(json);
                }
            }

            return stocks.Select(p => new StockModel()
            {
                Symbol = p.symbol,
                Name = p.name,
                Price = p.price,
                Exchange = p.exchange,
                ExchangeShortName = p.exchangeShortName,
                Type = p.type
            }).ToList();
        }


    }
}
