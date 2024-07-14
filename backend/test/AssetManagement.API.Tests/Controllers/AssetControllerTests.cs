using AssetManagement.API.Controllers;
using AssetManagement.Application.Filter;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Assets;
using AssetManagement.Application.Models.DTOs.Assets.Requests;
using AssetManagement.Application.Models.DTOs.Assets.Responses;
using AssetManagement.Application.Models.Filters;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AssetManagement.API.Tests.Controller
{
    public class AssetControllerTests
    {
        private readonly Mock<IAssetServiceAsync> _assetServiceMock;
        private readonly AssetController _assetController;

        public AssetControllerTests()
        {
            _assetServiceMock = new Mock<IAssetServiceAsync>();
            _assetController = new AssetController(_assetServiceMock.Object);

            _assetController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }


        [Fact]
        public async Task GetAllAsset_ReturnsOkResult_WhenServiceSucceeds()
        {
            // Arrange
            var assetFilter = new AssetFilter
            {
                pagination = new PaginationFilter(),
                search = "",
                categoryId = Guid.NewGuid(),
                assetStateType = new List<AssetStateType?>(),
                adminLocation = EnumLocation.HaNoi,
                orderBy = "firstname",
                isDescending = false
            };

            var serviceResponse = new PagedResponse<List<AssetResponseDto>>
            {
                Succeeded = true,
                Data = new List<AssetResponseDto>()
            };

            _assetServiceMock.Setup(x => x.GetAllAseets(
            It.IsAny<PaginationFilter>(),
            It.IsAny<string>(),
            It.IsAny<Guid?>(),
            It.IsAny<ICollection<AssetStateType?>>(),
            It.IsAny<EnumLocation>(),
            It.IsAny<string>(),
            It.IsAny<bool?>(),
            It.IsAny<string>()
        )).ReturnsAsync(serviceResponse);

            // Act
            var result = await _assetController.GetAllAsset(assetFilter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<List<AssetResponseDto>>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetAllAsset_ReturnsBadRequestResult_WhenServiceResponseFails()
        {
            // Arrange
            var assetFilter = new AssetFilter();
            var expectedResponse = new PagedResponse<List<AssetResponseDto>>
            {
                Succeeded = false,
                Errors = new List<string> { "Error" }
            };
            _assetServiceMock.Setup(x => x.GetAllAseets(
                It.IsAny<PaginationFilter>(),
                It.IsAny<string>(),
                It.IsAny<Guid?>(),
                It.IsAny<ICollection<AssetStateType?>>(),
                It.IsAny<EnumLocation>(),
                It.IsAny<string>(),
                It.IsAny<bool?>(),
                It.IsAny<string>()
            )).ReturnsAsync(expectedResponse);

            // Act
            var result = await _assetController.GetAllAsset(assetFilter);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<List<AssetResponseDto>>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
            Assert.Contains("Error", returnValue.Errors);
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WhenAssetIsCreatedSuccessfully()
        {
            // Arrange
            var addAssetRequest = new AddAssetRequestDto();
            var expectedResponse = new Response<AssetDto>
            {
                Succeeded = true
            };
            _assetServiceMock.Setup(x => x.AddAssetAsync(It.IsAny<AddAssetRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assetController.Create(addAssetRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssetDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task Create_ReturnsBadRequestResult_WhenServiceResponseFails()
        {
            // Arrange
            var addAssetRequest = new AddAssetRequestDto();
            var expectedResponse = new Response<AssetDto>
            {
                Succeeded = false
            };
            _assetServiceMock.Setup(x => x.AddAssetAsync(It.IsAny<AddAssetRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assetController.Create(addAssetRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssetDto>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }

        [Fact]
        public async Task Edit_ReturnsOkResult_WhenAssetIsEditedSuccessfully()
        {
            // Arrange
            var editAssetRequest = new EditAssetRequestDto();
            var assetId = Guid.NewGuid();
            var expectedResponse = new Response<AssetDto>
            {
                Succeeded = true
            };
            _assetServiceMock.Setup(x => x.EditAssetAsync(It.IsAny<Guid>(), It.IsAny<EditAssetRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assetController.Edit(assetId, editAssetRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssetDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task Edit_ReturnsBadRequestResult_WhenServiceResponseFails()
        {
            // Arrange
            var editAssetRequest = new EditAssetRequestDto();
            var assetId = Guid.NewGuid();
            var expectedResponse = new Response<AssetDto>
            {
                Succeeded = false
            };
            _assetServiceMock.Setup(x => x.EditAssetAsync(It.IsAny<Guid>(), It.IsAny<EditAssetRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assetController.Edit(assetId, editAssetRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssetDto>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }

        [Fact]
        public async Task DeleteAsset_ReturnsOkResult_WhenAssetIsDeletedSuccessfully()
        {
            // Arrange
            var assetId = Guid.NewGuid();
            var expectedResponse = new Response<AssetDto>
            {
                Succeeded = true
            };
            _assetServiceMock.Setup(x => x.DeleteAssetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assetController.DeleteAsset(assetId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssetDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task DeleteAsset_ReturnsNotFoundResult_WhenServiceResponseFails()
        {
            // Arrange
            var assetId = Guid.NewGuid();
            var expectedResponse = new Response<AssetDto>
            {
                Succeeded = false
            };
            _assetServiceMock.Setup(x => x.DeleteAssetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assetController.DeleteAsset(assetId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssetDto>>(notFoundResult.Value);
            Assert.False(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetAssetById_ReturnsOkResult_WhenAssetIsFound()
        {
            // Arrange
            var assetId = Guid.NewGuid();
            var expectedResponse = new Response<AssetDto>
            {
                Succeeded = true
            };
            _assetServiceMock.Setup(x => x.GetAssetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assetController.GetAssetById(assetId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssetDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetAssetById_ReturnsNotFoundResult_WhenAssetIsNotFound()
        {
            // Arrange
            var assetId = Guid.NewGuid();
            var expectedResponse = new Response<AssetDto>
            {
                Succeeded = false
            };
            _assetServiceMock.Setup(x => x.GetAssetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assetController.GetAssetById(assetId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssetDto>>(notFoundResult.Value);
            Assert.False(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetUserByAssetCode_ReturnsOkResult_WhenAssetIsFound()
        {
            // Arrange
            var assetCode = "ASSET123";
            var expectedResponse = new Response<AssetDto>
            {
                Succeeded = true
            };
            _assetServiceMock.Setup(x => x.GetAssetByAssetCode(It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assetController.GetUserByAssetCode(assetCode);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssetDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetUserByAssetCode_ReturnsNotFoundResult_WhenAssetIsNotFound()
        {
            // Arrange
            var assetCode = "ASSET123";
            var expectedResponse = new Response<AssetDto>
            {
                Succeeded = false
            };
            _assetServiceMock.Setup(x => x.GetAssetByAssetCode(It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assetController.GetUserByAssetCode(assetCode);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssetDto>>(notFoundResult.Value);
            Assert.False(returnValue.Succeeded);
        }


        [Fact]
        public async Task IsValidDeleteAsset_ReturnsOkResult_WhenServiceSucceeds()
        {
            // Arrange
            var assetId = Guid.NewGuid();
            var serviceResponse = new Response<bool>
            {
                Succeeded = true,
                Data = true
            };

            _assetServiceMock.Setup(x => x.IsValidDeleteAsset(assetId)).ReturnsAsync(serviceResponse);

            // Act
            var result = await _assetController.IsValidDeleteAsset(assetId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
            Assert.True(returnValue.Data);
        }

        [Fact]
        public async Task IsValidDeleteAsset_ReturnsBadRequestResult_WhenServiceFails()
        {
            // Arrange
            var assetId = Guid.NewGuid();
            var serviceResponse = new Response<bool>
            {
                Succeeded = false,
                Data = false,
                Errors = new List<string> { "Asset cannot be deleted." }
            };

            _assetServiceMock.Setup(x => x.IsValidDeleteAsset(assetId)).ReturnsAsync(serviceResponse);

            // Act
            var result = await _assetController.IsValidDeleteAsset(assetId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<bool>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
            Assert.Contains("Asset cannot be deleted.", returnValue.Errors);
        }
    }
}
