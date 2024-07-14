using AssetManagement.Application.Common;
using AssetManagement.Domain.Entites;

namespace AssetManagement.Application.Interfaces.Repositories
{
    public interface ICategoryRepositoriesAsync : IBaseRepositoryAsync<Category>
    {
        Task<bool> IsCategoryNameDuplicateAsync(string categoryName);

        Task<bool> IsPrefixDuplicateAsync(string prefix);

        Task<List<Category>> ListAllActiveAsync();

    }
}
