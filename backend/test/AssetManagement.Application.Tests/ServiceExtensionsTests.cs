using AssetManagement.Application;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Mappings;
using AssetManagement.Application.Models.DTOs.Assets.Requests;
using AssetManagement.Application.Models.DTOs.Assignments.Reques;
using AssetManagement.Application.Models.DTOs.Assignments.Request;
using AssetManagement.Application.Models.DTOs.Category.Requests;
using AssetManagement.Application.Models.DTOs.ReturnRequests.Request;
using AssetManagement.Application.Models.DTOs.Users.Requests;
using AssetManagement.Application.Services;
using AssetManagement.Application.Validations.Asset;
using AssetManagement.Application.Validations.Assignment;
using AssetManagement.Application.Validations.Category;
using AssetManagement.Application.Validations.ReturnRequest;
using AssetManagement.Application.Validations.User;
using AssetManagement.Domain.Common.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Text;
using Xunit;

namespace AssetManagement.Application.Tests
{
    public class ServiceExtensionsTests
    {
        private readonly ServiceCollection _services;
        private readonly Mock<IConfiguration> _configurationMock;

        public ServiceExtensionsTests()
        {
            _services = new ServiceCollection();
            _configurationMock = new Mock<IConfiguration>();
            SetupConfigurationMock();
        }

        private void SetupConfigurationMock()
        {
            var jwtSettingsSectionMock = new Mock<IConfigurationSection>();
            jwtSettingsSectionMock.Setup(x => x["Issuer"]).Returns("testIssuer");
            jwtSettingsSectionMock.Setup(x => x["Audience"]).Returns("testAudience");
            jwtSettingsSectionMock.Setup(x => x["Key"]).Returns("testKey");

            _configurationMock.Setup(x => x.GetSection("JWTSettings")).Returns(jwtSettingsSectionMock.Object);
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterServicesCorrectly()
        {
            // Act
            ServiceExtensions.ConfigureServices(_services, _configurationMock.Object);
            var serviceProvider = _services.BuildServiceProvider();

            // Assert
            Assert.NotNull(serviceProvider.GetService<IAssetServiceAsync>());
            Assert.NotNull(serviceProvider.GetService<ICategoryServiceAsync>());
            Assert.NotNull(serviceProvider.GetService<IAssignmentServicesAsync>());
            Assert.NotNull(serviceProvider.GetService<IReturnRequestServiceAsync>());
            Assert.NotNull(serviceProvider.GetService<IUserServiceAsync>());
            Assert.NotNull(serviceProvider.GetService<IReportServices>());
            Assert.NotNull(serviceProvider.GetService<ITokenService>());
            Assert.NotNull(serviceProvider.GetService<IAccountServicecs>());
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterValidatorsCorrectly()
        {
            // Act
            ServiceExtensions.ConfigureServices(_services, _configurationMock.Object);
            var serviceProvider = _services.BuildServiceProvider();

            // Assert
            Assert.NotNull(serviceProvider.GetService<IValidator<AddCategoryRequestDto>>());
            Assert.NotNull(serviceProvider.GetService<IValidator<AddAssetRequestDto>>());
            Assert.NotNull(serviceProvider.GetService<IValidator<UpdateCategoryRequestDto>>());
            Assert.NotNull(serviceProvider.GetService<IValidator<EditAssetRequestDto>>());
            Assert.NotNull(serviceProvider.GetService<IValidator<AddAssignmentRequestDto>>());
            Assert.NotNull(serviceProvider.GetService<IValidator<EditAssignmentRequestDto>>());
            Assert.NotNull(serviceProvider.GetService<IValidator<AddReturnRequestDto>>());
            Assert.NotNull(serviceProvider.GetService<IValidator<AddUserRequestDto>>());
            Assert.NotNull(serviceProvider.GetService<IValidator<EditUserRequestDto>>());
            Assert.NotNull(serviceProvider.GetService<IValidator<ChangePasswordRequest>>());
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterAutoMapperProfilesCorrectly()
        {
            // Act
            ServiceExtensions.ConfigureServices(_services, _configurationMock.Object);
            var serviceProvider = _services.BuildServiceProvider();

            // Assert
            var mapper = serviceProvider.GetService<AutoMapper.IMapper>();
            Assert.NotNull(mapper);
            var configurationProvider = serviceProvider.GetService<AutoMapper.IConfigurationProvider>();
            Assert.NotNull(configurationProvider);
            configurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void ConfigureServices_ShouldConfigureJwtBearerAuthentication()
        {
            // Act
            ServiceExtensions.ConfigureServices(_services, _configurationMock.Object);
            var serviceProvider = _services.BuildServiceProvider();

            // Assert
            var authenticationSchemeProvider = serviceProvider.GetService<IAuthenticationSchemeProvider>();
            var scheme = authenticationSchemeProvider.GetSchemeAsync(JwtBearerDefaults.AuthenticationScheme).Result;
            Assert.NotNull(scheme);
        }

        [Fact]
        public void ConfigureServices_ShouldConfigureAuthorizationPolicies()
        {
            // Act
            ServiceExtensions.ConfigureServices(_services, _configurationMock.Object);
            var serviceProvider = _services.BuildServiceProvider();

            // Assert
            var authorizationService = serviceProvider.GetService<IAuthorizationService>();
            Assert.NotNull(authorizationService);

            var policyProvider = serviceProvider.GetService<IAuthorizationPolicyProvider>();
            var adminPolicy = policyProvider.GetPolicyAsync("Admin").Result;
            var staffPolicy = policyProvider.GetPolicyAsync("Staff").Result;
            Assert.NotNull(adminPolicy);
            Assert.NotNull(staffPolicy);
        }

        private class TestHttpContextAccessor : IHttpContextAccessor
        {
            public HttpContext HttpContext { get; set; } = new DefaultHttpContext();
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterUriServiceCorrectly()
        {
            // Arrange
            _services.AddSingleton<IHttpContextAccessor, TestHttpContextAccessor>();

            // Act
            ServiceExtensions.ConfigureServices(_services, _configurationMock.Object);
            var serviceProvider = _services.BuildServiceProvider();

            // Assert
            var uriService = serviceProvider.GetService<IUriService>();
            Assert.NotNull(uriService);
        }
    }
}
