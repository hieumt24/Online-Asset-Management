using AssetManagement.API.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace AssetManagement.API.Tests.Middlewares
{
    public class ResponseTimeMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_LogsResponseTime()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ResponseTimeMiddleware>>();
            var mockRequestDelegate = new Mock<RequestDelegate>();
            mockRequestDelegate
                .Setup(rd => rd(It.IsAny<HttpContext>()))
                .Returns(async (HttpContext ctx) =>
                {
                    // Simulate some processing time
                    await Task.Delay(100);
                });

            var context = new DefaultHttpContext();

            var middleware = new ResponseTimeMiddleware(mockLogger.Object);

            // Act
            await middleware.InvokeAsync(context, mockRequestDelegate.Object);

            // Assert
            mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Response Time :")),
                    null,
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
