using Newtonsoft.Json;
using StockAnalyzer.Contracts.Services;
using StockAnalyzer.Logging;
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
    /// The main class <c>ProjectsController</c>.
    /// Handles request related projects and generates 2d and 3d views
    /// </summary>
    [RoutePrefix("api/projects")]
    public class ProjectsController : BaseController
    {
        //readonly IProjectService projectService;
        //readonly IBimModelService bimService;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectsController"/> class.
        /// </summary>
        /// <param name="projectService">The project service instance.</param>
        /// <param name="bimModelService">The bim model service instance.</param>
        /// <param name="logger">An ILogger instance to log</param>
        //public ProjectsController(IProjectService projectService, IBimModelService bimModelService,
        //    ILogger logger)
        //{
        //    this.projectService = projectService;
        //    this.bimService = bimModelService;
        //}
        /// <summary>
        /// Get all projects
        /// </summary>
        /// <returns>List of all projects</returns>
        //[HttpGet]
        //public async Task<List<Project>> GetProjects()
        //{
        //    return await (Task<List<Project>>.Run(() => projectService.GetProjects()));
        //}
        /// <summary>
        /// Get project using id
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns>The project with the given ID.</returns>
        //[HttpGet]
        //[Route("{projectId}")]
        //public async Task<Project> GetProject(string projectId)
        //{
        //    return await (Task<Project>.Run(() =>
        //    {
        //        Project project = projectService.GetProject(projectId);
        //        //if (project != null)
        //        //{
        //        //    string path = $"{PathHelper.GetRootedPath(ApiConfiguration.ProjectsLocalStoragePath)}\\{projectId}\\Residential.json";
        //        //    if (File.Exists(path))
        //        //    {
        //        //        project.ProjectData = Convert.ToString(JsonConvert.DeserializeObject(File.ReadAllText(path)));
        //        //    }
        //        //}
        //        return project;
        //    }
        //    ));
        //}

        
    }
}
