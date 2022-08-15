using StockAnalyzer.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace StockAnalyzer.WebApi.Logger
{
    /// <summary>
    /// The main class <c>GlobalExceptionLogger</c>.
    /// Handles the exception logging requests.
    /// </summary>
    public class GlobalExceptionLogger : ExceptionLogger
    {
        readonly ILogger exceptionLogger;
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionLogger"/> class.
        /// </summary>
        /// <param name="logger">An ILogger instance to log the exception.</param>
        public GlobalExceptionLogger(ILogger logger)
        {
            exceptionLogger = logger;
        }
        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="context">The exception context.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            var ex = context.Exception;
            string message = $"{ex.Message}--{ex.Source}\n{ex.StackTrace}\n{ex.TargetSite}\n";
            await Task.Run(() =>
            {
                exceptionLogger.Log(LogLevel.Error, ex, message);
            });
        }

    }
}