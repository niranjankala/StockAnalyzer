using Newtonsoft.Json;
using StockAnalyzer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace StockAnalyzer.WebApi.Controllers
{
    /// <summary>
    /// The main class <c>StocksController</c>.
    /// Handles request related stocks and generates 2d and 3d views
    /// </summary>
    [RoutePrefix("api/stocks")]
    public class StocksController : BaseController
    {
        //readonly IProjectService stockService;        
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectsController"/> class.
        /// </summary>
        /// <param name="stockService">The stock service instance.</param>       
        /// <param name="logger">An ILogger instance to log</param>
        //public StocksController(IProjectService stockService,
        //    ILogger logger)
        //{
        //    this.stockService = stockService;      
        //}
        /// <summary>
        /// Get all stocks
        /// </summary>
        /// <returns>List of all stocks</returns>
        //[HttpGet]
        //public async Task<List<Project>> GetProjects()
        //{
        //    return await (Task<List<Project>>.Run(() => stockService.GetProjects()));
        //}
        /// <summary>
        /// Get stock using id
        /// </summary>
        /// <param name="stockId">Project Id</param>
        /// <returns>The stock with the given ID.</returns>
        //[HttpGet]
        //[Route("{stockId}")]
        //public async Task<Project> GetProject(string stockId)
        //{
        //    return await (Task<Project>.Run(() =>
        //    {
        //        Project stock = stockService.GetProject(stockId);
        //        //if (stock != null)
        //        //{
        //        //    string path = $"{PathHelper.GetRootedPath(ApiConfiguration.ProjectsLocalStoragePath)}\\{stockId}\\Residential.json";
        //        //    if (File.Exists(path))
        //        //    {
        //        //        stock.ProjectData = Convert.ToString(JsonConvert.DeserializeObject(File.ReadAllText(path)));
        //        //    }
        //        //}
        //        return stock;
        //    }
        //    ));
        //}

        
    }
}
