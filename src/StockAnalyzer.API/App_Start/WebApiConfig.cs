using Autofac.Integration.WebApi;
using StockAnalyzer.API.Handler;
using StockAnalyzer.API.Logger;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace StockAnalyzer.API
{
    /// <summary>
    /// The main class <c>WebApiConfig</c>.
    /// Represents the default api configuration
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers the api configuration 
        /// </summary>
        /// <param name="config">The Http configuration object to configure.</param>
        public static void Register(HttpConfiguration config)
        {

            config.DependencyResolver = new AutofacWebApiDependencyResolver(AutofacConfig.Container);
            var logger = config.DependencyResolver.GetService(typeof(GlobalExceptionLogger));
            config.Services.Replace(typeof(IExceptionLogger), logger);
            config.Services.Replace(typeof(IExceptionHandler), new GenericExceptionHandler());
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
