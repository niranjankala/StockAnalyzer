using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace StockAnalyzer.WebApi.Handler
{
    /// <summary>
    /// The main class <c>GenericExceptionHandler</c>.
    /// Handles the execptions of the api.
    /// </summary>
    public class GenericExceptionHandler : ExceptionHandler
    {
        /// <summary>
        /// Custom Web API global exception handler
        /// </summary>
        /// <param name="context">ExceptionHandlerContext</param>
        /// <param name="cancellationToken"></param>
        public async override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            HttpStatusCode exceptionStatus;
            string errorMessage = "";
            if (context.Exception is NotImplementedException)
            {
                exceptionStatus = HttpStatusCode.NotImplemented;
                errorMessage = "Unavailable content";
            }
            else if (context.Exception is InvalidOperationException)
            {
                exceptionStatus = HttpStatusCode.BadRequest;
                errorMessage = "invalid operation";
            }
            else if (context.Exception is FileNotFoundException)
            {
                exceptionStatus = HttpStatusCode.NotFound;
                errorMessage = "File not found";
            }
            else if (context.Exception is FormatException)
            {
                exceptionStatus = HttpStatusCode.InternalServerError;
                errorMessage = context.Exception.Message;
            }
            else if (context.Exception is ArgumentException)
            {
                exceptionStatus = HttpStatusCode.BadRequest;
                errorMessage = context.Exception.Message;
            }
            else
            {
                errorMessage = "An unexpected error occurred";
                exceptionStatus = HttpStatusCode.InternalServerError;
            }

            context.Result = await Task<IHttpActionResult>.Run(() =>
             {
                 return new GenericErrorResult(context.Request, exceptionStatus, errorMessage);
             });
        }
    }
}