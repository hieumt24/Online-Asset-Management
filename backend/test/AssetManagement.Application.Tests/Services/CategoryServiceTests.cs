using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Application.Models.DTOs.Category;
using AssetManagement.Application.Models.DTOs.Category.Requests;
using AssetManagement.Application.Services;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Entites;
using AutoMapper;
using FluentValidation;
using Moq;
using Xunit;

namespace AssetManagement.Application.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepositoriesAsync> _categoryRepositoryMock;
        private readonly Mock<IAssetRepositoriesAsync> _assetRepositoryMock;
        private readonly Mock<IValidator<AddCategoryRequestDto>> _addCategoryValidatorMock;
        private readonly Mock<IValidator<UpdateCategoryRequestDto>> _editCategoryValidatorMock;
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepositoriesAsync>();
            _assetRepositoryMock = new Mock<IAssetRepositoriesAsync>();
            _addCategoryValidatorMock = new Mock<IValidator<AddCategoryRequestDto>>();
            _editCategoryValidatorMock = new Mock<IValidator<UpdateCategoryRequestDto>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddCategoryRequestDto, Category>();
                cfg.CreateMap<Category, CategoryDto>();
                cfg.CreateMap<UpdateCategoryRequestDto, Category>();
            });

            _mapper = config.CreateMapper();

            _categoryService = new CategoryService(
                _categoryRepositoryMock.Object,
                _mapper,
                _addCategoryValidatorMock.Object,
                _editCategoryValidatorMock.Object,
                _assetRepositoryMock.Object
            );
        }

        [Fact]
        public async Task AddCategoryAsync_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new AddCategoryRequestDto
            {
                CategoryName = "Test Category",
                Prefix = "TC"
            };

            var category = _mapper.Map<Category>(request);
            category.Id = Guid.NewGuid();

            _addCategoryValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _categoryRepositoryMock.Setup(r => r.IsCategoryNameDuplicateAsync(request.CategoryName))
                .ReturnsAsync(false);

            _categoryRepositoryMock.Setup(r => r.IsPrefixDuplicateAsync(request.Prefix))
                .ReturnsAsync(false);

            _categoryRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Category>()))
                .ReturnsAsync(category);

            // Act
            var result = await _categoryService.AddCategoryAsync(request);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Create Category Successfully.", result.Message);
        }

        [Fact]
        public async Task AddCategoryAsync_InvalidRequest_ReturnsValidationErrors()
        {
            // Arrange
            var request = new AddCategoryRequestDto
            {
                CategoryName = "Test Category",
                Prefix = "TC"
            };

            var validationErrors = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("CategoryName", "CategoryName is required.")
            };

            _addCategoryValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

            // Act
            var result = await _categoryService.AddCategoryAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains("CategoryName is required.", result.Message);
        }

        [Fact]
        public async Task AddCategoryAsync_DuplicateCategoryName_ReturnsErrorResponse()
        {
            // Arrange
            var request = new AddCategoryRequestDto
            {
                CategoryName = "Test Category",
                Prefix = "TC"
            };

            _addCategoryValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _categoryRepositoryMock.Setup(r => r.IsCategoryNameDuplicateAsync(request.CategoryName))
                .ReturnsAsync(true);

            // Act
            var result = await _categoryService.AddCategoryAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Category name already exists.", result.Message);
        }

        [Fact]
        public async Task AddCategoryAsync_DuplicatePrefix_ReturnsErrorResponse()
        {
            // Arrange
            var request = new AddCategoryRequestDto
            {
                CategoryName = "Test Category",
                Prefix = "TC"
            };

            _addCategoryValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _categoryRepositoryMock.Setup(r => r.IsPrefixDuplicateAsync(request.Prefix))
                .ReturnsAsync(true);

            // Act
            var result = await _categoryService.AddCategoryAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Prefix already exists.", result.Message);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_CategoryExists_ReturnsCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var existingCategory = new Category
            {
                Id = categoryId,
                CategoryName = "Test Category",
                Prefix = "TC"
            };

            _categoryRepositoryMock.Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync(existingCategory);

            // Act
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(existingCategory.CategoryName, result.Data.CategoryName);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_CategoryNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            _categoryRepositoryMock.Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync((Category)null);

            // Act
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Category not found.", result.Message);
        }

        [Fact]
        public async Task EditCategoryAsync_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new UpdateCategoryRequestDto
            {
                Id = Guid.NewGuid(),
                CategoryName = "Updated Category",
                Prefix = "UC"
            };

            var existingCategory = new Category
            {
                Id = request.Id,
                CategoryName = "Original Category",
                Prefix = "OC"
            };

            _editCategoryValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _categoryRepositoryMock.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(existingCategory);

            _categoryRepositoryMock.Setup(r => r.IsCategoryNameDuplicateAsync(request.CategoryName))
                .ReturnsAsync(false);

            _categoryRepositoryMock.Setup(r => r.IsPrefixDuplicateAsync(request.Prefix))
                .ReturnsAsync(false);

            //_categoryRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Category>()))
            //    .Returns(Task.CompletedTask);

            // Act
            var result = await _categoryService.EditCategoryAsync(request);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Edit Category Successfully.", result.Message);
        }

        [Fact]
        public async Task EditCategoryAsync_CategoryNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var request = new UpdateCategoryRequestDto
            {
                Id = Guid.NewGuid(),
                CategoryName = "Updated Category",
                Prefix = "UC"
            };

            _editCategoryValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _categoryRepositoryMock.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync((Category)null);

            // Act
            var result = await _categoryService.EditCategoryAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Category not found.", result.Message);
        }

        [Fact]
        public async Task EditCategoryAsync_DuplicateCategoryName_ReturnsErrorResponse()
        {
            // Arrange
            var request = new UpdateCategoryRequestDto
            {
                Id = Guid.NewGuid(),
                CategoryName = "Updated Category",
                Prefix = "UC"
            };

            var existingCategory = new Category
            {
                Id = request.Id,
                CategoryName = "Original Category",
                Prefix = "OC"
            };

            _editCategoryValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _categoryRepositoryMock.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(existingCategory);

            _categoryRepositoryMock.Setup(r => r.IsCategoryNameDuplicateAsync(request.CategoryName))
                .ReturnsAsync(true);

            // Act
            var result = await _categoryService.EditCategoryAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Category name already exists.", result.Message);
        }

        [Fact]
        public async Task EditCategoryAsync_DuplicatePrefix_ReturnsErrorResponse()
        {
            // Arrange
            var request = new UpdateCategoryRequestDto
            {
                Id = Guid.NewGuid(),
                CategoryName = "Updated Category",
                Prefix = "UC"
            };

            var existingCategory = new Category
            {
                Id = request.Id,
                CategoryName = "Original Category",
                Prefix = "OC"
            };

            _editCategoryValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _categoryRepositoryMock.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(existingCategory);

            _categoryRepositoryMock.Setup(r => r.IsPrefixDuplicateAsync(request.Prefix))
                .ReturnsAsync(true);

            // Act
            var result = await _categoryService.EditCategoryAsync(request);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Prefix already exists.", result.Message);
        }

        [Fact]
        public async Task DeleteCategoryAsync_CategoryExists_ReturnsSuccessResponse()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var existingCategory = new Category
            {
                Id = categoryId,
                CategoryName = "Test Category",
                Prefix = "TC"
            };

            _assetRepositoryMock.Setup(r => r.FindExitingCategory(categoryId))
                .ReturnsAsync((Asset)null);

            _categoryRepositoryMock.Setup(r => r.DeletePermanentAsync(categoryId))
                .ReturnsAsync(existingCategory);

            // Act
            var result = await _categoryService.DeleteCategoryAsync(categoryId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Delete Category Successfully.", result.Message);
        }

        [Fact]
        public async Task DeleteCategoryAsync_CategoryNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            _assetRepositoryMock.Setup(r => r.FindExitingCategory(categoryId))
                .ReturnsAsync((Asset)null);

            _categoryRepositoryMock.Setup(r => r.DeletePermanentAsync(categoryId))
                .ReturnsAsync((Category)null);

            // Act
            var result = await _categoryService.DeleteCategoryAsync(categoryId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Category not found.", result.Message);
        }

        [Fact]
        public async Task DeleteCategoryAsync_CategoryHasAssets_ReturnsErrorResponse()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var existingAsset = new Asset
            {
                Id = Guid.NewGuid(),
                CategoryId = categoryId,
                AssetName = "Test Asset"
            };

            _assetRepositoryMock.Setup(r => r.FindExitingCategory(categoryId))
                .ReturnsAsync(existingAsset);

            // Act
            var result = await _categoryService.DeleteCategoryAsync(categoryId);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Cannot delete category because there are assets associated with it.", result.Message);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ReturnsSortedCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), CategoryName = "Category B", Prefix = "B" },
                new Category { Id = Guid.NewGuid(), CategoryName = "Category A", Prefix = "A" },
                new Category { Id = Guid.NewGuid(), CategoryName = "Category C", Prefix = "C" }
            };

            _categoryRepositoryMock.Setup(r => r.ListAllActiveAsync())
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllCategoriesAsync();

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(3, result.Data.Count);
            Assert.Equal("Category A", result.Data[0].CategoryName);
            Assert.Equal("Category B", result.Data[1].CategoryName);
            Assert.Equal("Category C", result.Data[2].CategoryName);
        }
    }
}
