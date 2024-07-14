using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetManagement.Application.Filter;
using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Assignments;
using AssetManagement.Application.Models.DTOs.Assignments.Reques;
using AssetManagement.Application.Models.DTOs.Assignments.Request;
using AssetManagement.Application.Models.DTOs.Assignments.Requests;
using AssetManagement.Application.Models.DTOs.Assignments.Response;
using AssetManagement.Application.Services;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using Xunit;

namespace AssetManagement.Application.Tests.Services
{
    public class AssignmentServiceTests
    {
        private readonly Mock<IAssignmentRepositoriesAsync> _assignmentRepositoryMock;
        private readonly Mock<IUserRepositoriesAsync> _userRepositoryMock;
        private readonly Mock<IAssetRepositoriesAsync> _assetRepositoryMock;
        private readonly Mock<IUriService> _uriServiceMock;
        private readonly Mock<IValidator<AddAssignmentRequestDto>> _addAssignmentValidatorMock;
        private readonly Mock<IValidator<EditAssignmentRequestDto>> _editAssignmentValidatorMock;
        private readonly IMapper _mapper;
        private readonly AssignmentServiceAsync _assignmentService;

        public AssignmentServiceTests()
        {
            _assignmentRepositoryMock = new Mock<IAssignmentRepositoriesAsync>();
            _userRepositoryMock = new Mock<IUserRepositoriesAsync>();
            _assetRepositoryMock = new Mock<IAssetRepositoriesAsync>();
            _uriServiceMock = new Mock<IUriService>();
            _addAssignmentValidatorMock = new Mock<IValidator<AddAssignmentRequestDto>>();
            _editAssignmentValidatorMock = new Mock<IValidator<EditAssignmentRequestDto>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddAssignmentRequestDto, Assignment>();
                cfg.CreateMap<Assignment, AssignmentDto>();
                cfg.CreateMap<Assignment, AssignmentResponseDto>();
                cfg.CreateMap<EditAssignmentRequestDto, Assignment>();
            });

            _mapper = config.CreateMapper();

            _assignmentService = new AssignmentServiceAsync(
                _assignmentRepositoryMock.Object,
                _mapper,
                _addAssignmentValidatorMock.Object,
                _editAssignmentValidatorMock.Object,
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object,
                _uriServiceMock.Object
            );
        }

        [Fact]
        public async Task AddAssignmentAsync_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new AddAssignmentRequestDto
            {
                AssetId = Guid.NewGuid(),
                AssignedIdBy = Guid.NewGuid(),
                AssignedIdTo = Guid.NewGuid(),
                AssignedDate = DateTime.Now,
                Note = "Test Note"
            };

            var assignment = _mapper.Map<Assignment>(request);
            assignment.Id = Guid.NewGuid();

            _addAssignmentValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _assetRepositoryMock.Setup(r => r.GetByIdAsync(request.AssetId))
                .ReturnsAsync(new Asset { Id = request.AssetId });

            _userRepositoryMock.Setup(r => r.GetByIdAsync(request.AssignedIdBy))
                .ReturnsAsync(new User { Id = request.AssignedIdBy, JoinedDate = DateTime.Now.AddDays(-1) });

            _assignmentRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Assignment>()))
                .ReturnsAsync(assignment);

            // Act
            var result = await _assignmentService.AddAssignmentAsync(request);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(" Create Assignment Successfully!", result.Message);
        }

        [Fact]
        public async Task AddAssignmentAsync_InvalidRequest_ReturnsValidationErrors()
        {
            // Arrange
            var request = new AddAssignmentRequestDto
            {
                AssetId = Guid.NewGuid(),
                AssignedIdBy = Guid.NewGuid(),
                AssignedIdTo = Guid.NewGuid(),
                AssignedDate = DateTime.Now,
                Note = "Test Note"
            };

            var validationErrors = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("AssetId", "AssetId is required.")
            };

            _addAssignmentValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

            // Act
            var result = await _assignmentService.AddAssignmentAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains("AssetId is required.", result.Errors);
        }

        [Fact]
        public async Task EditAssignmentAsync_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var request = new EditAssignmentRequestDto
            {
                AssetId = Guid.NewGuid(),
                AssignedIdBy = Guid.NewGuid(),
                AssignedIdTo = Guid.NewGuid(),
                AssignedDate = DateTime.Now,
                Note = "Updated Note"
            };

            var existingAssignment = new Assignment
            {
                Id = assignmentId,
                AssetId = Guid.NewGuid(),
                AssignedIdBy = Guid.NewGuid(),
                AssignedIdTo = Guid.NewGuid(),
                AssignedDate = DateTime.Now,
                Note = "Original Note",
                State = EnumAssignmentState.WaitingForAcceptance
            };

            _editAssignmentValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _assignmentRepositoryMock.Setup(r => r.GetByIdAsync(assignmentId))
                .ReturnsAsync(existingAssignment);

            _assetRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Asset { Id = request.AssetId });

            // Act
            var result = await _assignmentService.EditAssignmentAsync(request, assignmentId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Update assignment successfully.", result.Message);
        }

        [Fact]
        public async Task EditAssignmentAsync_AssignmentNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var request = new EditAssignmentRequestDto
            {
                AssetId = Guid.NewGuid(),
                AssignedIdBy = Guid.NewGuid(),
                AssignedIdTo = Guid.NewGuid(),
                AssignedDate = DateTime.Now,
                Note = "Updated Note"
            };

            _editAssignmentValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _assignmentRepositoryMock.Setup(r => r.GetByIdAsync(assignmentId))
                .ReturnsAsync((Assignment)null);

            // Act
            var result = await _assignmentService.EditAssignmentAsync(request, assignmentId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Assignment not found.", result.Message);
        }

        [Fact]
        public async Task GetAssignmentByIdAsync_AssignmentExists_ReturnsAssignment()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var existingAssignment = new Assignment
            {
                Id = assignmentId,
                AssetId = Guid.NewGuid(),
                AssignedIdBy = Guid.NewGuid(),
                AssignedIdTo = Guid.NewGuid(),
                AssignedDate = DateTime.Now,
                Note = "Test Note",
                State = EnumAssignmentState.WaitingForAcceptance
            };

            _assignmentRepositoryMock.Setup(r => r.GetAssignemntByIdAsync(assignmentId))
                .ReturnsAsync(existingAssignment);

            // Act
            var result = await _assignmentService.GetAssignmentByIdAsync(assignmentId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(existingAssignment.Note, result.Data.Note);
        }

        [Fact]
        public async Task GetAssignmentByIdAsync_AssignmentNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();

            _assignmentRepositoryMock.Setup(r => r.GetAssignemntByIdAsync(assignmentId))
                .ReturnsAsync((Assignment)null);

            // Act
            var result = await _assignmentService.GetAssignmentByIdAsync(assignmentId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Assignment not found", result.Message);
        }

        [Fact]
        public async Task DeleteAssignmentAsync_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var existingAssignment = new Assignment
            {
                Id = assignmentId,
                AssetId = Guid.NewGuid(),
                State = EnumAssignmentState.WaitingForAcceptance
            };

            var existingAsset = new Asset
            {
                Id = existingAssignment.AssetId,
                State = AssetStateType.Assigned
            };

            _assignmentRepositoryMock.Setup(r => r.GetAssignemntByIdAsync(assignmentId))
                .ReturnsAsync(existingAssignment);

            _assetRepositoryMock.Setup(r => r.GetByIdAsync(existingAssignment.AssetId))
                .ReturnsAsync(existingAsset);

            //_assignmentRepositoryMock.Setup(r => r.DeleteAsync(assignmentId))
            //    .Returns(Task.CompletedTask);

            // Act
            var result = await _assignmentService.DeleteAssignmentAsync(assignmentId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Assignment deleted successfully.", result.Message);
        }

        [Fact]
        public async Task DeleteAssignmentAsync_AssignmentNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();

            _assignmentRepositoryMock.Setup(r => r.GetAssignemntByIdAsync(assignmentId))
                .ReturnsAsync((Assignment)null);

            // Act
            var result = await _assignmentService.DeleteAssignmentAsync(assignmentId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Assignment cannot be found.", result.Message);
        }

        [Fact]
        public async Task ChangeAssignmentStateAsync_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var request = new ChangeStateAssignmentDto
            {
                AssignmentId = assignmentId,
                AssignedIdTo = Guid.NewGuid(),
                NewState = EnumAssignmentState.Accepted
            };

            var assignment = new Assignment
            {
                Id = assignmentId,
                AssignedIdTo = request.AssignedIdTo,
                State = EnumAssignmentState.WaitingForAcceptance,
                AssetId = Guid.NewGuid()
            };

            _assignmentRepositoryMock.Setup(r => r.GetByIdAsync(assignmentId))
                .ReturnsAsync(assignment);

            // Act
            var result = await _assignmentService.ChangeAssignmentStateAsync(request);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Assignment state changed successfully.", result.Message);
            Assert.Equal(request.NewState, assignment.State);
        }

        [Fact]
        public async Task ChangeAssignmentStateAsync_AssignmentNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var request = new ChangeStateAssignmentDto
            {
                AssignmentId = assignmentId,
                AssignedIdTo = Guid.NewGuid(),
                NewState = EnumAssignmentState.Accepted
            };

            _assignmentRepositoryMock.Setup(r => r.GetByIdAsync(assignmentId))
                .ReturnsAsync((Assignment)null);

            // Act
            var result = await _assignmentService.ChangeAssignmentStateAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("The assignment no longer exists.", result.Message);
        }

        [Fact]
        public async Task ChangeAssignmentStateAsync_AssignedToDifferentStaff_ReturnsErrorResponse()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var request = new ChangeStateAssignmentDto
            {
                AssignmentId = assignmentId,
                AssignedIdTo = Guid.NewGuid(),
                NewState = EnumAssignmentState.Accepted
            };

            var assignment = new Assignment
            {
                Id = assignmentId,
                AssignedIdTo = Guid.NewGuid(), // Different from request.AssignedIdTo
                State = EnumAssignmentState.WaitingForAcceptance,
                AssetId = Guid.NewGuid()
            };

            _assignmentRepositoryMock.Setup(r => r.GetByIdAsync(assignmentId))
                .ReturnsAsync(assignment);

            // Act
            var result = await _assignmentService.ChangeAssignmentStateAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Assignment has assigned for other staff.", result.Message);
        }

        [Fact]
        public async Task ChangeAssignmentStateAsync_InvalidState_ReturnsErrorResponse()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var request = new ChangeStateAssignmentDto
            {
                AssignmentId = assignmentId,
                AssignedIdTo = Guid.NewGuid(),
                NewState = EnumAssignmentState.Accepted
            };

            var assignment = new Assignment
            {
                Id = assignmentId,
                AssignedIdTo = request.AssignedIdTo,
                State = EnumAssignmentState.Accepted, // Already in Accepted state
                AssetId = Guid.NewGuid()
            };

            _assignmentRepositoryMock.Setup(r => r.GetByIdAsync(assignmentId))
                .ReturnsAsync(assignment);

            // Act
            var result = await _assignmentService.ChangeAssignmentStateAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Assignment state cannot be changed.", result.Message);
        }

        [Fact]
        public async Task ChangeAssignmentStateAsync_DeclineAssetNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var request = new ChangeStateAssignmentDto
            {
                AssignmentId = assignmentId,
                AssignedIdTo = Guid.NewGuid(),
                NewState = EnumAssignmentState.Declined
            };

            var assignment = new Assignment
            {
                Id = assignmentId,
                AssignedIdTo = request.AssignedIdTo,
                State = EnumAssignmentState.WaitingForAcceptance,
                AssetId = Guid.NewGuid()
            };

            _assignmentRepositoryMock.Setup(r => r.GetByIdAsync(assignmentId))
                .ReturnsAsync(assignment);

            _assetRepositoryMock.Setup(r => r.GetByIdAsync(assignment.AssetId))
                .ReturnsAsync((Asset)null);

            // Act
            var result = await _assignmentService.ChangeAssignmentStateAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Asset not found.", result.Message);
        }

        [Fact]
        public async Task ChangeAssignmentStateAsync_DeclineAssetUpdateError_ReturnsErrorResponse()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var request = new ChangeStateAssignmentDto
            {
                AssignmentId = assignmentId,
                AssignedIdTo = Guid.NewGuid(),
                NewState = EnumAssignmentState.Declined
            };

            var assignment = new Assignment
            {
                Id = assignmentId,
                AssignedIdTo = request.AssignedIdTo,
                State = EnumAssignmentState.WaitingForAcceptance,
                AssetId = Guid.NewGuid()
            };

            var asset = new Asset
            {
                Id = assignment.AssetId,
                State = AssetStateType.Assigned
            };

            _assignmentRepositoryMock.Setup(r => r.GetByIdAsync(assignmentId))
                .ReturnsAsync(assignment);

            _assetRepositoryMock.Setup(r => r.GetByIdAsync(assignment.AssetId))
                .ReturnsAsync(asset);

            _assetRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Asset>()))
                .ThrowsAsync(new Exception("Update error"));

            // Act
            var result = await _assignmentService.ChangeAssignmentStateAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("An error occurred while updating the asset state.", result.Message);
            Assert.Contains("Update error", result.Errors);
        }

        [Fact]
        public async Task GetAssignmentsOfUser_ValidRequest_ReturnsPagedResponse()
        {
            // Arrange
            var paginationFilter = new PaginationFilter();
            var userId = Guid.NewGuid();
            var search = "test";
            var assignmentState = EnumAssignmentState.Accepted;
            var dateFrom = DateTime.UtcNow.AddDays(-7);
            var dateTo = DateTime.UtcNow;
            var orderBy = "AssignmentDate";
            var isDescending = true;
            var route = "api/assignments";

            var assignmentsList = new List<Assignment>
            {
                new Assignment { Id = Guid.NewGuid(), AssignedIdTo = userId },
                new Assignment { Id = Guid.NewGuid(), AssignedIdTo = userId }
            };

            var pagedResponse = new PagedResponse<List<AssignmentResponseDto>>
            {
                Succeeded = true,
                Data = _mapper.Map<List<AssignmentResponseDto>>(assignmentsList),
                Message = "Assignments retrieved successfully."
            };

            _assignmentRepositoryMock.Setup(r => r.FilterAssignmentOfUserAsync(userId, search, assignmentState, dateFrom, dateTo))
                .ReturnsAsync(assignmentsList.AsQueryable());

            _uriServiceMock.Setup(u => u.GetPageUri(paginationFilter, route))
                .Returns(new Uri("http://localhost/api/assignments?pageNumber=1&pageSize=10"));

            // Act
            var result = await _assignmentService.GetAssignmentsOfUser(paginationFilter, userId, search, assignmentState, dateFrom, dateTo, orderBy, isDescending, route);

            // Assert
            result.Should().BeEquivalentTo(pagedResponse);
        }

        [Fact]
        public async Task GetAssignmentsOfUser_DateFromGreaterThanDateTo_ReturnsFailedResponse()
        {
            // Arrange
            var paginationFilter = new PaginationFilter();
            var userId = Guid.NewGuid();
            var search = "test";
            var assignmentState = EnumAssignmentState.Accepted;
            var dateFrom = DateTime.UtcNow;
            var dateTo = DateTime.UtcNow.AddDays(-7);
            var orderBy = "AssignmentDate";
            var isDescending = true;
            var route = "api/assignments";

            // Act
            var result = await _assignmentService.GetAssignmentsOfUser(paginationFilter, userId, search, assignmentState, dateFrom, dateTo, orderBy, isDescending, route);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Date from must be before date to");
        }

        [Fact]
        public async Task GetAssetAssign_ValidRequest_ReturnsPagedResponse()
        {
            // Arrange
            var paginationFilter = new PaginationFilter();
            var assetId = Guid.NewGuid();
            var search = "test";
            var assignmentState = EnumAssignmentState.Accepted;
            var dateFrom = DateTime.UtcNow.AddDays(-7);
            var dateTo = DateTime.UtcNow;
            var orderBy = "AssignmentDate";
            var isDescending = true;
            var route = "api/assignments";

            var assignmentsList = new List<Assignment>
            {
                new Assignment { Id = Guid.NewGuid(), AssetId = assetId },
                new Assignment { Id = Guid.NewGuid(), AssetId = assetId }
            };

            var pagedResponse = new PagedResponse<List<AssignmentResponseDto>>
            {
                Succeeded = true,
                Data = _mapper.Map<List<AssignmentResponseDto>>(assignmentsList),
                Message = "Assignments retrieved successfully."
            };

            _assignmentRepositoryMock.Setup(r => r.FilterAssignmentByAssetIdAsync(assetId, search, assignmentState, dateFrom, dateTo))
                .ReturnsAsync(assignmentsList.AsQueryable());

            _uriServiceMock.Setup(u => u.GetPageUri(paginationFilter, route))
                .Returns(new Uri("http://localhost/api/assignments?pageNumber=1&pageSize=10"));

            // Act
            var result = await _assignmentService.GetAssetAssign(paginationFilter, assetId, search, assignmentState, dateFrom, dateTo, orderBy, isDescending, route);

            // Assert
            result.Should().BeEquivalentTo(pagedResponse);
        }

        [Fact]
        public async Task GetAssetAssign_DateFromGreaterThanDateTo_ReturnsFailedResponse()
        {
            // Arrange
            var paginationFilter = new PaginationFilter();
            var assetId = Guid.NewGuid();
            var search = "test";
            var assignmentState = EnumAssignmentState.Accepted;
            var dateFrom = DateTime.UtcNow;
            var dateTo = DateTime.UtcNow.AddDays(-7);
            var orderBy = "AssignmentDate";
            var isDescending = true;
            var route = "api/assignments";

            // Act
            var result = await _assignmentService.GetAssetAssign(paginationFilter, assetId, search, assignmentState, dateFrom, dateTo, orderBy, isDescending, route);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Date from must be before date to");
        }

        //[Fact]
        //public async Task GetAllAssignmentsAsync_ValidRequest_ReturnsPagedResponse()
        //{
        //    // Arrange
        //    var paginationFilter = new PaginationFilter();
        //    var search = "test";
        //    var assignmentState = EnumAssignmentState.Accepted;
        //    var dateFrom = DateTime.UtcNow.AddDays(-7);
        //    var dateTo = DateTime.UtcNow;
        //    var location = EnumLocation.HaNoi;
        //    var orderBy = "AssignmentDate";
        //    var isDescending = true;
        //    var route = "api/assignments";

        //    var assignmentsList = new PagedResponse<List<Assignment>>
        //    {
        //        Succeeded = true,
        //        Data = new List<Assignment>
        //        {
        //            new Assignment { Id = Guid.NewGuid(), Location = location },
        //            new Assignment { Id = Guid.NewGuid(), Location = location }
        //        },
        //        Message = "Assignments retrieved successfully.",
        //        TotalRecords = 2
        //    };

        //    _assignmentRepositoryMock.Setup(r => r.GetAllMatchingAssignmentAsync(location, search, assignmentState, dateFrom, dateTo, orderBy, isDescending, paginationFilter))
        //        .ReturnsAsync(assignmentsList);

        //    _uriServiceMock.Setup(u => u.GetPageUri(paginationFilter, route))
        //        .Returns(new Uri("http://localhost/api/assignments?pageNumber=1&pageSize=10"));

        //    // Act
        //    var result = await _assignmentService.GetAllAssignmentsAsync(paginationFilter, search, assignmentState, dateFrom, dateTo, location, orderBy, isDescending, route);

        //    // Assert
        //    result.Should().BeEquivalentTo(assignmentsList);
        //}

        [Fact]
        public async Task GetAllAssignmentsAsync_DateFromGreaterThanDateTo_ReturnsFailedResponse()
        {
            // Arrange
            var paginationFilter = new PaginationFilter();
            var search = "test";
            var assignmentState = EnumAssignmentState.Accepted;
            var dateFrom = DateTime.UtcNow;
            var dateTo = DateTime.UtcNow.AddDays(-7);
            var location = EnumLocation.HaNoi;
            var orderBy = "AssignmentDate";
            var isDescending = true;
            var route = "api/assignments";

            // Act
            var result = await _assignmentService.GetAllAssignmentsAsync(paginationFilter, search, assignmentState, dateFrom, dateTo, location, orderBy, isDescending, route);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Date from must be before date to");
        }


    }
}

    