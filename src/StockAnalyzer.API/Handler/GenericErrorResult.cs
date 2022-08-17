using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace StockAnalyzer.API.Handler
{
    /// <summary>
    /// The main class <c>GenericErrorResult</c>.
    /// Provides generic exception action result
    /// </summary>
    public class GenericErrorResult : IHttpActionResult
    {
        private readonly string errorMessage;
        private readonly HttpRequestMessage requestMessage;
        private readonly HttpStatusCode statusCode;
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericErrorResult"/> class.
        /// </summary>
        /// <param name="requestMessage">The exception request message.</param>
        /// <param name="statusCode">The exception status code.</param>
        /// <param name="errorMessage">An error message for exception.</param>
        public GenericErrorResult(HttpRequestMessage requestMessage,
           HttpStatusCode statusCode, string errorMessage)
        {
            this.requestMessage = requestMessage;
            this.statusCode = statusCode;
            this.errorMessage = errorMessage;
        }
        /// <summary>
        /// Creates the http response message for the exception.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Exception error response message.</returns>
        public Task<HttpResponseMessage> ExecuteAsync(
           CancellationToken cancellationToken)
        {
            return Task.FromResult(requestMessage.CreateErrorResponse(
                statusCode, errorMessage));
        }
    }
}