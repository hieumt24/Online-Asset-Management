using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Category;
using AssetManagement.Application.Models.DTOs.Category.Requests;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Entites;
using AutoMapper;
using FluentValidation;

namespace AssetManagement.Application.Services
{
    public class CategoryService : ICategoryServiceAsync
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepositoriesAsync _categoryRepository;
        private readonly IValidator<AddCategoryRequestDto> _addCategoryValidator;
        private readonly IAssetRepositoriesAsync _assetRepository;
        private readonly IValidator<UpdateCategoryRequestDto> _editCategoryValidator;


        public CategoryService(
            ICategoryRepositoriesAsync categoryRepository,
            IMapper mapper,
            IValidator<AddCategoryRequestDto> addCategoryValidator,
            IValidator<UpdateCategoryRequestDto> editCategoryValidator,
            IAssetRepositoriesAsync assetRepository
        )
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _addCategoryValidator = addCategoryValidator;
            _editCategoryValidator = editCategoryValidator;
            _assetRepository = assetRepository;
        }

        public async Task<Response<CategoryDto>> AddCategoryAsync(AddCategoryRequestDto request)
        {
            var validationResult = await _addCategoryValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return new Response<CategoryDto>
                {
                    Succeeded = false,
                    Message = string.Join("; ", errors)
                };
            }

            try
            {
                if (await _categoryRepository.IsCategoryNameDuplicateAsync(request.CategoryName))
                {
                    return new Response<CategoryDto> { Succeeded = false, Message = "Category name already exists." };
                }

                if (await _categoryRepository.IsPrefixDuplicateAsync(request.Prefix))
                {
                    return new Response<CategoryDto> { Succeeded = false, Message = "Prefix already exists." };
                }

               
                var category = _mapper.Map<Category>(request);
                category.CategoryName=  string.Join(" ", request.CategoryName.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                category.Prefix = request.Prefix.Replace(" ","").ToUpper();
                var addedCategory = await _categoryRepository.AddAsync(category);

                return new Response<CategoryDto> { Succeeded = true, Message = "Create Category Successfully." };
            }
            catch (Exception ex)
            {
                return new Response<CategoryDto> { Succeeded = false, Errors = { ex.Message } };
            }
        }

        public async Task<Response<CategoryDto>> GetCategoryByIdAsync(Guid categoryId)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                {
                    return new Response<CategoryDto> { Succeeded = false, Message = "Category not found." };
                }

                var categoryDto = _mapper.Map<CategoryDto>(category);
                return new Response<CategoryDto> { Succeeded = true, Data = categoryDto };
            }
            catch (Exception ex)
            {
                return new Response<CategoryDto> { Succeeded = false, Errors = { ex.Message } };
            }
        }

        public async Task<Response<CategoryDto>> EditCategoryAsync(UpdateCategoryRequestDto request)
        {
            var validationResult = await _editCategoryValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return new Response<CategoryDto>
                {
                    Succeeded = false,
                    Message = string.Join("; ", errors)
                };
            }

            try
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(request.Id);
                if (existingCategory == null)
                {
                    return new Response<CategoryDto> { Succeeded = false, Message = "Category not found." };
                }

                if (await _categoryRepository.IsCategoryNameDuplicateAsync(request.CategoryName))
                {
                    return new Response<CategoryDto> { Succeeded = false, Message = "Category name already exists." };
                }

                if (await _categoryRepository.IsPrefixDuplicateAsync(request.Prefix))
                {
                    return new Response<CategoryDto> { Succeeded = false, Message = "Prefix already exists." };
                }

                existingCategory.CategoryName = request.CategoryName.Trim();
                existingCategory.Prefix = request.Prefix.Replace(" ","").ToUpper();
                await _categoryRepository.UpdateAsync(existingCategory);

                return new Response<CategoryDto> { Succeeded = true, Message = "Edit Category Successfully." };
            }
            catch (Exception ex)
            {
                return new Response<CategoryDto> { Succeeded = false, Errors = { ex.Message } };
            }
        }

        public async Task<Response<CategoryDto>> DeleteCategoryAsync(Guid categoryId)
        {
            try
            {
                var assetInCategory = await _assetRepository.FindExitingCategory(categoryId);
                if (assetInCategory != null)
                {
                    return new Response<CategoryDto> { Succeeded = false, Message = "Cannot delete category because there are assets associated with it." };
                }

                var category = await _categoryRepository.DeletePermanentAsync(categoryId);
                if (category == null)
                {
                    return new Response<CategoryDto> { Succeeded = false, Message = "Category not found." };
                }

                return new Response<CategoryDto> { Succeeded = true, Message = "Delete Category Successfully." };
            }
            catch (Exception ex)
            {
                return new Response<CategoryDto> { Succeeded = false, Errors = { ex.Message } };
            }
        }


        public async Task<Response<List<CategoryDto>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.ListAllActiveAsync();
                var sortedCategories = categories.OrderBy(c => c.CategoryName).ToList();

                var categoryDtos = _mapper.Map<List<CategoryDto>>(sortedCategories);

                return new Response<List<CategoryDto>> { Succeeded = true, Data = categoryDtos };
            }
            catch (Exception ex)
            {
                return new Response<List<CategoryDto>> { Succeeded = false, Errors = { ex.Message } };
            }
        }
    }
}
