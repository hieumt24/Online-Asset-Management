using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AssetManagement.API.Controllers;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Users;
using AssetManagement.Application.Models.DTOs.Users.Requests;
using AssetManagement.Application.Models.DTOs.Users.Responses;
using AssetManagement.Application.Models.Filters;
using AssetManagement.Application.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using AssetManagement.Application.Filter;
using AssetManagement.Domain.Enums;

namespace AssetManagement.API.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserServiceAsync> _mockUserService;
        private readonly Mock<IValidator<AddUserRequestDto>> _mockValidator;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUserServiceAsync>();
            _mockValidator = new Mock<IValidator<AddUserRequestDto>>();
            _controller = new UsersController(_mockUserService.Object, _mockValidator.Object);
        }

        // Helper method to create DefaultHttpContext
        private DefaultHttpContext CreateHttpContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/api/v1/users";
            return httpContext;
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WhenUserIsCreatedSuccessfully()
        {
            // Arrange
            var request = new AddUserRequestDto();
            var response = new Response<UserDto> { Succeeded = true };

            _mockUserService.Setup(service => service.AddUserAsync(request)).ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.Create(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenUserCreationFails()
        {
            // Arrange
            var request = new AddUserRequestDto();
            var response = new Response<UserDto> { Succeeded = false };

            _mockUserService.Setup(service => service.AddUserAsync(request)).ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.Create(request) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOkResult_WhenUsersAreFetchedSuccessfully()
        {
            // Arrange
            var filter = new UserFilter();
            var response = new PagedResponse<List<UserResponseDto>> { Succeeded = true, Data = new List<UserResponseDto>() };

            _mockUserService.Setup(service => service.GetAllUsersAsync(It.IsAny<PaginationFilter>(), It.IsAny<string>(), It.IsAny<EnumLocation>(), It.IsAny<RoleType?>(), It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string>()))
                            .ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.GetAllUsers(filter) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsBadRequest_WhenFetchingUsersFails()
        {
            // Arrange
            var filter = new UserFilter();
            var response = new PagedResponse<List<UserResponseDto>> { Succeeded = false };

            _mockUserService.Setup(service => service.GetAllUsersAsync(It.IsAny<PaginationFilter>(), It.IsAny<string>(), It.IsAny<EnumLocation>(), It.IsAny<RoleType?>(), It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string>()))
                            .ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.GetAllUsers(filter) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkResult_WhenUserIsFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var response = new Response<UserResponseDto> { Succeeded = true };

            _mockUserService.Setup(service => service.GetUserByIdAsync(userId)).ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.GetUserById(userId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenUserIsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var response = new Response<UserResponseDto> { Succeeded = false };

            _mockUserService.Setup(service => service.GetUserByIdAsync(userId)).ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.GetUserById(userId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task UpdateUser_ReturnsOkResult_WhenUserIsUpdatedSuccessfully()
        {
            // Arrange
            var request = new EditUserRequestDto();
            var response = new Response<UserDto> { Succeeded = true };

            _mockUserService.Setup(service => service.EditUserAsync(request)).ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.UpdateUser(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task UpdateUser_ReturnsBadRequest_WhenUserUpdateFails()
        {
            // Arrange
            var request = new EditUserRequestDto();
            var response = new Response<UserDto> { Succeeded = false };

            _mockUserService.Setup(service => service.EditUserAsync(request)).ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.UpdateUser(request) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task DisableUser_ReturnsOkResult_WhenUserIsDisabledSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var response = new Response<UserDto> { Succeeded = true };

            _mockUserService.Setup(service => service.DisableUserAsync(userId)).ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.DisableUser(userId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task DisableUser_ReturnsBadRequest_WhenDisablingUserFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var response = new Response<UserDto> { Succeeded = false };

            _mockUserService.Setup(service => service.DisableUserAsync(userId)).ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.DisableUser(userId) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task ResetPassword_ReturnsOkResult_WhenPasswordIsResetSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var response = new Response<UserDto> { Succeeded = true };

            _mockUserService.Setup(service => service.ResetPasswordAsync(userId)).ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.ResetPassword(userId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task ResetPassword_ReturnsBadRequest_WhenPasswordResetFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var response = new Response<UserDto> { Succeeded = false };

            _mockUserService.Setup(service => service.ResetPasswordAsync(userId)).ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.ResetPassword(userId) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task GetUserByStaffCode_ReturnsOkResult_WhenUserIsFound()
        {
            // Arrange
            var staffCode = "staff123";
            var response = new Response<UserResponseDto> { Succeeded = true };

            _mockUserService.Setup(service => service.GetUserByStaffCodeAsync(staffCode)).ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.GetUserByStaffCode(staffCode) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task GetUserByStaffCode_ReturnsNotFound_WhenUserIsNotFound()
        {
            // Arrange
            var staffCode = "staff123";
            var response = new Response<UserResponseDto> { Succeeded = false };

            _mockUserService.Setup(service => service.GetUserByStaffCodeAsync(staffCode)).ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.GetUserByStaffCode(staffCode) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task IsValidDisableUser_ReturnsOkResult_WhenUserIsValidForDisable()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var response = new Response<bool> { Succeeded = true };

            _mockUserService.Setup(service => service.IsValidDisableUser(userId)).ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.IsValidDisableUser(userId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response, result.Value);
        }

        [Fact]
        public async Task IsValidDisableUser_ReturnsBadRequest_WhenUserIsInvalidForDisable()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var response = new Response<bool> { Succeeded = false };

            _mockUserService.Setup(service => service.IsValidDisableUser(userId)).ReturnsAsync(response);
            _controller.ControllerContext.HttpContext = CreateHttpContext();

            // Act
            var result = await _controller.IsValidDisableUser(userId) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(response, result.Value);
        }


    }
}
