using Moq;
using NUnit.Framework;
using StockAnalyzer.WebApi.Controllers;
using StockAnalyzer.Common.Helpers;
using StockAnalyzer.Contracts.Services;
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

namespace StockAnalyzer.WebApi.Tests
{
    [TestFixture]
    public class ProjectsTests
    {
        private Project project;
        Mock<IProjectService> repoProjectMock;
        Mock<IBimModelService> repoBimModelMock;
        Mock<ILogger> repoLogMock;
        ProjectsController controller;


        [SetUp]
        public void SetUp()
        {
            repoProjectMock = new Mock<IProjectService>();
            repoBimModelMock = new Mock<IBimModelService>();
            repoLogMock = new Mock<ILogger>();
            project = GetProjects().First();
            controller = new ProjectsController(repoProjectMock.Object, repoBimModelMock.Object, repoLogMock.Object);
        }

        [Test]
        public void GetProjectsShouldReturnProjects()
        {
            // Arrange
            repoProjectMock.Setup(repo => repo.GetProjects())
                .Returns(GetProjects());
            SetupControllerForTests(controller, HttpMethod.Get);
            //Act
            var projects = controller.GetProjects().Result;
            // Assert
            Assert.IsNotNull(projects);
            Assert.IsInstanceOf(typeof(List<Project>), projects);
            Assert.AreEqual(3, projects.Count);
        }

        [Test]
        public void GetProject_WithProjectId_ReturnsProject()
        {
            // Arrange
            repoProjectMock.Setup(repo => repo.GetProject(project.ProjectId.ToString()))
                .Returns(project);
            SetupControllerForTests(controller, HttpMethod.Get);
            //Act
            Project response = controller.GetProject(project.ProjectId.ToString()).Result;
            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOf(typeof(Project), response);
        }


        [Test]
        public void GetUserProjects_WithUserId_ReturnsProjects()
        {
            // Arrange
            repoProjectMock.Setup(repo => repo.GetUserProjects(project.UserId.ToString()))
                .Returns(GetProjects());
            SetupControllerForTests(controller, HttpMethod.Get);
            //Act
            var projects = controller.GetUserProjects(project.UserId).Result;
            // Assert
            Assert.IsNotNull(projects);
            Assert.IsInstanceOf(typeof(List<Project>), projects);
            Assert.AreEqual(3, projects.Count);
        }

        [Test]
        public void GetProjectStructure_WithProjectId_ReturnsProjectTree()
        {
            XTreeNode treeNode = new XTreeNode("");

            repoBimModelMock.Setup(repo => repo.GetModelTree(project.ProjectId.ToString()))
                .Returns(treeNode);
            SetupControllerForTests(controller, HttpMethod.Get);
            //Act
            var node = controller.GetProjectStructure(project.ProjectId.ToString()).Result;
            // Assert
            Assert.IsNotNull(node);
            Assert.IsInstanceOf(typeof(XTreeNode[]), node);
        }

        [Test]
        public void CreateProject_WithProjectData_ShouldCreateProject()
        {
            // Arrange
            repoBimModelMock.Setup(repo => repo.CreateProject(project.Name, project.Unit, project.Site))
                .Returns(project.ProjectId.ToString());
            SetupControllerForTests(controller, HttpMethod.Post);
            //Act
            var projectID = controller.CreateProject(project).Result;
            // Assert
            Assert.IsNotNull(projectID);
            Assert.AreEqual(project.ProjectId.ToString(), projectID);
        }

        [Test]
        public void SaveProject_WithProjectData_ShouldSaveProject()
        {
            repoProjectMock.Setup(repo => repo.SaveProject(project))
                .Returns(true);
            SetupControllerForTests(controller, HttpMethod.Put);
            var result = controller.SaveProject(project).Result;
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateBuilding_WithResidentialParameters_ShouldCreateResidentialBuilding()
        {
            var parameters = new BimParameters
            {
                StructureType = StructureType.Residential,
                ProjectId = project.ProjectId.ToString()
            };
            repoBimModelMock.Setup(repo => repo.CreateInterior(parameters));
            repoBimModelMock.Setup(repo => repo.CreateBuilding(parameters));
            SetupControllerForTests(controller, HttpMethod.Post);
            //Act
            HttpResponseMessage responseMessage = controller.CreateBuilding(parameters).Result;
            // Assert
            Assert.IsNotNull(responseMessage);
            Assert.AreEqual(HttpStatusCode.Created, responseMessage.StatusCode);
        }

        [Test]
        public void Get2D_WithProjectid_ReturnsSVGFilePath()
        {
            var path = $"{PathHelper.GetRootedPath()}\\{project.ProjectId}\\{project.ProjectId}_{DateTime.Now.Ticks}.svg";
            var parameters = new ViewParameters
            {
                ProjectId = project.ProjectId.ToString()
            };
            repoBimModelMock.Setup(repo => repo.CreateSVGFile(project.ProjectId.ToString(), parameters))
            .Returns(path);
            SetupControllerForTests(controller, HttpMethod.Get);
            //Act
            var url = controller.Get2D(parameters).Result;
            // Assert
            Assert.IsNotNull(url);
            Assert.That(url, Does.Contain(".svg"));
        }
        [Test]
        public void Get3D_WithProjectid_ReturnsGLTFFilePath()
        {
            var path = $"{PathHelper.GetRootedPath()}\\{project.ProjectId}\\{project.ProjectId}.gltf";
            var parameters = new ViewParameters
            {
                ProjectId = project.ProjectId.ToString()
            };
            repoBimModelMock.Setup(repo => repo.CreateGLTFFile(project.ProjectId.ToString(), parameters))
                .Returns(path);
            SetupControllerForTests(controller, HttpMethod.Get);
            //Act
            var url = controller.Get3D(parameters).Result;
            // Assert
            Assert.IsNotNull(url);
            Assert.That(url, Does.Contain(".gltf"));
        }
        [Test]
        public void Get2D_WithInvalidProjectid_ThrowFileNotFoundException()
        {
            var parameters = new ViewParameters
            {
                ProjectId = project.ProjectId.ToString()
            };
            repoBimModelMock.Setup(repo => repo.CreateSVGFile(project.ProjectId.ToString(), parameters))
                .Throws<FileNotFoundException>();
            SetupControllerForTests(controller, HttpMethod.Get);
            //Act
             Assert.ThrowsAsync<FileNotFoundException>(async () =>
             {
                 await controller.Get2D(parameters);
             });
        }


        internal static void SetupControllerForTests(ApiController controller, HttpMethod method)
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(method, "http://localhost:53769/api/Projects/");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            controller.Request.Properties.Add(HttpPropertyKeys.HttpRouteDataKey, routeData);
        }
        internal static List<Project> GetProjects()
        {
            List<Project> projects = new List<Project>();
            projects.Add(new Project
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                ProjectId = Guid.Parse("7b0067f2-4d03-4197-90a3-abc402e16295"),
                Name = "Highland Apartments 123",
                Site = "1050 Stonehedge Drive, Aurora, CO",
                Unit = "Feet"
            });
            projects.Add(new Project
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                ProjectId = Guid.Parse("40911169-8a89-4bf5-b209-887578fdb9d2"),
                Name = "Highland Apartments 123",
                Site = "1050 Stonehedge Drive, Aurora, CO",
                Unit = "Feet"
            });
            projects.Add(new Project
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                ProjectId = Guid.Parse("68a345c0-16d0-484f-a8e8-8497639bc694"),
                Name = "Highland Apartments 123",
                Site = "1050 Stonehedge Drive, Aurora, CO",
                Unit = "Feet"
            });
            return projects;

        }


    }
}
