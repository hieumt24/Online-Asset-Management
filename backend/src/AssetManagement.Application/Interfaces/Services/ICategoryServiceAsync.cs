using AssetManagement.Application.Models.DTOs.Category.Requests;
using AssetManagement.Application.Models.DTOs.Category;
using AssetManagement.Application.Wrappers;

namespace AssetManagement.Application.Interfaces.Services
{
    public interface ICategoryServiceAsync
    {
        Task<Response<CategoryDto>> AddCategoryAsync(AddCategoryRequestDto request);
        Task<Response<CategoryDto>> GetCategoryByIdAsync(Guid categoryId);
        Task<Response<CategoryDto>> EditCategoryAsync(UpdateCategoryRequestDto request);
        Task<Response<CategoryDto>> DeleteCategoryAsync(Guid categoryId);
        Task<Response<List<CategoryDto>>> GetAllCategoriesAsync();
    }
}
