using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Services;
using AssetManagement.Domain.Common.Settings;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace AssetManagement.Application.Tests.Services
{
    public class TokenServiceTests
    {
        private readonly Mock<IOptions<JWTSettings>> _jwtSettingsMock;
        private readonly JWTSettings _jwtSettings;
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            _jwtSettings = new JWTSettings
            {
                Key = "This is a test key for JWT",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpiryInMinutes = 30
            };
            _jwtSettingsMock = new Mock<IOptions<JWTSettings>>();
            _jwtSettingsMock.Setup(x => x.Value).Returns(_jwtSettings);

            _tokenService = new TokenService(_jwtSettingsMock.Object);
        }

        [Fact]
        public void GenerateJwtToken_ValidUser_ReturnsToken()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                StaffCode = "S1234",
                Location = EnumLocation.HaNoi,
                IsFirstTimeLogin = true,
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            var role = RoleType.Admin;

            // Act
            var token = _tokenService.GenerateJwtToken(user, role);

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
        }

        //[Fact]
        //public void ValidateToken_ValidToken_ReturnsUserId()
        //{
        //    // Arrange
        //    var user = new User
        //    {
        //        Id = Guid.NewGuid(),
        //        Username = "testuser",
        //        StaffCode = "S1234",
        //        Location = LocationType.HaNoi,
        //        IsFirstTimeLogin = true,
        //        DateOfBirth = new DateTime(1990, 1, 1)
        //    };
        //    var role = RoleType.Admin;

        //    var token = _tokenService.GenerateJwtToken(user, role);

        //    // Act
        //    var userId = _tokenService.ValidateToken(token);

        //    // Assert
        //    Assert.Equal(user.Id, userId);
        //}

        [Fact]
        public void ValidateToken_InvalidToken_ReturnsNull()
        {
            // Arrange
            var invalidToken = "invalid.token.string";

            // Act
            var userId = _tokenService.ValidateToken(invalidToken);

            // Assert
            Assert.Null(userId);
        }

        [Fact]
        public void ValidateToken_NullToken_ReturnsNull()
        {
            // Arrange
            string nullToken = null;

            // Act
            var userId = _tokenService.ValidateToken(nullToken);

            // Assert
            Assert.Null(userId);
        }
    }
}
