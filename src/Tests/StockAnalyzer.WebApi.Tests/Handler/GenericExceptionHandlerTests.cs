using Moq;
using NUnit.Framework;
using StockAnalyzer.WebApi.Handler;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace StockAnalyzer.WebApi.Tests.Handler
{
    [TestFixture]
    public class GenericExceptionHandlerTests
    {

        [SetUp]
        public void SetUp()
        {

        }

        [TearDown]
        public void TearDown()
        {            
        }

        private GenericExceptionHandler CreateGenericExceptionHandler()
        {
            return new GenericExceptionHandler();
        }

        [Test]
        public async Task HandleAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            //Arrange
            var genericExceptionHandler = this.CreateGenericExceptionHandler();
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);
            var exception = new Exception("Hello World");
            var catchblock = new ExceptionContextCatchBlock("webpi", true, false);
            var configuration = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/test");
            request.SetConfiguration(configuration);
            var exceptionContext = new ExceptionContext(exception, catchblock, request);
            ExceptionHandlerContext context = new ExceptionHandlerContext(exceptionContext);

            Assert.IsNull(context.Result);
            
            // Act
            await genericExceptionHandler.HandleAsync(
                context,
                cancellationToken);

            //Assert
            Assert.IsNotNull(context.Result);
        }
    }
}
