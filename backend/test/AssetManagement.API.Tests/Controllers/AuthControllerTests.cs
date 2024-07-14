using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using AssetManagement.API.Controllers;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Users.Requests;
using AssetManagement.Application.Models.DTOs.Users.Responses;
using AssetManagement.Application.Wrappers;
using System.Threading.Tasks;
using System;
using FluentValidation.Results;

namespace AssetManagement.API.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAccountServicecs> _mockAuthService;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAccountServicecs>();
            _authController = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginRequest = new AuthenticationRequest
            {
                Username = "testuser",
                Password = "password"
            };

            var authResponse = new AuthenticationResponse
            {
                Username = loginRequest.Username,
                IsFirstTimeLogin = false,
                Role = "User",
                Token = "sample_token"
            };

            _mockAuthService.Setup(service => service.LoginAsync(loginRequest))
                            .ReturnsAsync(new Response<AuthenticationResponse> { Succeeded = true, Data = authResponse });

            // Act
            var result = await _authController.Login(loginRequest) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var authModel = Assert.IsType<Response<AuthenticationResponse>>(result.Value);
            Assert.Equal(authResponse.Username, authModel.Data.Username);
            Assert.Equal(authResponse.Role, authModel.Data.Role);
            Assert.Equal(authResponse.Token, authModel.Data.Token);
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenLoginFails()
        {
            // Arrange
            var loginRequest = new AuthenticationRequest
            {
                Username = "invaliduser",
                Password = "invalidpassword"
            };

            _mockAuthService.Setup(service => service.LoginAsync(loginRequest))
                            .ReturnsAsync(new Response<AuthenticationResponse> { Succeeded = false, Message = "Invalid username or password" });

            // Act
            var result = await _authController.Login(loginRequest) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            var response = Assert.IsType<Response<AuthenticationResponse>>(result.Value);
            Assert.False(response.Succeeded);
            Assert.Equal("Invalid username or password", response.Message);
        }

        [Fact]
        public async Task ChangePassword_ReturnsOkResult_WhenChangePasswordIsSuccessful()
        {
            // Arrange
            var changePasswordRequest = new ChangePasswordRequest
            {
                Username = "testuser",
                CurrentPassword = "oldpassword",
                NewPassword = "newpassword"
            };

            var response = new Response<string>
            {
                Succeeded = true,
                Message = "Password changed successfully"
            };

            _mockAuthService.Setup(service => service.ChangePasswordAsync(changePasswordRequest))
                            .ReturnsAsync(response);

            // Act
            var result = await _authController.ChangePassword(changePasswordRequest) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var responseModel = Assert.IsType<Response<string>>(result.Value);
            Assert.True(responseModel.Succeeded);
            Assert.Equal("Password changed successfully", responseModel.Message);
        }

        [Fact]
        public async Task ChangePassword_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var changePasswordRequest = new ChangePasswordRequest
            {
                Username = "testuser",
                CurrentPassword = "oldpassword",
                NewPassword = "short" // This will fail validation as it does not meet the MaxLength requirement
            };

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("NewPassword", "The NewPassword field must be a string with a maximum length of 50.")
            };

            var validationResult = new ValidationResult(validationFailures);

            _mockAuthService.Setup(service => service.ChangePasswordAsync(changePasswordRequest))
                            .ReturnsAsync(new Response<string> { Succeeded = false, Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList() });

            // Act
            var result = await _authController.ChangePassword(changePasswordRequest) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            var responseModel = Assert.IsType<Response<string>>(result.Value);
            Assert.False(responseModel.Succeeded);
            Assert.Contains("The NewPassword field must be a string with a maximum length of 50.", responseModel.Errors);
        }

        [Fact]
        public async Task ChangePassword_ReturnsBadRequest_WhenChangePasswordFails()
        {
            // Arrange
            var changePasswordRequest = new ChangePasswordRequest
            {
                Username = "testuser",
                CurrentPassword = "oldpassword",
                NewPassword = "newpassword"
            };

            _mockAuthService.Setup(service => service.ChangePasswordAsync(changePasswordRequest))
                            .ReturnsAsync(new Response<string> { Succeeded = false, Message = "Current password is incorrect" });

            // Act
            var result = await _authController.ChangePassword(changePasswordRequest) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            var responseModel = Assert.IsType<Response<string>>(result.Value);
            Assert.False(responseModel.Succeeded);
            Assert.Equal("Current password is incorrect", responseModel.Message);
        }


    }

}

