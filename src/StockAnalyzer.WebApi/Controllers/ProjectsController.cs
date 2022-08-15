using Newtonsoft.Json;
using StockAnalyzer.Common;
using StockAnalyzer.Common.Helpers;
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
        readonly IProjectService projectService;
        readonly IBimModelService bimService;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectsController"/> class.
        /// </summary>
        /// <param name="projectService">The project service instance.</param>
        /// <param name="bimModelService">The bim model service instance.</param>
        /// <param name="logger">An ILogger instance to log</param>
        public ProjectsController(IProjectService projectService, IBimModelService bimModelService,
            ILogger logger)
        {
            this.projectService = projectService;
            this.bimService = bimModelService;
        }
        /// <summary>
        /// Get all projects
        /// </summary>
        /// <returns>List of all projects</returns>
        [HttpGet]
        public async Task<List<Project>> GetProjects()
        {
            return await (Task<List<Project>>.Run(() => projectService.GetProjects()));
        }
        /// <summary>
        /// Get project using id
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns>The project with the given ID.</returns>
        [HttpGet]
        [Route("{projectId}")]
        public async Task<Project> GetProject(string projectId)
        {
            return await (Task<Project>.Run(() =>
            {
                Project project = projectService.GetProject(projectId);
                //if (project != null)
                //{
                //    string path = $"{PathHelper.GetRootedPath(ApiConfiguration.ProjectsLocalStoragePath)}\\{projectId}\\Residential.json";
                //    if (File.Exists(path))
                //    {
                //        project.ProjectData = Convert.ToString(JsonConvert.DeserializeObject(File.ReadAllText(path)));
                //    }
                //}
                return project;
            }
            ));
        }

        /// <summary>
        /// Get all projects created by a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserProjects/{userId}")]
        public async Task<List<Project>> GetUserProjects(Guid userId)
        {
            return await (Task<List<Project>>.Run(() => projectService.GetUserProjects(userId.ToString())));
        }
        /// <summary>
        /// Gets tree structure of the project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [Route("{projectId}/projectstructure")]
        public async Task<XTreeNode[]> GetProjectStructure(string projectId)
        {
            return await Task<XTreeNode[]>.Run(() =>
            {
                List<XTreeNode> projectNodes = new List<XTreeNode>();
                XTreeNode node = bimService.GetModelTree(projectId);
                if (node != null)
                {
                    node.Expanded = true;
                    projectNodes.Add(node);
                }
                return projectNodes.ToArray();
            });
        }
        /// <summary>
        /// Creates a project
        /// </summary>
        /// <param name="project"></param>
        /// <returns>Project Id</returns>
        [HttpPost]
        public async Task<string> CreateProject([FromBody]Project project)
        {
            return await Task<string>.Run(() =>
            {
                string projectId = "";
                projectId = bimService.CreateProject(project.Name, project.Unit, project.Site);
                project.ProjectId = Guid.Parse(projectId);
                projectService.SaveProject(project);
                return projectId;
            });
        }
        /// <summary>
        /// Saves a project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> SaveProject([FromBody] Project project)
        {
            return await Task<bool>.Run(() =>
            {
                return projectService.SaveProject(project);
            });
        }

        /// <summary>
        /// Creates a building in the project
        /// </summary>
        /// <param name="parameters">Data required to create a building in the project</param>
        /// <returns></returns>
        [HttpPost]
        [Route("building")]
        public async Task<HttpResponseMessage> CreateBuilding([FromBody]BimParameters parameters)
        {
            return await Task<HttpResponseMessage>.Run(() =>
            {
                // ToDO: Remove this logic before the final handover
                if (parameters?.StructureType == StructureType.Residential &&
                   (parameters.ResidentialUnitInstancesLeft?.Count ?? 0) == 0)
                {
                    bimService.CreateInterior(parameters);
                }
                bimService.CreateBuilding(parameters);
                return new HttpResponseMessage(HttpStatusCode.Created);
            });
        }

        /// <summary>
        /// Create 2d view of the project
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="parameters"></param>
        /// <returns>Path of the 2d view file</returns>
        [HttpPost]
        [Route("Get2D")]
        public async Task<string> Get2D([FromBody]ViewParameters parameters)
        {
            return await Task<string>.Run(() =>
            {
                string svgFilePath = bimService.CreateSVGFile(parameters.ProjectId, parameters);
                return CreateFileUrl(svgFilePath);
            });
        }

        /// <summary>
        /// Create 3d view of the project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="viewParameters"></param>
        /// <returns>Url of the 3d view file</returns>
        [HttpPost]
        [Route("Get3D")]
        public async Task<string> Get3D([FromBody]ViewParameters viewParameters)
        {
            return await Task<string>.Run(() =>
            {
                string gltfFilePath = bimService.CreateGLTFFile(viewParameters.ProjectId, viewParameters);
                return CreateFileUrl(gltfFilePath);
            });
        }

        /// <summary>
        /// Gets Bim parameters of the project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>bim parameters of project</returns>
        [HttpGet]
        [Route("{projectId}/GetParameters")]
        public async Task<BimParameters> GetBimParameters(string projectId)
        {
            return await Task<BimParameters>.Run(() =>
            {
                string path = $"{PathHelper.GetRootedPath("Media\\"+ ApiConfiguration.ProjectsLocalStoragePath)}\\{projectId}\\Residential-final.json";
                if (File.Exists(path))
                {
                    BimParameters parameters = JsonConvert.DeserializeObject<BimParameters>(File.ReadAllText(path));
                    return parameters;
                }
                return null;
            });
        }

        private string CreateFileUrl(string filePath)
        {
            string url = string.Empty;
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                string hostingPath = PathHelper.GetRootedPath();
                filePath = filePath.Replace(hostingPath, "");
                if (!filePath.StartsWith("Media"))
                {
                    filePath = "Media/" + filePath;
                }
                url = Request.RequestUri.GetLeftPart(UriPartial.Authority) + Request.GetRequestContext().VirtualPathRoot;
                url = $"{url}{(filePath.Replace("\\", "/"))}";
            }
            return url;
        }
    }
}
