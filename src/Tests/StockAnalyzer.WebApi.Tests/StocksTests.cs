using Moq;
using NUnit.Framework;
using StockAnalyzer.WebApi.Controllers;
using StockAnalyzer.Logging;
using StockAnalyzer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using StockAnalyzer.Services;
using Newtonsoft.Json;
using System.Threading.Tasks;
using StockAnalyzer.Utility;

namespace StockAnalyzer.WebApi.Tests
{
    [TestFixture]
    public class ProjectsTests
    {
        private List<StockModel> stocksData;
        Mock<IStockService> repoStockServiceMock;
        StocksController controller;


        [SetUp]
        public void SetUp()
        {
            repoStockServiceMock = new Mock<IStockService>();
            stocksData = GetStocks();
            controller = new StocksController(repoStockServiceMock.Object);
        }

        [Test]
        public void GetStocksShouldReturnAllStocks()
        {
            // Arrange
            repoStockServiceMock.Setup(repo => repo.GetStockTickerSymbols())
                .Returns(GetStocksAsync());
            SetupControllerForTests(controller, HttpMethod.Get);
            //Act
            var stocks = controller.GetStockTickerSymbols().Result;
            // Assert
            Assert.IsNotNull(stocks);
            Assert.IsInstanceOf(typeof(List<StockModel>), stocks);
            Assert.AreEqual(43184, stocks.Count);
        }

        [Test]
        public void GetStock_WithTickerSymbolAndDate_ReturnsARecentStock()
        {
            // Arrange
            DateTime searchDate = DateTime.Today;
            string tickerSymbol = "CMCSA";
            repoStockServiceMock.Setup(repo => repo.GetStock(tickerSymbol, searchDate))
                .Returns(FindStock(tickerSymbol, searchDate));
            SetupControllerForTests(controller, HttpMethod.Get);
            //Act
            StockModel response = controller.GetStock(tickerSymbol, searchDate).Result;
            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOf(typeof(StockModel), response);
        }

        [Test]
        public void GetStock_WithTickerSymbolAndWithoutDate_ReturnsARecentStock()
        {
            // Arrange
            DateTime searchDate = DateTime.MinValue;
            string tickerSymbol = "CMCSA";
            repoStockServiceMock.Setup(repo => repo.GetStock(tickerSymbol, searchDate))
                .Returns(FindStock(tickerSymbol, searchDate));
            SetupControllerForTests(controller, HttpMethod.Get);
            //Act
            StockModel response = controller.GetStock(tickerSymbol, searchDate).Result;
            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOf(typeof(StockModel), response);
        }
        [Test]

        public void GetStock_WithTickerSymbolAndWithNotExistingDate_ThrowException()
        {
            // Arrange
            DateTime searchDate = DateTime.Today.AddDays(-1);
            string tickerSymbol = "CMCSA";
            repoStockServiceMock.Setup(repo => repo.GetStock(tickerSymbol, searchDate))
                 .Returns(FindStock(tickerSymbol, searchDate));
            SetupControllerForTests(controller, HttpMethod.Get);
            //Act
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
               {
                   StockModel response = await controller.GetStock(tickerSymbol, searchDate);
               });

        }
        [Test]

        public void GetStock_WithNotExistingTickerSymbolAndWithExistingDate_ThrowException()
        {
            // Arrange
            DateTime searchDate = DateTime.Today;
            string tickerSymbol = "ABCD";
            repoStockServiceMock.Setup(repo => repo.GetStock(tickerSymbol, searchDate))
                 .Returns(FindStock(tickerSymbol, searchDate));
            SetupControllerForTests(controller, HttpMethod.Get);
            //Act
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                StockModel response = await controller.GetStock(tickerSymbol, searchDate);
            });

        }

        internal static void SetupControllerForTests(ApiController controller, HttpMethod method)
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(method, "http://localhost:53769/api/Stocks/");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            controller.Request.Properties.Add(HttpPropertyKeys.HttpRouteDataKey, routeData);
        }


        Task<List<StockModel>> GetStocksAsync()
        {
            return Task.Run<List<StockModel>>(() =>
            {
                return stocksData;
            });
        }
        List<StockModel> GetStocks()
        {
            List<StockModel> stocks = new List<StockModel>();

            var mediaFilePath = PathHelper.GetRootedPath("Media\\stock_symbols.json");

            if (File.Exists(mediaFilePath))
            {
                string json = File.ReadAllText(mediaFilePath);
                if (!string.IsNullOrEmpty(json))
                {
                    stocks = JsonConvert.DeserializeObject<List<StockModel>>(json);
                }
            }
            return stocks;
        }
        Task<StockModel> FindStock(string tickerSymbol, DateTime searchDate)
        {
            return Task.Run<StockModel>(() =>
            {
                if (searchDate == DateTime.MinValue)
                {
                    return stocksData.FirstOrDefault(f => f.Symbol == tickerSymbol);
                }
                else
                {
                    return stocksData.FirstOrDefault(f => f.Symbol == tickerSymbol && f.Date == searchDate);
                }

            });
        }
    }

}
