using AssetManagement.API.Controllers;
using AssetManagement.Application.Filter;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Assignments;
using AssetManagement.Application.Models.DTOs.Assignments.Reques;
using AssetManagement.Application.Models.DTOs.Assignments.Request;
using AssetManagement.Application.Models.DTOs.Assignments.Requests;
using AssetManagement.Application.Models.DTOs.Assignments.Response;
using AssetManagement.Application.Models.Filters;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Enums;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AssetManagement.API.Tests.Controller
{
    public class AssignmentControllerTests
    {
        private readonly Mock<IAssignmentServicesAsync> _assignmentServiceMock;
        private readonly AssignmentController _assignmentController;

        public AssignmentControllerTests()
        {
            _assignmentServiceMock = new Mock<IAssignmentServicesAsync>();
            _assignmentController = new AssignmentController(_assignmentServiceMock.Object);
            _assignmentController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public async Task AddAssignment_ReturnsOkResult_WhenAssignmentIsCreatedSuccessfully()
        {
            // Arrange
            var addAssignmentRequest = new AddAssignmentRequestDto();
            var expectedResponse = new Response<AssignmentDto>
            {
                Succeeded = true
            };
            _assignmentServiceMock.Setup(x => x.AddAssignmentAsync(It.IsAny<AddAssignmentRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.AddAssignment(addAssignmentRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssignmentDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task AddAssignment_ReturnsBadRequestResult_WhenServiceResponseFails()
        {
            // Arrange
            var addAssignmentRequest = new AddAssignmentRequestDto();
            var expectedResponse = new Response<AssignmentDto>
            {
                Succeeded = false
            };
            _assignmentServiceMock.Setup(x => x.AddAssignmentAsync(It.IsAny<AddAssignmentRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.AddAssignment(addAssignmentRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssignmentDto>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }


        [Fact]
        public async Task FilterAssignment_ReturnsOkResult_WhenAssignmentsAreFetchedSuccessfully()
        {
            // Arrange
            var assignmentFilter = new AssignmentFilter
            {
                pagination = new PaginationFilter(),
                search = "",
                assignmentState = EnumAssignmentState.WaitingForAcceptance,
                dateFrom = new DateTime(2024, 1, 1),
                dateTo = new DateTime(2025, 1, 1),
                adminLocation = EnumLocation.HaNoi,
                orderBy = "assetid",
                isDescending = true
            };
            var expectedResponse = new PagedResponse<List<AssignmentResponseDto>>
            {
                Succeeded = true,
                Data = new List<AssignmentResponseDto>()
            };
            _assignmentServiceMock.Setup(x => x.GetAllAssignmentsAsync(
                It.IsAny<PaginationFilter>(),
                It.IsAny<string>(),
                It.IsAny<EnumAssignmentState?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<EnumLocation>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.FilterAssignment(assignmentFilter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<List<AssignmentResponseDto>>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task FilterAssignment_ReturnsBadRequestResult_WhenServiceResponseFails()
        {
            // Arrange
            var assignmentFilter = new AssignmentFilter
            {
                pagination = new PaginationFilter(),
                search = "",
                assignmentState = EnumAssignmentState.WaitingForAcceptance,
                dateFrom = new DateTime(2024, 1, 1),
                dateTo = new DateTime(2025, 1, 1),
                adminLocation = EnumLocation.HaNoi,
                orderBy = "assetid",
                isDescending = true
            };
            var expectedResponse = new PagedResponse<List<AssignmentResponseDto>>
            {
                Succeeded = false
            };
            _assignmentServiceMock.Setup(x => x.GetAllAssignmentsAsync(
                It.IsAny<PaginationFilter>(),
                It.IsAny<string>(),
                It.IsAny<EnumAssignmentState?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<EnumLocation>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.FilterAssignment(assignmentFilter);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<List<AssignmentResponseDto>>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }



        [Fact]
        public async Task GetAssignmentsForUser_ReturnsOkResult_WhenAssignmentsAreFetchedSuccessfully()
        {
            // Arrange
            var filterAssignmentForUser = new FilterAssignmentForUser
            {
                pagination = new PaginationFilter(),
                search = "",
                assignmentState = EnumAssignmentState.WaitingForAcceptance,
                dateFrom = new DateTime(2024, 1, 1),
                dateTo = new DateTime(2025, 1, 1),
                userId = Guid.NewGuid(),
                orderBy = "assetid",
                isDescending = true
            };
            var expectedResponse = new PagedResponse<List<AssignmentResponseDto>>
            {
                Succeeded = true,
                Data = new List<AssignmentResponseDto>()
            };
            _assignmentServiceMock.Setup(x => x.GetAssignmentsOfUser(
                It.IsAny<PaginationFilter>(),
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<EnumAssignmentState?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.GetAssignmentsForUser(filterAssignmentForUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<List<AssignmentResponseDto>>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }


        [Fact]
        public async Task GetAssignmentsForUser_ReturnsBadRequestResult_WhenServiceResponseFails()
        {
            // Arrange
            var filterAssignmentForUser = new FilterAssignmentForUser
            {
                pagination = new PaginationFilter(),
                search = "",
                assignmentState = EnumAssignmentState.WaitingForAcceptance,
                dateFrom = new DateTime(2024, 1, 1),
                dateTo = new DateTime(2025, 1, 1),
                userId = Guid.NewGuid(),
                orderBy = "assetid",
                isDescending = true
            };
            var expectedResponse = new PagedResponse<List<AssignmentResponseDto>>
            {
                Succeeded = false
            };
            _assignmentServiceMock.Setup(x => x.GetAssignmentsOfUser(It.IsAny<PaginationFilter>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<EnumAssignmentState?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.GetAssignmentsForUser(filterAssignmentForUser);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<List<AssignmentResponseDto>>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }

        // object instance null 
        [Fact]
        public async Task GetAssetAssignHistory_ReturnsOkResult_WhenAssignmentsAreFetchedSuccessfully()
        {
            // Arrange
            var filterAssetAssignHistory = new FilterAssetAssignHistory
            {
                pagination = new PaginationFilter(),
                assetId = Guid.NewGuid(),
                search = "",
                assignmentState = EnumAssignmentState.WaitingForAcceptance,
                dateFrom = new DateTime(2024, 1, 1),
                dateTo = new DateTime(2025, 1, 1),
                orderBy = "assetcode",
                isDescending = true,
            };
            var expectedResponse = new PagedResponse<List<AssignmentResponseDto>>
            {
                Succeeded = true
            };
            _assignmentServiceMock.Setup(x => x.GetAssetAssign(It.IsAny<PaginationFilter>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<EnumAssignmentState?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.GetAssetAssignHistory(filterAssetAssignHistory);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<List<AssignmentResponseDto>>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetAssetAssignHistory_ReturnsBadRequestResult_WhenServiceResponseFails()
        {
            // Arrange
            var filterAssetAssignHistory = new FilterAssetAssignHistory
            {
                pagination = new PaginationFilter(),
                assetId = Guid.NewGuid(),
                search = "",
                assignmentState = EnumAssignmentState.WaitingForAcceptance,
                dateFrom = new DateTime(2024, 1, 1),
                dateTo = new DateTime(2025, 1, 1),
                orderBy = "assetid",
                isDescending = true,
            };
            var expectedResponse = new PagedResponse<List<AssignmentResponseDto>>
            {
                Succeeded = false
            };
            _assignmentServiceMock.Setup(x => x.GetAssetAssign(It.IsAny<PaginationFilter>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<EnumAssignmentState?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.GetAssetAssignHistory(filterAssetAssignHistory);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<List<AssignmentResponseDto>>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetAssignmentById_ReturnsOkResult_WhenAssignmentIsFound()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var expectedResponse = new PagedResponse<AssignmentResponseDto>
            {
                Succeeded = true
            };
            _assignmentServiceMock.Setup(x => x.GetAssignmentByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.GetAssignmentById(assignmentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<AssignmentResponseDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetAssignmentById_ReturnsBadRequest_WhenAssignmentIsNotFound()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var expectedResponse = new PagedResponse<AssignmentResponseDto>
            {
                Succeeded = false
            };
            _assignmentServiceMock.Setup(x => x.GetAssignmentByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.GetAssignmentById(assignmentId);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<AssignmentResponseDto>>(badRequest.Value);
            Assert.False(returnValue.Succeeded);
        }



        [Fact]
        public async Task ChangeAssignmentStatus_ReturnsOkResult_WhenStatusIsChangedSuccessfully()
        {
            // Arrange
            var changeStateAssignmentRequest = new ChangeStateAssignmentDto();
            var expectedResponse = new Response<AssignmentDto>
            {
                Succeeded = true
            };
            _assignmentServiceMock.Setup(x => x.ChangeAssignmentStateAsync(It.IsAny<ChangeStateAssignmentDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.ChangeAssignmentStatus(changeStateAssignmentRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssignmentDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }



        [Fact]
        public async Task ChangeAssignmentStatus_ReturnsBadRequestResult_WhenStatusChangeFails()
        {
            // Arrange
            var changeStateAssignmentRequest = new ChangeStateAssignmentDto();
            var expectedResponse = new Response<AssignmentDto>
            {
                Succeeded = false
            };
            _assignmentServiceMock.Setup(x => x.ChangeAssignmentStateAsync(It.IsAny<ChangeStateAssignmentDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.ChangeAssignmentStatus(changeStateAssignmentRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssignmentDto>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }

        [Fact]
        public async Task EditAssignment_ReturnsOkResult_WhenAssignmentIsEditedSuccessfully()
        {
            // Arrange
            var editAssignmentRequest = new EditAssignmentRequestDto();
            var assignmentId = Guid.NewGuid();
            var expectedResponse = new Response<AssignmentDto>
            {
                Succeeded = true
            };
            _assignmentServiceMock.Setup(x => x.EditAssignmentAsync(It.IsAny<EditAssignmentRequestDto>(), It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.EditAssignment(editAssignmentRequest, assignmentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssignmentDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task EditAssignment_ReturnsBadRequestResult_WhenServiceResponseFails()
        {
            // Arrange
            var editAssignmentRequest = new EditAssignmentRequestDto();
            var assignmentId = Guid.NewGuid();
            var expectedResponse = new Response<AssignmentDto>
            {
                Succeeded = false
            };
            _assignmentServiceMock.Setup(x => x.EditAssignmentAsync(It.IsAny<EditAssignmentRequestDto>(), It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.EditAssignment(editAssignmentRequest, assignmentId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssignmentDto>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }

        [Fact]
        public async Task DeleteAssignment_ReturnsOkResult_WhenAssignmentIsDeletedSuccessfully()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var expectedResponse = new Response<AssignmentDto>
            {
                Succeeded = true
            };
            _assignmentServiceMock.Setup(x => x.DeleteAssignmentAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.DeleteAssignment(assignmentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssignmentDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task DeleteAssignment_ReturnsBadRequestResult_WhenServiceResponseFails()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var expectedResponse = new Response<AssignmentDto>
            {
                Succeeded = false
            };
            _assignmentServiceMock.Setup(x => x.DeleteAssignmentAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _assignmentController.DeleteAssignment(assignmentId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<AssignmentDto>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }
    }
}
