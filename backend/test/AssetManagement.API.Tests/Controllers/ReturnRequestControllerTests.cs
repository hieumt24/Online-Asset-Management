using AssetManagement.API.Controllers;
using AssetManagement.Application.Filter;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.ReturnRequests;
using AssetManagement.Application.Models.DTOs.ReturnRequests.Reponses;
using AssetManagement.Application.Models.DTOs.ReturnRequests.Request;
using AssetManagement.Application.Models.Filters;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AssetManagement.API.Tests.Controller
{
    public class ReturnRequestControllerTests
    {
        private readonly Mock<IReturnRequestServiceAsync> _returnRequestServiceMock;
        private readonly ReturnRequestController _controller;

        public ReturnRequestControllerTests()
        {
            _returnRequestServiceMock = new Mock<IReturnRequestServiceAsync>();
            _controller = new ReturnRequestController(_returnRequestServiceMock.Object);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/api/v1/returnRequests";
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WhenReturnRequestsAreFetchedSuccessfully()
        {
            // Arrange
            var filter = new ReturnRequestFilter
            {
                pagination = new PaginationFilter(),
                returnState = EnumReturnRequestState.WaitingForReturning,
                returnedDateFrom = new DateTime(2024, 1, 1),
                returnedDateTo = new DateTime(2024, 1, 1),
                search = "",
                orderBy = "assetcode",
                location = EnumLocation.HaNoi,
                isDescending = true,
            };
            var expectedResponse = new PagedResponse<List<ReturnRequestResponseDto>>
            {
                Succeeded = true,
                Data = new List<ReturnRequestResponseDto>()
            };
            _returnRequestServiceMock.Setup(x => x.GetAllReturnRequestAsync(It.IsAny<PaginationFilter>(), It.IsAny<string>(), It.IsAny<EnumReturnRequestState?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<EnumLocation>(), It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAll(filter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<List<ReturnRequestResponseDto>>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetAll_ReturnsBadRequest_WhenReturnRequestsFetchingFails()
        {
            // Arrange
            var filter = new ReturnRequestFilter
            {
                pagination = new PaginationFilter(),
                returnState = EnumReturnRequestState.WaitingForReturning,
                returnedDateFrom = new DateTime(2024, 1, 1),
                returnedDateTo = new DateTime(2024, 1, 1),
                search = "",
                orderBy = "assetcode",
                location = EnumLocation.HaNoi,
                isDescending = true,
            };
            var expectedResponse = new PagedResponse<List<ReturnRequestResponseDto>>
            {
                Succeeded = false,
                Data = null
            };
            _returnRequestServiceMock.Setup(x => x.GetAllReturnRequestAsync(It.IsAny<PaginationFilter>(), It.IsAny<string>(), It.IsAny<EnumReturnRequestState?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<EnumLocation>(), It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAll(filter);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<List<ReturnRequestResponseDto>>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WhenReturnRequestIsCreatedSuccessfully()
        {
            // Arrange
            var request = new AddReturnRequestDto { /* set properties as needed */ };
            var expectedResponse = new Response<ReturnRequestDto>
            {
                Succeeded = true,
                Data = new ReturnRequestDto()
            };
            _returnRequestServiceMock.Setup(x => x.AddReturnRequestAsync(It.IsAny<AddReturnRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<ReturnRequestDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenReturnRequestCreationFails()
        {
            // Arrange
            var request = new AddReturnRequestDto { /* set properties as needed */ };
            var expectedResponse = new Response<ReturnRequestDto>
            {
                Succeeded = false,
                Data = null
            };
            _returnRequestServiceMock.Setup(x => x.AddReturnRequestAsync(It.IsAny<AddReturnRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<ReturnRequestDto>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }

        [Fact]
        public async Task CancelReturnRequest_ReturnsOkResult_WhenReturnRequestIsCancelledSuccessfully()
        {
            // Arrange
            var returnRequestId = Guid.NewGuid();
            var expectedResponse = new Response<string>
            {
                Succeeded = true,
                Data = "Cancel Return Request Successfully."
            };
            _returnRequestServiceMock.Setup(x => x.CancelReturnRequestAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CancelReturnRequest(returnRequestId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<string>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task CancelReturnRequest_ReturnsBadRequest_WhenReturnRequestCancellationFails()
        {
            // Arrange
            var returnRequestId = Guid.NewGuid();
            var expectedResponse = new Response<string>
            {
                Succeeded = false,
                Data = "Cancel Return Request failed."
            };
            _returnRequestServiceMock.Setup(x => x.CancelReturnRequestAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CancelReturnRequest(returnRequestId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }

        [Fact]
        public async Task ChangeReturnRequestStatus_ReturnsOkResult_WhenStatusIsChangedSuccessfully()
        {
            // Arrange
            var request = new ChangeStateReturnRequestDto { /* set properties as needed */ };
            var expectedResponse = new Response<ReturnRequestDto>
            {
                Succeeded = true,
                Data = new ReturnRequestDto()
            };
            _returnRequestServiceMock.Setup(x => x.ChangeAssignmentStateAsync(It.IsAny<ChangeStateReturnRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.ChangeReturnRequestStatus(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<ReturnRequestDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task ChangeReturnRequestStatus_ReturnsBadRequest_WhenStatusChangeFails()
        {
            // Arrange
            var request = new ChangeStateReturnRequestDto { /* set properties as needed */ };
            var expectedResponse = new Response<ReturnRequestDto>
            {
                Succeeded = false,
                Data = null
            };
            _returnRequestServiceMock.Setup(x => x.ChangeAssignmentStateAsync(It.IsAny<ChangeStateReturnRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.ChangeReturnRequestStatus(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<ReturnRequestDto>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }
    }
}
