using AssetManagement.API.Controllers;
using AssetManagement.Application.Filter;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Category;
using AssetManagement.Application.Models.DTOs.Category.Requests;
using AssetManagement.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit.Sdk;

namespace AssetManagement.API.Tests.Controller
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryServiceAsync> _categoryServiceMock;
        private readonly CategoryController _categoryController;

        public CategoryControllerTests()
        {
            _categoryServiceMock = new Mock<ICategoryServiceAsync>();
            _categoryController = new CategoryController(_categoryServiceMock.Object);
            _categoryController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public async Task AddCategory_ReturnsOkResult_WhenCategoryIsCreatedSuccessfully()
        {
            // Arrange
            var addCategoryRequest = new AddCategoryRequestDto();
            var expectedResponse = new PagedResponse<CategoryDto>
            {
                Succeeded = true
            };
            _categoryServiceMock.Setup(x => x.AddCategoryAsync(It.IsAny<AddCategoryRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _categoryController.AddCategory(addCategoryRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<CategoryDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task AddCategory_ReturnsBadRequestResult_WhenServiceResponseFails()
        {
            // Arrange
            var addCategoryRequest = new AddCategoryRequestDto();
            var expectedResponse = new PagedResponse<CategoryDto>
            {
                Succeeded = false
            };
            _categoryServiceMock.Setup(x => x.AddCategoryAsync(It.IsAny<AddCategoryRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _categoryController.AddCategory(addCategoryRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<CategoryDto>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsOkResult_WhenCategoriesAreFetchedSuccessfully()
        {
            // Arrange
            var expectedResponse = new PagedResponse<List<CategoryDto>>
            {
                Succeeded = true,
                Data = new List<CategoryDto>()
            };
            _categoryServiceMock.Setup(x => x.GetAllCategoriesAsync())
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _categoryController.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<List<CategoryDto>>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }


        [Fact]
        public async Task GetCategories_ReturnsBadRequestResult_WhenServiceResponseFails()
        {
            // Arrange
            var expectedResponse = new Response<List<CategoryDto>>
            {
                Succeeded = false,
                Errors = new List<string> { "Some error" }
            };
            _categoryServiceMock.Setup(x => x.GetAllCategoriesAsync())
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _categoryController.GetAllCategories();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<Response<List<CategoryDto>>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }


        [Fact]
        public async Task GetCategoryById_ReturnsOkResult_WhenCategoryIsFound()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var expectedResponse = new PagedResponse<CategoryDto>
            {
                Succeeded = true
            };
            _categoryServiceMock.Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _categoryController.GetCategoryById(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<CategoryDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsBadRequestResult_WhenServiceResponseFails()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var expectedResponse = new PagedResponse<CategoryDto>
            {
                Succeeded = false
            };
            _categoryServiceMock.Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _categoryController.GetCategoryById(categoryId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<CategoryDto>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }

        [Fact]
        public async Task EditCategory_ReturnsOkResult_WhenCategoryIsEditedSuccessfully()
        {
            // Arrange
            var editCategoryRequest = new UpdateCategoryRequestDto
            {
                Id = Guid.NewGuid(),
                CategoryName = "New Category",
                Prefix = "NC"
            };
            var expectedResponse = new Response<CategoryDto>
            {
                Succeeded = true,
                Message = "Edit Category Successfully."
            };
            _categoryServiceMock.Setup(x => x.EditCategoryAsync(It.IsAny<UpdateCategoryRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _categoryController.EditCategory(editCategoryRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<CategoryDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }


        [Fact]
        public async Task EditCategory_ReturnsBadRequestResult_WhenServiceResponseFails()
        {
            // Arrange
            var editCategoryRequest = new UpdateCategoryRequestDto
            {
                Id = Guid.NewGuid(),
                CategoryName = "New Category",
                Prefix = "NC"
            };
            var expectedResponse = new PagedResponse<CategoryDto>
            {
                Succeeded = false,
                Message = "update category fail."
            };
            _categoryServiceMock.Setup(x => x.EditCategoryAsync(It.IsAny<UpdateCategoryRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _categoryController.EditCategory(editCategoryRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<CategoryDto>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsOkResult_WhenCategoryIsDeletedSuccessfully()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var expectedResponse = new PagedResponse<CategoryDto>
            {
                Succeeded = true
            };
            _categoryServiceMock.Setup(x => x.DeleteCategoryAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _categoryController.DeleteCategory(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<CategoryDto>>(okResult.Value);
            Assert.True(returnValue.Succeeded);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsBadRequestResult_WhenServiceResponseFails()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var expectedResponse = new PagedResponse<CategoryDto>
            {
                Succeeded = false
            };
            _categoryServiceMock.Setup(x => x.DeleteCategoryAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _categoryController.DeleteCategory(categoryId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<PagedResponse<CategoryDto>>(badRequestResult.Value);
            Assert.False(returnValue.Succeeded);
        }
    }
}
