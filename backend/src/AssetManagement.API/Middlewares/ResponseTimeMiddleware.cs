namespace AssetManagement.API.Middlewares
{
    public class ResponseTimeMiddleware : IMiddleware
    {
        private readonly ILogger<ResponseTimeMiddleware> _logger;

        public ResponseTimeMiddleware(ILogger<ResponseTimeMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            DateTime startTime = DateTime.Now;

            await next(context);

            DateTime endTime = DateTime.Now;
            var responseTime = endTime - startTime;

            _logger.LogInformation($"Response Time : {responseTime.TotalMilliseconds}");
        }
    }
}