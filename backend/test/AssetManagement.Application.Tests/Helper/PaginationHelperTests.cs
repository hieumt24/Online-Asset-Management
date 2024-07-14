using AssetManagement.Application.Filter;
using AssetManagement.Application.Helper;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Wrappers;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace AssetManagement.Application.Tests.Helper
{
    public class PaginationHelperTests
    {
        private readonly Mock<IUriService> _uriServiceMock;

        public PaginationHelperTests()
        {
            _uriServiceMock = new Mock<IUriService>();
        }

        [Fact]
        public void CreatePagedReponse_ShouldReturnCorrectPagedResponse_WhenTotalRecordsAreLessThanPageSize()
        {
            // Arrange
            var pagedData = new List<string> { "item1", "item2" };
            var paginationFilter = new PaginationFilter { PageIndex = 1, PageSize = 10 };
            var totalRecords = 2;
            var route = "api/items";

            _uriServiceMock.Setup(x => x.GetPageUri(It.IsAny<PaginationFilter>(), route))
                .Returns((PaginationFilter filter, string r) => new Uri($"http://localhost/{r}?pageIndex={filter.PageIndex}&pageSize={filter.PageSize}"));

            // Act
            var result = PaginationHelper.CreatePagedReponse(pagedData, paginationFilter, totalRecords, _uriServiceMock.Object, route);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(pagedData);
            result.PageIndex.Should().Be(paginationFilter.PageIndex);
            result.PageSize.Should().Be(paginationFilter.PageSize);
            result.TotalPages.Should().Be(1);
            result.TotalRecords.Should().Be(totalRecords);
            result.NextPage.Should().BeNull();
            result.PreviousPage.Should().BeNull();
            result.FirstPage.Should().Be(new Uri("http://localhost/api/items?pageIndex=1&pageSize=10"));
            result.LastPage.Should().Be(new Uri("http://localhost/api/items?pageIndex=1&pageSize=10"));
        }

        [Fact]
        public void CreatePagedReponse_ShouldReturnCorrectPagedResponse_WhenTotalRecordsAreGreaterThanPageSize()
        {
            // Arrange
            var pagedData = new List<string> { "item1", "item2" };
            var paginationFilter = new PaginationFilter { PageIndex = 1, PageSize = 1 };
            var totalRecords = 2;
            var route = "api/items";

            _uriServiceMock.Setup(x => x.GetPageUri(It.IsAny<PaginationFilter>(), route))
                .Returns((PaginationFilter filter, string r) => new Uri($"http://localhost/{r}?pageIndex={filter.PageIndex}&pageSize={filter.PageSize}"));

            // Act
            var result = PaginationHelper.CreatePagedReponse(pagedData, paginationFilter, totalRecords, _uriServiceMock.Object, route);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(pagedData);
            result.PageIndex.Should().Be(paginationFilter.PageIndex);
            result.PageSize.Should().Be(paginationFilter.PageSize);
            result.TotalPages.Should().Be(2);
            result.TotalRecords.Should().Be(totalRecords);
            result.NextPage.Should().Be(new Uri("http://localhost/api/items?pageIndex=2&pageSize=1"));
            result.PreviousPage.Should().BeNull();
            result.FirstPage.Should().Be(new Uri("http://localhost/api/items?pageIndex=1&pageSize=1"));
            result.LastPage.Should().Be(new Uri("http://localhost/api/items?pageIndex=2&pageSize=1"));
        }

        [Fact]
        public void CreatePagedReponse_ShouldReturnCorrectPagedResponse_WhenOnLastPage()
        {
            // Arrange
            var pagedData = new List<string> { "item1", "item2" };
            var paginationFilter = new PaginationFilter { PageIndex = 2, PageSize = 1 };
            var totalRecords = 2;
            var route = "api/items";

            _uriServiceMock.Setup(x => x.GetPageUri(It.IsAny<PaginationFilter>(), route))
                .Returns((PaginationFilter filter, string r) => new Uri($"http://localhost/{r}?pageIndex={filter.PageIndex}&pageSize={filter.PageSize}"));

            // Act
            var result = PaginationHelper.CreatePagedReponse(pagedData, paginationFilter, totalRecords, _uriServiceMock.Object, route);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(pagedData);
            result.PageIndex.Should().Be(paginationFilter.PageIndex);
            result.PageSize.Should().Be(paginationFilter.PageSize);
            result.TotalPages.Should().Be(2);
            result.TotalRecords.Should().Be(totalRecords);
            result.NextPage.Should().BeNull();
            result.PreviousPage.Should().Be(new Uri("http://localhost/api/items?pageIndex=1&pageSize=1"));
            result.FirstPage.Should().Be(new Uri("http://localhost/api/items?pageIndex=1&pageSize=1"));
            result.LastPage.Should().Be(new Uri("http://localhost/api/items?pageIndex=2&pageSize=1"));
        }
    }
}
