using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AssetManagement.API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var requestTime = DateTime.Now;
            var requestMethod = httpContext.Request.Method;
            var requestPath = httpContext.Request.Path;
            _logger.LogInformation($" Request\n Time : {requestTime}\n Method Type :{requestMethod}\n  Path :{requestPath}");

            await _next(httpContext);

            var responseTime = DateTime.Now;
            var responseStatusCode = httpContext.Response.StatusCode;
            _logger.LogInformation($" Response\n Time : {responseTime}\n Status Code :{responseStatusCode}");
        }
    }
}