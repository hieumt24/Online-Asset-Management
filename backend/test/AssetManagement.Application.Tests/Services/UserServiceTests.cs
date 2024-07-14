using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Users;
using AssetManagement.Application.Models.DTOs.Users.Requests;
using AssetManagement.Application.Models.DTOs.Users.Responses;
using AssetManagement.Application.Services;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;
using AutoMapper;
using FluentValidation;
using Moq;

namespace AssetManagement.Tests.Services
{
    public class UserServiceAsyncTests
    {
        private readonly Mock<IUserRepositoriesAsync> _mockUserRepositories;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<AddUserRequestDto>> _mockAddUserValidator;
        private readonly Mock<IValidator<EditUserRequestDto>> _mockEditUserValidator;
        private readonly Mock<IUriService> _mockUriService;
        private readonly Mock<IAssignmentRepositoriesAsync> _mockAssignmentRepositories;
        private readonly Mock<ITokenRepositoriesAsync> _mockTokenRepositories;
        private readonly Mock<IBlackListTokensRepositoriesAsync> _mockBlackListTokensRepositories;
        private readonly UserServiceAsync _userService;

        public UserServiceAsyncTests()
        {
            _mockUserRepositories = new Mock<IUserRepositoriesAsync>();
            _mockMapper = new Mock<IMapper>();
            _mockAddUserValidator = new Mock<IValidator<AddUserRequestDto>>();
            _mockEditUserValidator = new Mock<IValidator<EditUserRequestDto>>();
            _mockUriService = new Mock<IUriService>();
            _mockAssignmentRepositories = new Mock<IAssignmentRepositoriesAsync>();
            _mockTokenRepositories = new Mock<ITokenRepositoriesAsync>();
            _mockBlackListTokensRepositories = new Mock<IBlackListTokensRepositoriesAsync>();

            _userService = new UserServiceAsync(
                _mockUserRepositories.Object,
                _mockMapper.Object,
                _mockAddUserValidator.Object,
                _mockEditUserValidator.Object,
                _mockUriService.Object,
                _mockAssignmentRepositories.Object,
                _mockTokenRepositories.Object,
                _mockBlackListTokensRepositories.Object
            );
        }

        [Fact]
        public async Task AddUserAsync_ShouldReturnSuccess_WhenUserIsValid()
        {
            // Arrange
            var request = new AddUserRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-30)
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-30)
            };

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = "John",
                LastName = "Doe"
            };

            _mockAddUserValidator.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockMapper.Setup(m => m.Map<User>(request)).Returns(user);

            _mockUserRepositories.Setup(r => r.GenerateUsername(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("johndoe");

            _mockUserRepositories.Setup(r => r.GeneratePassword(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns("GeneratedPassword123!");

            _mockUserRepositories.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(user);

            _mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.AddUserAsync(request);

            // Assert
            Assert.True(result.Succeeded);
        }


        [Fact]
        public async Task AddUserAsync_ShouldReturnFailure_WhenUserIsInvalid()
        {
            // Arrange
            var request = new AddUserRequestDto { FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.Now.AddYears(-30) };
            var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("FirstName", "FirstName is required")
            });

            _mockAddUserValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);

            // Act
            var result = await _userService.AddUserAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains("FirstName is required", result.Errors);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, FirstName = "John", LastName = "Doe" };
            var userDto = new UserResponseDto { Id = userId, FirstName = "John", LastName = "Doe" };

            _mockUserRepositories.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<UserResponseDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(userDto, result.Data);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserRepositories.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("User not found", result.Message);
        }

        [Fact]
        public async Task EditUserAsync_ShouldReturnSuccess_WhenUserIsValid()
        {
            // Arrange
            var request = new EditUserRequestDto { UserId = Guid.NewGuid(), DateOfBirth = DateTime.Now.AddYears(-30) };
            var user = new User { Id = request.UserId, FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.Now.AddYears(-30) };
            var userDto = new UserDto { Id = request.UserId, FirstName = "John", LastName = "Doe" };

            _mockEditUserValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _mockUserRepositories.Setup(r => r.GetByIdAsync(request.UserId)).ReturnsAsync(user);
            _mockUserRepositories.Setup(r => r.UpdateAsync(user)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.EditUserAsync(request);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(userDto, result.Data);
        }

        [Fact]
        public async Task EditUserAsync_ShouldReturnFailure_WhenUserIsInvalid()
        {
            // Arrange
            var request = new EditUserRequestDto {UserId = Guid.NewGuid(), DateOfBirth = DateTime.Now.AddYears(-30) };
            var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("FirstName", "FirstName is required")
            });

            _mockEditUserValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);

            // Act
            var result = await _userService.EditUserAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains("FirstName is required", result.Errors);
        }

        [Fact]
        public async Task DisableUserAsync_ShouldReturnSuccess_WhenUserIsDisabled()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, FirstName = "John", LastName = "Doe" };

            _mockUserRepositories.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockUserRepositories.Setup(r => r.DeleteAsync(user.Id)).ReturnsAsync(user);

            // Act
            var result = await _userService.DisableUserAsync(userId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Disable user successfully", result.Message);
        }

        [Fact]
        public async Task DisableUserAsync_ShouldReturnFailure_WhenUserHasAssignments()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, FirstName = "John", LastName = "Doe" };
            var assignments = new List<Assignment> { new Assignment { Id = Guid.NewGuid(), AssignedIdTo = userId } };

            _mockUserRepositories.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockAssignmentRepositories.Setup(r => r.GetAssignmentsByUserId(userId)).ReturnsAsync(assignments.AsQueryable()); 

            // Act
            var result = await _userService.DisableUserAsync(userId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("There are valid assignments belonging to this user. Please close all assignments before disabling user.", result.Message);
        }




        [Fact]
        public async Task ResetPasswordAsync_ShouldReturnSuccess_WhenPasswordIsReset()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, FirstName = "John", LastName = "Doe", Username = "johndoe", DateOfBirth = DateTime.Now.AddYears(-30) };
            var userDto = new UserDto { Id = userId, FirstName = "John", LastName = "Doe" };

            // Mocking GetByIdAsync to return the user
            _mockUserRepositories.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // Assuming UpdateAsync is used to update the user's password
            _mockUserRepositories.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(user);

            // Act
            var result = await _userService.ResetPasswordAsync(userId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Password has been reset successfully", result.Message);
            Assert.Equal(userDto.Id, result.Data.Id);
            Assert.Equal(userDto.FirstName, result.Data.FirstName);
            Assert.Equal(userDto.LastName, result.Data.LastName);
        }

        [Fact]
        public async Task ResetPasswordAsync_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockUserRepositories.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.ResetPasswordAsync(userId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("User not found", result.Message);
        }
    }
}
