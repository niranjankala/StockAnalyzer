using Autofac;
using StockAnalyzer.Logging;
using log4net.Appender;
using log4net.Core;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using ILogger = StockAnalyzer.Logging.ILogger;
using NUnit.Framework;

namespace StockAnalyzer.Tests.Logging
{
    [TestFixture]
    public class LoggingModuleTests
    {
        public LoggingModuleTests()
        {
            Serilog.Log.Logger = new LoggerConfiguration().WriteTo.Sink(
                  new InMemorySink()).CreateLogger();
        }
        [Test]
        public void LoggingModuleWillSetLoggerProperty()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new LoggingModule());
            builder.RegisterType<StubLogger>();
            var container = builder.Build();
            var loggerService = container.Resolve<StubLogger>();
            Assert.IsNotNull(loggerService.Logger);
        }

        [Test]
        public void LoggerFactoryIsPassedTheTypeOfTheContainingInstance()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new LoggingModule());
            builder.RegisterType<StubLogger>();
            var stubFactory = new StubFactory();
            builder.RegisterInstance(stubFactory).As<ILoggerFactory>();
            var container = builder.Build();
            var loggerService = container.Resolve<StubLogger>();
            Assert.IsNotNull(loggerService.Logger);
            Assert.AreEqual(stubFactory.CalledType, typeof(StubLogger));
        }
        [Test]
        public void GivenErrorMessageIsLoggedAndAssertingError()
        {
            Assert.IsNotNull(InMemorySink.Messages);
            InMemorySink.Messages.Clear();
            Log.Error("error");
            Assert.AreEqual(1, InMemorySink.Messages.Count());
            Assert.IsTrue(InMemorySink.Messages.FirstOrDefault().Contains("error"));
        }

        [Test]
        public void GivenWarningMessageIsLoggedAndAssertingWarning()
        {
            Assert.IsNotNull(InMemorySink.Messages);
            InMemorySink.Messages.Clear();
            Log.Warning(new ApplicationException("problem"), "crash");
            var result = InMemorySink.Messages.FirstOrDefault();
            Assert.AreEqual(1, InMemorySink.Messages.Count());
            Assert.IsTrue(result.Contains("problem"));
            Assert.IsTrue(result.Contains("crash"));
            Assert.IsTrue(result.Contains("ApplicationException"));
        }

        //[Test]
        public void DefaultLoggerConfigurationUsesCastleLoggerFactoryOverTraceSource()
        {
            #region
            //ContainerBuilder builder = new ContainerBuilder();
            //builder.RegisterModule(new LoggingModule());
            //builder.RegisterType<StubLogger>(); 
            //var container = builder.Build();
            //var loggerService = container.Resolve<StubLogger>();
            //Assert.IsNotNull(loggerService.Logger);

            //log4net.Config.BasicConfigurator.Configure(new MemoryAppender());
            //MemoryAppender.Messages.Clear();
            //loggerService.Logger.Error("-boom{0}-", 42);
            //Assert.IsTrue(MemoryAppender.Messages.Any(x=>x.Contains("-boom42-")));

            //MemoryAppender.Messages.Clear();
            //loggerService.Logger.Warning(new ApplicationException("problem"), "crash");
            //Assert.IsTrue(MemoryAppender.Messages.Any(x => x.Contains("problem")));
            //Assert.IsTrue(MemoryAppender.Messages.Any(x => x.Contains("crash")));
            //Assert.IsTrue(MemoryAppender.Messages.Any(x => x.Contains("ApplicationException")));

            #endregion
        }
    }

    public class StubLogger
    {
        public ILogger Logger { get; set; }
    }
    public class StubFactory : ILoggerFactory
    {
        public ILogger CreateLogger(Type type)
        {
            CalledType = type;
            return NullLogger.Instance;
        }
        public Type CalledType { get; set; }
    }
    public class MemoryAppender : IAppender
    {
        static MemoryAppender()
        {
            Messages = new List<string>();
        }

        public static List<string> Messages { get; set; }

        public void DoAppend(LoggingEvent loggingEvent)
        {
            if (loggingEvent.ExceptionObject != null)
            {
                lock (Messages) Messages.Add(string.Format("{0} {1} {2}",
                    loggingEvent.ExceptionObject.GetType().Name,
                    loggingEvent.ExceptionObject.Message,
                    loggingEvent.RenderedMessage));
            }
            else lock (Messages) Messages.Add(loggingEvent.RenderedMessage);
        }

        public void Close() { }
        public string Name { get; set; }
    }

    public class InMemorySink : ILogEventSink
    {
        public static List<string> Messages { get; set; }
        static InMemorySink()
        {
            Messages = new List<string>();
        }
        public void Emit(LogEvent loggingEvent)
        {
            if (loggingEvent.Exception != null)
            {
                lock (Messages) Messages.Add(string.Format("{0} {1} {2}",
                    loggingEvent.Exception.GetType().Name,
                    loggingEvent.Exception.Message,
                    loggingEvent.RenderMessage()));
            }
            else lock (Messages) Messages.Add(loggingEvent.RenderMessage());
        }
    }

}
