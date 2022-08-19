using Autofac;
using Autofac.Integration.WebApi;
using StockAnalyzer.WebApi.Logger;
using StockAnalyzer.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StockAnalyzer.WebApi
{
    /// <summary>
    /// The main class <c>AutofacConfig</c>.
    /// Provides Autofac DI configuration of the API.
    /// </summary>
    public static class AutofacConfig
    {

        #region Autofac Container
        private static Lazy<IContainer> builder =
          new Lazy<IContainer>(() =>
          {
              var autofacbuilder = new ContainerBuilder();
              RegisterTypes(autofacbuilder);
              return autofacbuilder.Build();
          });

        /// <summary>
        /// Configured Autofac Container.
        /// </summary>
        public static IContainer Container => builder.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the autofac container builder.
        /// </summary>
        /// <param name="builder">The autofac container builder to configure.</param>
        /// <remarks>
        /// </remarks>
        public static void RegisterTypes(ContainerBuilder builder)
        {
           string baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory + "bin";
            if (!Directory.Exists(baseDirectoryPath))
                baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;

            builder.RegisterModule(new LoggingModule());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            builder.RegisterType<GlobalExceptionLogger>().InstancePerLifetimeScope();

            var assemblies = Directory.EnumerateFiles(baseDirectoryPath, "*.dll", SearchOption.TopDirectoryOnly)
                .Where(filePath => Path.GetFileName(filePath).StartsWith("StockAnalyzer"))
                .Select(Assembly.LoadFrom).Where(assemblyType =>
                (assemblyType.FullName.StartsWith("StockAnalyzer") && !assemblyType.FullName.Contains("StockAnalyzer.Framework") &&
                !assemblyType.FullName.Contains("StockAnalyzer.WebApi")
                )).ToArray();

            builder.RegisterAssemblyTypes(assemblies)
            .AsImplementedInterfaces().InstancePerLifetimeScope();

        }
    }
}