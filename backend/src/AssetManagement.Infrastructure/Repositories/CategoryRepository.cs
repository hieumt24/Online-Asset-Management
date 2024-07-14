using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Domain.Entites;
using AssetManagement.Infrastructure.Common;
using AssetManagement.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepositoryAsync<Category>, ICategoryRepositoriesAsync
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsCategoryNameDuplicateAsync(string categoryName)
        {
            return await _dbContext.Set<Category>().AnyAsync(c => c.CategoryName == categoryName && !c.IsDeleted);
        }

        public async Task<bool> IsPrefixDuplicateAsync(string prefix)
        {
            return await _dbContext.Set<Category>().AnyAsync(c => c.Prefix == prefix && !c.IsDeleted);
        }

        public async Task<List<Category>> ListAllActiveAsync()
        {
            return await _dbContext.Set<Category>().AsNoTracking().Where(c => !c.IsDeleted).ToListAsync();
        }
    }
}