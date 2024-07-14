using AssetManagement.API.Middlewares;
using AssetManagement.Domain.Common.Settings;
using AssetManagement.Domain.Entites;
using AssetManagement.Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace AssetManagement.API.Tests.Middlewares
{
    public class TokenValidationMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly Mock<IOptions<JWTSettings>> _jwtSettingsMock;
        private readonly Mock<ILogger<TokenValidationMiddleware>> _loggerMock;
        private readonly ApplicationDbContext _dbContext;

        public TokenValidationMiddlewareTests()
        {
            _nextMock = new Mock<RequestDelegate>();
            _jwtSettingsMock = new Mock<IOptions<JWTSettings>>();
            _loggerMock = new Mock<ILogger<TokenValidationMiddleware>>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new ApplicationDbContext(options);
        }

        [Fact]
        public async Task InvokeAsync_ValidToken_CallsNextDelegate()
        {
            // Arrange
            var middleware = new TokenValidationMiddleware(
                _nextMock.Object,
                _jwtSettingsMock.Object,
                _loggerMock.Object
            );

            var context = new DefaultHttpContext();
            context.Request.Headers["Authorization"] = "Bearer valid_token";

            // Act
            await middleware.InvokeAsync(context, _dbContext);

            // Assert
            _nextMock.Verify(next => next(context), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_MissingToken_ReturnsUnauthorized()
        {
            // Arrange
            var middleware = new TokenValidationMiddleware(
                _nextMock.Object,
                _jwtSettingsMock.Object,
                _loggerMock.Object
            );

            var context = new DefaultHttpContext();

            // Act
            await middleware.InvokeAsync(context, _dbContext);

            // Assert
            Assert.Equal(401, context.Response.StatusCode);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Equal("Unauthorized request: Missing token", responseText);
            _loggerMock.Verify(logger => logger.LogInformation("Unauthorized request: Missing token"), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_BlacklistedToken_ReturnsUnauthorized()
        {
            // Arrange
            var blacklistedToken = new BlackListToken { Token = "blacklisted_token" };
            _dbContext.BlackListTokens.Add(blacklistedToken);
            _dbContext.SaveChanges();

            var middleware = new TokenValidationMiddleware(
                _nextMock.Object,
                _jwtSettingsMock.Object,
                _loggerMock.Object
            );

            var context = new DefaultHttpContext();
            context.Request.Headers["Authorization"] = "Bearer blacklisted_token";

            // Act
            await middleware.InvokeAsync(context, _dbContext);

            // Assert
            Assert.Equal(401, context.Response.StatusCode);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Equal("Unauthorized request: Token is blacklisted", responseText);
            _loggerMock.Verify(logger => logger.LogInformation("Unauthorized request: Token is blacklisted"), Times.Once);
        }
    }
}
