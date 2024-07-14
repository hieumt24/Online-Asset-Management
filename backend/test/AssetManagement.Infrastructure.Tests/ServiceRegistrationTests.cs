using AssetManagement.Application.Common;
using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Domain.Common.Models;
using AssetManagement.Domain.Common.Settings;
using AssetManagement.Infrastructure;
using AssetManagement.Infrastructure.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace AssetManagement.Infrastructure.Tests
{
    public class ServiceRegistrationTests
    {
        [Fact]
        public void ConfigureServices_ShouldRegisterServices()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var configurationMock = new Mock<IConfiguration>();
            var connectionString = "Server=.\\SQLEXPRESS;Database=Test;TrustServerCertificate=True; Integrated Security=True;";

            // Set up the configuration mock to return the connection string
            var configurationSectionMock = new Mock<IConfigurationSection>();
            configurationSectionMock.Setup(c => c["DefaultConnection"]).Returns(connectionString);
            configurationMock.Setup(c => c.GetSection("ConnectionStrings")).Returns(configurationSectionMock.Object);

            // Act
            ServiceRegistration.Configure(serviceCollection, configurationMock.Object);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert
            Assert.NotNull(serviceProvider.GetService<IBaseRepositoryAsync<BaseEntity>>());
            Assert.NotNull(serviceProvider.GetService<IUserRepositoriesAsync>());
            Assert.NotNull(serviceProvider.GetService<IAssetRepositoriesAsync>());
            Assert.NotNull(serviceProvider.GetService<ICategoryRepositoriesAsync>());
            Assert.NotNull(serviceProvider.GetService<IAssignmentRepositoriesAsync>());
            Assert.NotNull(serviceProvider.GetService<IReturnRequestRepositoriesAsync>());
            Assert.NotNull(serviceProvider.GetService<ITokenRepositoriesAsync>());
            Assert.NotNull(serviceProvider.GetService<ApplicationDbContext>());

            // Ensure that ApplicationDbContext is configured with the correct connection string
            var dbContextOptions = serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            using var dbContext = new ApplicationDbContext(dbContextOptions);
            var actualConnectionString = dbContext.Database.GetDbConnection().ConnectionString;

            Assert.Equal(connectionString, actualConnectionString);
        }


        [Fact]
        public void ConfigureServices_ConfiguresJWTAuthentication()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            ServiceRegistration.Configure(services, configuration);

            var serviceProvider = services.BuildServiceProvider();

            var jwtSettings = configuration.GetSection("JWTSettings").Get<JWTSettings>();
            var jwtBearerOptions = serviceProvider.GetService<IOptionsMonitor<JwtBearerOptions>>();

            // Assert
            Assert.NotNull(jwtBearerOptions);
            Assert.Equal(jwtSettings.Issuer, jwtBearerOptions.CurrentValue.TokenValidationParameters.ValidIssuer);
            Assert.Equal(jwtSettings.Audience, jwtBearerOptions.CurrentValue.TokenValidationParameters.ValidAudience);
            Assert.IsType<SymmetricSecurityKey>(jwtBearerOptions.CurrentValue.TokenValidationParameters.IssuerSigningKey);

            var symmetricSecurityKey = jwtBearerOptions.CurrentValue.TokenValidationParameters.IssuerSigningKey as SymmetricSecurityKey;
            //Assert.Equal(jwtSettings.Key, symmetricSecurityKey.Key);
        }

        [Fact]
        public void ConfigureServices_ConfiguresAuthorizationPolicies()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build(); 
            ServiceRegistration.Configure(services, configuration);
            var serviceProvider = services.BuildServiceProvider();

            var authorizationOptions = serviceProvider.GetService<Microsoft.AspNetCore.Authorization.AuthorizationOptions>();

            // Assert
            Assert.NotNull(authorizationOptions);

            var policy = authorizationOptions.GetPolicy("RequireAdminRole");
            Assert.NotNull(policy);
        }
    }
}
