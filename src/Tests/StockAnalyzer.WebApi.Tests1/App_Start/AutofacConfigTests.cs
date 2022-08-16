using NUnit.Framework;
using StockAnalyzer.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace StockAnalyzer.WebApi.Tests
{
    [TestFixture()]
    public class AutofacConfigTests
    {
        ContainerBuilder autofacbuilder;

        [Test()]
        public void RegisterTypesTest_ForStaticClass()
        {
            // Arrange
            IntializeParameterValue();
            //Act
            AutofacConfig.RegisterTypes(autofacbuilder);
            //Assert
            Assert.IsNotNull(AutofacConfig.Container);
        }
        internal void IntializeParameterValue()
        {
            autofacbuilder = new ContainerBuilder();
        }
    }
}