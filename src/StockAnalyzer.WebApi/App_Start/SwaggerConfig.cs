using StockAnalyzer.WebApi;
using Swashbuckle.Application;
using System.Linq;
using System.Web.Http;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace StockAnalyzer.WebApi
{
    /// <summary>
    /// The main class <c>SwaggerConfig</c>.
    /// Provides Swagger configuration of the API.
    /// </summary>
    public static class SwaggerConfig
    {
        /// <summary>
        /// Registers the methods to enable Swagger configuration.
        /// </summary>
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {

                        c.SingleApiVersion("v1", "StockAnalyzer API");

                        c.IncludeXmlComments($@"{System.AppDomain.CurrentDomain.BaseDirectory}\bin\StockAnalyzer.WebApi.xml");
                        c.DescribeAllEnumsAsStrings();
                        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DocumentTitle("StockAnalyzer API");
                        c.InjectStylesheet(thisAssembly, "StockAnalyzer.WebApi.Content.swagger.css");
                        c.EnableDiscoveryUrlSelector();
                    });
        }

    }
}
