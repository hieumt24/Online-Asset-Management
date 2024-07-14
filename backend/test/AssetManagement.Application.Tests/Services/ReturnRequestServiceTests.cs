using AssetManagement.Application.Filter;
using AssetManagement.Application.Helper;
using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.ReturnRequests;
using AssetManagement.Application.Models.DTOs.ReturnRequests.Reponses;
using AssetManagement.Application.Models.DTOs.ReturnRequests.Request;
using AssetManagement.Application.Services;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;
using AutoMapper;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AssetManagement.Application.Tests.Services
{
    public class ReturnRequestServiceTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IReturnRequestRepositoriesAsync> _returnRequestRepositoryMock;
        private readonly Mock<IUserRepositoriesAsync> _userRepositoryMock;
        private readonly Mock<IAssignmentRepositoriesAsync> _assignmentRepositoryMock;
        private readonly Mock<IValidator<AddReturnRequestDto>> _addReturnRequestValidatorMock;
        private readonly Mock<IUriService> _uriServiceMock;
        private readonly Mock<IAssetRepositoriesAsync> _assetRepositoryMock;
        private readonly Mock<IAssetServiceAsync> _assetServiceMock;
        private readonly ReturnRequestService _returnRequestService;

        public ReturnRequestServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _returnRequestRepositoryMock = new Mock<IReturnRequestRepositoriesAsync>();
            _userRepositoryMock = new Mock<IUserRepositoriesAsync>();
            _assignmentRepositoryMock = new Mock<IAssignmentRepositoriesAsync>();
            _addReturnRequestValidatorMock = new Mock<IValidator<AddReturnRequestDto>>();
            _uriServiceMock = new Mock<IUriService>();
            _assetRepositoryMock = new Mock<IAssetRepositoriesAsync>();
            _assetServiceMock = new Mock<IAssetServiceAsync>();

            _returnRequestService = new ReturnRequestService(
                _returnRequestRepositoryMock.Object,
                _mapperMock.Object,
                _addReturnRequestValidatorMock.Object,
                _userRepositoryMock.Object,
                _assignmentRepositoryMock.Object,
                _uriServiceMock.Object,
                _assetRepositoryMock.Object,
                _assetServiceMock.Object,
                _assetRepositoryMock.Object
            );
        }

        [Fact]
        public async Task AddReturnRequestAsync_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new AddReturnRequestDto
            {
                AssignmentId = Guid.NewGuid(),
                RequestedBy = Guid.NewGuid()
            };

            var assignment = new Assignment
            {
                Id = request.AssignmentId,
                State = EnumAssignmentState.Accepted
            };

            var user = new User
            {
                Id = request.RequestedBy
            };

            _addReturnRequestValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _assignmentRepositoryMock.Setup(r => r.GetByIdAsync(request.AssignmentId))
                .ReturnsAsync(assignment);

            _userRepositoryMock.Setup(r => r.GetByIdAsync(request.RequestedBy))
                .ReturnsAsync(user);

            _returnRequestRepositoryMock.Setup(r => r.AddAsync(It.IsAny<ReturnRequest>()))
                .ReturnsAsync(new ReturnRequest());

            // Act
            var result = await _returnRequestService.AddReturnRequestAsync(request);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Create Return Request Successfully.", result.Message);
        }

        [Fact]
        public async Task AddReturnRequestAsync_InvalidRequest_ReturnsValidationErrors()
        {
            // Arrange
            var request = new AddReturnRequestDto
            {
                AssignmentId = Guid.NewGuid(),
                RequestedBy = Guid.NewGuid()
            };

            var validationResult = new FluentValidation.Results.ValidationResult(
                new List<FluentValidation.Results.ValidationFailure>
                {
                    new FluentValidation.Results.ValidationFailure("AssignmentId", "AssignmentId is required."),
                    new FluentValidation.Results.ValidationFailure("RequestedBy", "RequestedBy is required.")
                });

            _addReturnRequestValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _returnRequestService.AddReturnRequestAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains("AssignmentId is required.", result.Errors);
            Assert.Contains("RequestedBy is required.", result.Errors);
        }

        [Fact]
        public async Task CancelReturnRequestAsync_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var returnRequestId = Guid.NewGuid();
            var returnRequest = new ReturnRequest
            {
                Id = returnRequestId,
                ReturnState = EnumReturnRequestState.WaitingForReturning,
                AssignmentId = Guid.NewGuid()
            };

            var assignment = new Assignment
            {
                Id = returnRequest.AssignmentId,
                State = EnumAssignmentState.WaitingForReturning
            };

            _returnRequestRepositoryMock.Setup(r => r.GetByIdAsync(returnRequestId))
                .ReturnsAsync(returnRequest);

            _assignmentRepositoryMock.Setup(r => r.GetByIdAsync(returnRequest.AssignmentId))
                .ReturnsAsync(assignment);

            _returnRequestRepositoryMock.Setup(r => r.DeleteAsync(returnRequestId))
                .ReturnsAsync(returnRequest);

            // Act
            var result = await _returnRequestService.CancelReturnRequestAsync(returnRequestId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Cancel Return Request Successfully.", result.Message);
        }

        [Fact]
        public async Task CancelReturnRequestAsync_ReturnRequestNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var returnRequestId = Guid.NewGuid();

            _returnRequestRepositoryMock.Setup(r => r.GetByIdAsync(returnRequestId))
                .ReturnsAsync((ReturnRequest)null);

            // Act
            var result = await _returnRequestService.CancelReturnRequestAsync(returnRequestId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Return Request not found.", result.Message);
        }

        [Fact]
        public async Task GetAllReturnRequestAsync_ValidRequest_ReturnsPagedResponse()
        {
            // Arrange
            var pagination = new PaginationFilter(1, 10);
            var returnRequests = new List<ReturnRequest>
            {
                new ReturnRequest { Id = Guid.NewGuid(), ReturnState = EnumReturnRequestState.WaitingForReturning },
                new ReturnRequest { Id = Guid.NewGuid(), ReturnState = EnumReturnRequestState.Completed }
            };

            _returnRequestRepositoryMock.Setup(r => r.FilterReturnRequestAsync(It.IsAny<EnumLocation>(), It.IsAny<string>(), It.IsAny<EnumReturnRequestState?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .ReturnsAsync(returnRequests.AsQueryable());

            _mapperMock.Setup(m => m.Map<List<ReturnRequestResponseDto>>(It.IsAny<List<ReturnRequest>>()))
                .Returns(new List<ReturnRequestResponseDto>());

            // Act
            var result = await _returnRequestService.GetAllReturnRequestAsync(pagination, null, null, null, null, EnumLocation.HaNoi, null, null, null);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task ChangeAssignmentStateAsync_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new ChangeStateReturnRequestDto
            {
                ReturnRequestId = Guid.NewGuid(),
                NewState = EnumReturnRequestState.Completed,
                AcceptedBy = Guid.NewGuid()
            };

            var returnRequest = new ReturnRequest
            {
                Id = request.ReturnRequestId,
                ReturnState = EnumReturnRequestState.WaitingForReturning,
                AssignmentId = Guid.NewGuid()
            };

            var assignment = new Assignment
            {
                Id = returnRequest.AssignmentId,
                AssetId = Guid.NewGuid(),
                State = EnumAssignmentState.WaitingForReturning
            };

            var asset = new Asset
            {
                Id = assignment.AssetId,
                State = AssetStateType.Assigned
            };

            _returnRequestRepositoryMock.Setup(r => r.GetByIdAsync(request.ReturnRequestId))
                .ReturnsAsync(returnRequest);

            _assignmentRepositoryMock.Setup(r => r.GetByIdAsync(returnRequest.AssignmentId))
                .ReturnsAsync(assignment);

            _assetRepositoryMock.Setup(r => r.GetByIdAsync(assignment.AssetId))
                .ReturnsAsync(asset);

            // Act
            var result = await _returnRequestService.ChangeAssignmentStateAsync(request);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Return request state changed successfully.", result.Message);
        }
    }
}
