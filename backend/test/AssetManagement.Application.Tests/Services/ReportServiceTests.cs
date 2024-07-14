//using AssetManagement.Application.Filter;
//using AssetManagement.Application.Helper;
//using AssetManagement.Application.Interfaces.Repositories;
//using AssetManagement.Application.Interfaces.Services;
//using AssetManagement.Application.Models.DTOs.Reports.Responses;
//using AssetManagement.Application.Services;
//using AssetManagement.Application.Wrappers;
//using AssetManagement.Domain.Entites;
//using AssetManagement.Domain.Enums;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;

//namespace AssetManagement.Application.Tests.Services
//{
//    public class ReportServiceTests
//    {
//        private readonly Mock<ICategoryRepositoriesAsync> _categoryRepositoryMock;
//        private readonly Mock<IAssetRepositoriesAsync> _assetRepositoryMock;
//        private readonly Mock<IUriService> _uriServiceMock;
//        private readonly ReportService _reportService;

//        public ReportServiceTests()
//        {
//            _categoryRepositoryMock = new Mock<ICategoryRepositoriesAsync>();
//            _assetRepositoryMock = new Mock<IAssetRepositoriesAsync>();
//            _uriServiceMock = new Mock<IUriService>();

//            _reportService = new ReportService(
//                _categoryRepositoryMock.Object,
//                _assetRepositoryMock.Object,
//                _uriServiceMock.Object
//            );
//        }

//        [Fact]
//        public async Task GetReportAsync_ValidLocation_ReturnsReport()
//        {
//            // Arrange
//            var location = EnumLocation.HaNoi;
//            var categories = new List<Category>
//            {
//                new Category { Id = Guid.NewGuid(), CategoryName = "Category A" },
//                new Category { Id = Guid.NewGuid(), CategoryName = "Category B" }
//            };

//            var assets = new List<Asset>
//            {
//                new Asset { Id = Guid.NewGuid(), CategoryId = categories[0].Id, State = AssetStateType.Assigned },
//                new Asset { Id = Guid.NewGuid(), CategoryId = categories[0].Id, State = AssetStateType.Available },
//                new Asset { Id = Guid.NewGuid(), CategoryId = categories[1].Id, State = AssetStateType.NotAvailable },
//                new Asset { Id = Guid.NewGuid(), CategoryId = categories[1].Id, State = AssetStateType.Recycled }
//            };

//            _categoryRepositoryMock.Setup(r => r.ListAllAsync())
//                .ReturnsAsync(categories);

//            _assetRepositoryMock.Setup(r => r.ListAsync(It.IsAny<AssetSpecificationHelper>()))
//                .ReturnsAsync(assets);

//            // Act
//            var result = await _reportService.GetReportAsync(location);

//            // Assert
//            Assert.True(result.Succeeded);
//            Assert.Equal(2, result.Data.Count);
//            Assert.Equal("Category A", result.Data[0].CategoryName);
//            Assert.Equal("Category B", result.Data[1].CategoryName);
//        }

//        [Fact]
//        public async Task GetReportAsync_EmptyCategories_ReturnsEmptyReport()
//        {
//            // Arrange
//            var location = EnumLocation.HaNoi;
//            var categories = new List<Category>();
//            var assets = new List<Asset>();

//            _categoryRepositoryMock.Setup(r => r.ListAllAsync())
//                .ReturnsAsync(categories);

//            _assetRepositoryMock.Setup(r => r.ListAsync(It.IsAny<AssetSpecificationHelper>()))
//                .ReturnsAsync(assets);

//            // Act
//            var result = await _reportService.GetReportAsync(location);

//            // Assert
//            Assert.True(result.Succeeded);
//            Assert.Empty(result.Data);
//        }

//        [Fact]
//        public async Task GetReportPaginationAsync_ValidLocation_ReturnsPagedReport()
//        {
//            // Arrange
//            var location = EnumLocation.HaNoi;
//            var pagination = new PaginationFilter(1, 10);
//            var categories = new List<Category>
//            {
//                new Category { Id = Guid.NewGuid(), CategoryName = "Category A" },
//                new Category { Id = Guid.NewGuid(), CategoryName = "Category B" }
//            };

//            var assets = new List<Asset>
//            {
//                new Asset { Id = Guid.NewGuid(), CategoryId = categories[0].Id, State = AssetStateType.Assigned },
//                new Asset { Id = Guid.NewGuid(), CategoryId = categories[0].Id, State = AssetStateType.Available },
//                new Asset { Id = Guid.NewGuid(), CategoryId = categories[1].Id, State = AssetStateType.NotAvailable },
//                new Asset { Id = Guid.NewGuid(), CategoryId = categories[1].Id, State = AssetStateType.Recycled }
//            };

//            _categoryRepositoryMock.Setup(r => r.ListAllAsync())
//                .ReturnsAsync(categories);

//            _assetRepositoryMock.Setup(r => r.ListAsync(It.IsAny<AssetSpecificationHelper>()))
//                .ReturnsAsync(assets);

//            // Act
//            var result = await _reportService.GetReportPaginationAsync(location, pagination, null, null, null);

//            // Assert
//            Assert.NotNull(result);
//            Assert.True(result.Succeeded);
//            Assert.Equal(2, result.Data.Count);
//        }

//        [Fact]
//        public async Task ExportReportToExcelAsync_ValidLocation_ReturnsExcelFile()
//        {
//            // Arrange
//            var location = EnumLocation.HaNoi;
//            var categories = new List<Category>
//            {
//                new Category { Id = Guid.NewGuid(), CategoryName = "Category A" },
//                new Category { Id = Guid.NewGuid(), CategoryName = "Category B" }
//            };

//            var assets = new List<Asset>
//            {
//                new Asset { Id = Guid.NewGuid(), CategoryId = categories[0].Id, State = AssetStateType.Assigned },
//                new Asset { Id = Guid.NewGuid(), CategoryId = categories[0].Id, State = AssetStateType.Available },
//                new Asset { Id = Guid.NewGuid(), CategoryId = categories[1].Id, State = AssetStateType.NotAvailable },
//                new Asset { Id = Guid.NewGuid(), CategoryId = categories[1].Id, State = AssetStateType.Recycled }
//            };

//            _categoryRepositoryMock.Setup(r => r.ListAllAsync())
//                .ReturnsAsync(categories);

//            _assetRepositoryMock.Setup(r => r.ListAsync(It.IsAny<AssetSpecificationHelper>()))
//                .ReturnsAsync(assets);

//            // Act
//            var result = await _reportService.ExportReportToExcelAsync(location);

//            // Assert
//            Assert.NotNull(result);
//            Assert.True(result.Length > 0);
//        }
//    }
//}
