using AssetManagement.API.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace AssetManagement.API.Tests.Middlewares
{
    public class RequestResponseLoggingMiddlewareTests
    {
        [Fact]
        public async Task Invoke_LogsRequestAndResponseDetails()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<RequestResponseLoggingMiddleware>>();
            var mockRequestDelegate = new Mock<RequestDelegate>();
            mockRequestDelegate
                .Setup(rd => rd(It.IsAny<HttpContext>()))
                .Returns(Task.CompletedTask);

            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/test";
            context.Response.StatusCode = 200;

            var middleware = new RequestResponseLoggingMiddleware(mockRequestDelegate.Object, mockLogger.Object);

            // Act
            await middleware.Invoke(context);

            // Assert
            mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Request") && v.ToString().Contains("GET") && v.ToString().Contains("/test")),
                    null,
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);

            mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Response") && v.ToString().Contains("200")),
                    null,
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
