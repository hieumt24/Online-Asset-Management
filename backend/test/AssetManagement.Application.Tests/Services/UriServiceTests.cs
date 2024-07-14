using AssetManagement.Application.Filter;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Services;
using System;
using Xunit;
using static System.Net.WebRequestMethods;

namespace AssetManagement.Application.Tests.Services
{
    public class UriServiceTests
    {
        private readonly IUriService _uriService;

        public UriServiceTests()
        {
            // Setup base URI for testing
            var baseUri = "http://localhost:5000";
            _uriService = new UriService(baseUri);
        }

        [Fact]
        public void GetPageUri_ValidInput_ReturnsCorrectUri()
        {
            // Arrange
            var paginationFilter = new PaginationFilter { PageIndex = 2, PageSize = 10 };
            var route = "/api/test";

            // Act
            var resultUri = _uriService.GetPageUri(paginationFilter, route);

            // Assert
            var expectedUri = "http://localhost:5000/api/test?pageNumber=2&pageSize=10";
            Assert.Equal(expectedUri, resultUri.ToString());
        }

        [Fact]
        public void GetPageUri_BaseUriEndsWithSlash_ReturnsCorrectUri()
        {
            // Arrange
            var uriServiceWithSlash = new UriService("http://localhost:5000/");
            var paginationFilter = new PaginationFilter { PageIndex = 2, PageSize = 10 };
            var route = "api/test";

            // Act
            var resultUri = uriServiceWithSlash.GetPageUri(paginationFilter, route);

            // Assert
            var expectedUri = "http://localhost:5000/api/test?pageNumber=2&pageSize=10";
            Assert.Equal(expectedUri, resultUri.ToString());
        }

        [Fact]
        public void GetPageUri_RouteWithQueryString_ReturnsCorrectUri()
        {
            // Arrange
            var paginationFilter = new PaginationFilter { PageIndex = 1, PageSize = 5 };
            var route = "/api/test?existingParam=value";

            // Act
            var resultUri = _uriService.GetPageUri(paginationFilter, route);

            // Assert
            var expectedUri = "http://localhost:5000/api/test?existingParam=value&pageNumber=1&pageSize=5";
            Assert.Equal(expectedUri, resultUri.ToString());
        }

        [Fact]
        public void GetPageUri_NullRoute_ThrowsArgumentNullException()
        {
            // Arrange
            var paginationFilter = new PaginationFilter { PageIndex = 1, PageSize = 10 };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _uriService.GetPageUri(paginationFilter, null));
        }


        [Fact]
        public void GetPageUri_ValidRoute_ReturnsCorrectUri()
        {
            // Arrange
            var paginationFilter = new PaginationFilter { PageIndex = 3, PageSize = 15 };
            var route = "api/test";

            // Act
            var resultUri = _uriService.GetPageUri(paginationFilter, route);

            // Assert
            var expectedUri = $"{"http://localhost:5000".TrimEnd('/')}/{route.TrimStart('/')}?PageIndex=3&pageSize=15";
            Assert.Equal(expectedUri, resultUri.ToString());
        }

    }
}
