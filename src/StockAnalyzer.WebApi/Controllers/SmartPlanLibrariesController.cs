using StockAnalyzer.Libraries.Services;
using StockAnalyzer.Logging;
using StockAnalyzer.Models.SmartPlanLibrary;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace StockAnalyzer.WebApi.Controllers
{
    /// <summary>
    /// The main class <c>SmartPlanLibrariesController</c>.
    /// Handles request for SmartPlanLibrary and ISmartPlanUnit libraries
    /// </summary>
    [RoutePrefix("api/SmartPlanLibraries")]

    public class SmartPlanLibrariesController : BaseController
    {
        readonly ISmartPlanLibraryService unitLibService;
        /// <summary>
        /// Initializes a new instance of the <see cref="SmartPlanLibrariesController"/> class.
        /// </summary>
        /// <param name="unitLibService">The ISmartPlanLibraryService instance.</param>
        /// <param name="logger">An ILogger instance to log</param>
        public SmartPlanLibrariesController(ISmartPlanLibraryService unitLibService, ILogger logger)
        {
            this.unitLibService = unitLibService;
        }
        // GET: api/SmartPlanLibraries


        // GET: api/UnitLibraries/unitId
        /// <summary>
        /// Gets the SmartPlanLibrary library.
        /// </summary>
        /// <returns>The available SmartPlanLibrary library.</returns>
        [HttpGet]
        [Route("library")]
        public async Task<SmartPlanLibrary> GetLibrary()
        {
            return await (Task<SmartPlanLibrary>.Run(() => unitLibService.GetLibrary()));
        }
        /// <summary>
        /// Gets the available ISmartPlanUnit libraries.
        /// </summary>
        /// <returns>Response message with library data of given id.</returns>
        public IEnumerable<ISmartPlanUnit> Get()
        {
            List<ISmartPlanUnit> result = unitLibService.GetAll();
            return result;
        }
        /// <summary>
        /// Gets ISmartPlanUnit library model for given unitID.
        /// </summary>
        /// <param name="id">An unit id value of library.</param>
        /// <returns> </returns>
        public HttpResponseMessage Get(string id)
        {
            ISmartPlanUnit model = unitLibService.GetById(id);
            if (model != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Unit Not Found");
            }
        }
        /// <summary>
        /// Gets libraries for given unit collection.
        /// </summary>
        /// <param name="units">The collection of library units.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUnitsDataByIds")]
        public HttpResponseMessage GetUnitsDataByIds([FromBody]string[] units)
        {
            List<ISmartPlanUnit> libraries = unitLibService.GetByIds(units);
            if (libraries?.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, libraries);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Unit Not Found");
            }
        }
        /// <summary>
        /// Gets libraries as per given library type
        /// </summary>
        /// <param name="libraryType">The type of libraries.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUnitsByType/{libraryType}")]
        public async Task<List<ISmartPlanUnit>> GetUnitsByType(SmartPlanLibraryType libraryType)
        {
            return await (Task<List<ISmartPlanUnit>>.Run(() => unitLibService.GetSmartPlanUnitsByType(libraryType)));

        }        
    }
}
