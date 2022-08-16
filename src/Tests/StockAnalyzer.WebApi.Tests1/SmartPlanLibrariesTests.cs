using Moq;
using NUnit.Framework;
using StockAnalyzer.WebApi.Controllers;
using StockAnalyzer.Libraries.Services;
using StockAnalyzer.Logging;
using StockAnalyzer.Models.SmartPlanLibrary;
using System.Collections.Generic;
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
    public class SmartPlanLibrariesTests
    {
        List<LivingUnit> livingUnits;
        List<Elevator> elevators;
        List<Stair> stairs;
        Mock<ISmartPlanLibraryService> smartPlanLibraryMock;
        Mock<ILogger> loggerMock;
        SmartPlanLibrariesController smartPlanLibraryController;

        [SetUp]
        public void Initialize()
        {
            livingUnits = GetLivingUnits();
            elevators = GetElevators();
            stairs = GetStairs();
            smartPlanLibraryMock = new Mock<ISmartPlanLibraryService>();
            loggerMock = new Mock<ILogger>();
            smartPlanLibraryController = new SmartPlanLibrariesController(smartPlanLibraryMock.Object, loggerMock.Object);
            SetupControllerForTests(smartPlanLibraryController);
        }

        [Test]
        public void GetLibraryShouldReturnSmartPlanLibrary()
        {
            //Arrange
            smartPlanLibraryMock.Setup(ser => ser.GetLibrary()).Returns(GetLibrary());
            //Act
            var library = smartPlanLibraryController.GetLibrary().Result;
            //Assert
            Assert.IsNotNull(library);
            Assert.IsInstanceOf(typeof(SmartPlanLibrary), library);
        }

        [Test]
        public void Get_ReturnsISmartPlanLibraryUnits()
        {
            smartPlanLibraryMock.Setup(ser => ser.GetAll()).Returns(this.Units);
            var smartPlanUnits = smartPlanLibraryController.Get();

            Assert.IsNotNull(smartPlanUnits);
            Assert.AreEqual(smartPlanUnits.Count(), this.Units().Count);
            Assert.IsInstanceOf(typeof(List<ISmartPlanUnit>), smartPlanUnits);
        }

        [Test]
        public void Get_WithUnitId_ReturnsISmartPlanLibraryUnitResponse()
        {
            ISmartPlanUnit unitModel = livingUnits.First();
            smartPlanLibraryMock.Setup(ser => ser.GetById(unitModel.UnitID)).Returns(unitModel);
            var response = smartPlanLibraryController.Get(unitModel.UnitID);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void Get_WithNonExistingUnitId_ReturnsNotFoundResponse()
        {
            ISmartPlanUnit unitModel = livingUnits.First();
            smartPlanLibraryMock.Setup(ser => ser.GetById(unitModel.UnitID));
            var response = smartPlanLibraryController.Get(unitModel.UnitID);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public void GetUnitsDataByIdsShouldReturnISmartPlanUnitResponse()
        {
            List<ISmartPlanUnit> unitList = new List<ISmartPlanUnit>();
            unitList.AddRange(livingUnits);
            string[] unitIds = { "AP-S-2618-01-O-01_60", "AP-S-3218-03-O-01_35" };
            smartPlanLibraryMock.Setup(ser => ser.GetByIds(unitIds)).Returns(unitList);
            var response = smartPlanLibraryController.GetUnitsDataByIds(unitIds);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

        }

        [Test]
        public void GetUnitsDataByIds_WithNonExistingUnitIds_ReturnsNotFoundResponse()
        {
            List<ISmartPlanUnit> unitList = new List<ISmartPlanUnit>();
            unitList.AddRange(livingUnits);
            string[] unitIds = { "AP-S-2618-01-O-01_60", "AP-S-3218-03-O-01_35" };
            smartPlanLibraryMock.Setup(ser => ser.GetByIds(unitIds));
            var response = smartPlanLibraryController.GetUnitsDataByIds(unitIds);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public void GetUnitsByTypeShouldReturnISmartPlanUnits()
        {
            List<ISmartPlanUnit> units = new List<ISmartPlanUnit>();
            units.AddRange(elevators);
            smartPlanLibraryMock.Setup(ser => ser.GetSmartPlanUnitsByType(SmartPlanLibraryType.Elevator)).Returns(units);
            var smartPlanUnits = smartPlanLibraryController.GetUnitsByType(SmartPlanLibraryType.Elevator).Result;
            Assert.IsNotNull(smartPlanUnits);
            Assert.AreEqual(units.Count, units.Count);
            Assert.IsInstanceOf(typeof(List<ISmartPlanUnit>), smartPlanUnits);
        }

        internal static void SetupControllerForTests(ApiController controller)
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:53769/api/Projects/");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            controller.Request.Properties.Add(HttpPropertyKeys.HttpRouteDataKey, routeData);
        }

        private List<ISmartPlanUnit> Units()
        {
            List<ISmartPlanUnit> unitList = new List<ISmartPlanUnit>();
            unitList.AddRange(livingUnits);
            unitList.AddRange(elevators);
            unitList.AddRange(stairs);
            return unitList;
        }
        private SmartPlanLibrary GetLibrary()
        {
            return (new SmartPlanLibrary { LivingUnits = livingUnits, Elevators = elevators, Stairs = stairs });
        }
        internal static List<LivingUnit> GetLivingUnits()
        {
            return (new List<LivingUnit>  {
            new LivingUnit
            {
                UnitID ="AP-S-2618-01-O-01_60", Name="Studio Unit A", Units="ft/in" ,IfcFile="AP-S-2618-01-O-01_60.ifc",
            Depth=26 ,Width=18 ,UnitFTFHeight=10.75 ,PlanPngFile="AP-S-2618-01-O-01_60.png",
            PlanSVGFile="AP-S-2618-01-O-01_60.svg", UnitClngHeight=9,
            FloorArea=447 ,BedroomCount="0", BathroomCount="1",
            PlumbingGroups="A, B, C" ,CabinetGroups="A, B, C", ApplianceGroups="A, B, C",
            FinishGroups="A, B, C"
            },
            new LivingUnit
            {
                 UnitID="AP-S-3218-03-O-01_35" ,Name="Studio Unit B", Units="ft/in",
            IfcFile="AP-S-3218-03-O-01_35.ifc", Depth=32, Width=18, UnitFTFHeight=10.75,
            PlanPngFile="AP-S-3218-03-O-01_35.png", PlanSVGFile="AP-S-3218-03-O-01_35.svg",
            UnitClngHeight=9, FloorArea=552, BedroomCount="0", BathroomCount="1",
            PlumbingGroups="A, B, C", CabinetGroups="A, B, C" ,ApplianceGroups="A, B, C", FinishGroups="A, B, C"
            }
        });
        }
        internal static List<Elevator> GetElevators()
        {
            return (new List<Elevator> {
            new Elevator
            {
                UnitID ="EL-A-1010-01-O-01_01", Name="10' x 10' Elevator A", Units="ft/in", IfcFile="EL-A-1010-01-O-01_01.ifc", Depth=10,
                Width =10, UnitFTFHeight=10.75, PlanPngFile="EL-A-1010-01-O-01_01.png" ,PlanSVGFile="EL-A-1010-01-O-01_01.svg", CarCount="1", ElevManuf="Kone" ,
                ElevModel ="E1002", FinishGroups="A, B, C"
            },
            new Elevator
            {
                UnitID ="EL-A-1210-01-O-01_01", Name="12' x 10' Elevator A", Units="ft/in" ,IfcFile="EL-A-1210-01-O-01_01.ifc" ,
                Depth =12 ,Width=10 ,UnitFTFHeight=10.75 ,PlanPngFile ="EL-A-1210-01-O-01_01.png", PlanSVGFile="EL-A-1210-01-O-01_01.svg"
                ,CarCount="1" ,ElevManuf="Kone" ,ElevModel="E1004" ,FinishGroups="A, B, C"
            }
        });
        }
        internal static List<Stair> GetStairs()
        {
            return (new List<Stair> {
            new Stair
            {
                UnitID ="ST-A-2010-01-O-01_01", Name="20' x 10' Stair A", Units="ft/in" ,IfcFile="ST-A-2010-01-O-01_01.ifc", Depth=20, Width=10 ,
                UnitFTFHeight =10.75, PlanPngFile="ST-A-2010-01-O-01_01.png", PlanSVGFile="ST-A-2010-01-O-01_01.svg",
                RiserCount ="20" ,ExitPathWidthIn="45" ,RailingGroups="A, B, C", FinishGroups="A, B, C"
            },
            new Stair
            {
                UnitID ="ST-A-2012-01-O-01_01" ,Name="20' x 12' Stair A" ,Units="ft/in" ,IfcFile="ST-A-2012-01-O-01_01.ifc", Depth=20 ,
                Width =12, UnitFTFHeight=10.75 ,PlanPngFile="ST-A-2012-01-O-01_01.png", PlanSVGFile="ST-A-2012-01-O-01_01.svg", RiserCount="20" ,
                ExitPathWidthIn ="60" ,RailingGroups="A, B, C" ,FinishGroups="A, B, C"
            }
        });
        }
    }
}
