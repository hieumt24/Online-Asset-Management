using AssetManagement.Application.Mappings;
using AssetManagement.Application.Models.DTOs.Assets;
using AssetManagement.Application.Models.DTOs.Assets.Requests;
using AssetManagement.Application.Models.DTOs.Assets.Responses;
using AssetManagement.Application.Models.DTOs.Assignments;
using AssetManagement.Application.Models.DTOs.Assignments.Request;
using AssetManagement.Application.Models.DTOs.Assignments.Response;
using AssetManagement.Application.Models.DTOs.Category;
using AssetManagement.Application.Models.DTOs.Category.Requests;
using AssetManagement.Application.Models.DTOs.ReturnRequests;
using AssetManagement.Application.Models.DTOs.ReturnRequests.Request;
using AssetManagement.Application.Models.DTOs.Users;
using AssetManagement.Application.Models.DTOs.Users.Requests;
using AssetManagement.Application.Models.DTOs.Users.Responses;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;
using AutoMapper;
using Xunit;

namespace AssetManagement.Application.Tests.Mappings
{
    public class GeneralProfileTests
    {
        private readonly IMapper _mapper;

        public GeneralProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GeneralProfile>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void GeneralProfile_ConfigurationIsValid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GeneralProfile>();
            });

            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void ShouldMap_UserDtoToUser()
        {
            var userDto = new UserDto { Id = Guid.NewGuid(), Username = "testuser" };
            var user = _mapper.Map<User>(userDto);

            Assert.NotNull(user);
            Assert.Equal(userDto.Id, user.Id);
            Assert.Equal(userDto.Username, user.Username);
        }

        [Fact]
        public void ShouldMap_UserToUserResponseDto()
        {
            var user = new User { Id = Guid.NewGuid(), Username = "testuser" };
            var userResponseDto = _mapper.Map<UserResponseDto>(user);

            Assert.NotNull(userResponseDto);
            Assert.Equal(user.Id, userResponseDto.Id);
            Assert.Equal(user.Username, userResponseDto.Username);
        }

        [Fact]
        public void ShouldMap_AddAssetRequestDtoToAsset()
        {
            var addAssetRequestDto = new AddAssetRequestDto { AssetName = "Laptop" };
            var asset = _mapper.Map<Asset>(addAssetRequestDto);

            Assert.NotNull(asset);
            Assert.Equal(addAssetRequestDto.AssetName, asset.AssetName);
        }

        [Fact]
        public void ShouldMap_AssetToAssetDto()
        {
            var asset = new Asset { Id = Guid.NewGuid(), AssetName = "Laptop" };
            var assetDto = _mapper.Map<AssetDto>(asset);

            Assert.NotNull(assetDto);
            Assert.Equal(asset.Id, assetDto.Id);
            Assert.Equal(asset.AssetName, assetDto.AssetName);
        }

        [Fact]
        public void ShouldMap_CategoryToCategoryDto()
        {
            var category = new Category { Id = Guid.NewGuid(), CategoryName = "Electronics" };
            var categoryDto = _mapper.Map<CategoryDto>(category);

            Assert.NotNull(categoryDto);
            Assert.Equal(category.Id, categoryDto.Id);
            Assert.Equal(category.CategoryName, categoryDto.CategoryName);
        }

        [Fact]
        public void ShouldMap_AddCategoryRequestDtoToCategory()
        {
            var addCategoryRequestDto = new AddCategoryRequestDto { CategoryName = "Electronics" };
            var category = _mapper.Map<Category>(addCategoryRequestDto);

            Assert.NotNull(category);
            Assert.Equal(addCategoryRequestDto.CategoryName, category.CategoryName);
        }

        [Fact]
        public void ShouldMap_AssignmentToAssignmentDto()
        {
            var assignment = new Assignment { Id = Guid.NewGuid(), Note = "Test Note" };
            var assignmentDto = _mapper.Map<AssignmentDto>(assignment);

            Assert.NotNull(assignmentDto);
            Assert.Equal(assignment.Id, assignmentDto.Id);
            Assert.Equal(assignment.Note, assignmentDto.Note);
        }

        [Fact]
        public void ShouldMap_AddAssignmentRequestDtoToAssignment()
        {
            var addAssignmentRequestDto = new AddAssignmentRequestDto { Note = "Test Note" };
            var assignment = _mapper.Map<Assignment>(addAssignmentRequestDto);

            Assert.NotNull(assignment);
            Assert.Equal(addAssignmentRequestDto.Note, assignment.Note);
        }

        [Fact]
        public void ShouldMap_ReturnRequestToReturnRequestDto()
        {
            var returnRequest = new ReturnRequest { ReturnState = EnumReturnRequestState.Completed };
            var returnRequestDto = _mapper.Map<ReturnRequestDto>(returnRequest);

            Assert.NotNull(returnRequestDto);
            Assert.Equal(returnRequest.ReturnState, returnRequestDto.ReturnState);
        }

        //[Fact]
        //public void ShouldMap_AddReturnRequestDtoToReturnRequest()
        //{
        //    var addReturnRequestDto = new AddReturnRequestDto { ReturnState = EnumReturnRequestState.Completed };
        //    var returnRequest = _mapper.Map<ReturnRequest>(addReturnRequestDto);

        //    Assert.NotNull(returnRequest);
        //    Assert.Equal(addReturnRequestDto.Re, returnRequest.ReturnState);
        //}
    }
}
