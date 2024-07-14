using AssetManagement.Application.Filter;
using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;
using AssetManagement.Infrastructure.Common;
using AssetManagement.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AssetManagement.Infrastructure.Repositories
{
    public class AssetRepository : BaseRepositoryAsync<Asset>, IAssetRepositoriesAsync
    {
        public AssetRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IQueryable<Asset>> FilterAsset(EnumLocation adminLocation, string? search, Guid? categoryId, ICollection<AssetStateType?>? assetStateType)
        {
            //check asset by adminLocation

            var query = _dbContext.Assets.AsNoTracking().Where(x => x.AssetLocation == adminLocation && !x.IsDeleted);
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.AssetName.ToLower().Contains(search.ToLower()) || x.AssetCode.ToLower().Contains(search.ToLower()));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }
            if (assetStateType != null && assetStateType.Count > 0)
            {
                query = query.Where(x => assetStateType.Contains(x.State));
            }
            else
            {
                query = query.Where(x => x.State == AssetStateType.Available
                                                       || x.State == AssetStateType.NotAvailable
                                                       || x.State == AssetStateType.Assigned
                                                       || x.State == AssetStateType.WaitingForRecycling
                                                       || x.State == AssetStateType.Recycled
                                                       || x.State == AssetStateType.Recycled);
            }
            return query;
        }

        public async Task<Asset> FindExitingCategory(Guid categoryId)
        {
            return await _dbContext.Set<Asset>()
                .FirstOrDefaultAsync(asset => asset.CategoryId == categoryId);
        }

        public async Task<string> GenerateAssetCodeAsync(Guid CategoryId)
        {
            // Get the category to access its prefix
            var category = await _dbContext.Categories.FindAsync(CategoryId);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            // Use the prefix from the category
            string prefix = category.Prefix.ToUpper();

            // Get the last asset code for this category
            var lastAssetCode = await _dbContext.Assets
                .Where(a => a.CategoryId == CategoryId && a.AssetCode.StartsWith(prefix) && !a.IsDeleted)
                .OrderByDescending(a => a.AssetCode)
                .Select(a => a.AssetCode)
                .FirstOrDefaultAsync();

            // Generate the new asset code
            int newNumber = 1;
            if (lastAssetCode != null)
            {
                int lastNumber = int.Parse(lastAssetCode.Substring(prefix.Length));
                newNumber = lastNumber + 1;
            }

            return $"{prefix}{newNumber:D6}";
        }

        public async Task<(IEnumerable<Asset> Data, int TotalRecords)> GetAllMatchingAssetAsync(EnumLocation adminLocation, string? search, Guid? categoryId, ICollection<AssetStateType?>? assetStateType, string? orderBy, bool? isDescending, PaginationFilter pagination)
        {
            string searchPhraseLower = string.Empty;
            if (!string.IsNullOrEmpty(search))
            {
                searchPhraseLower = search?.ToLower();
            }

            var baseQuery = _dbContext.Assets.Where(x => x.AssetLocation == adminLocation && !x.IsDeleted).Include(x => x.Category);

            var searchQuery = baseQuery.Where(x => x.AssetName.ToLower().Contains(searchPhraseLower)
                                                || x.AssetCode.ToLower().Contains(searchPhraseLower));

            if (categoryId.HasValue)
            {
                searchQuery = searchQuery.Where(x => x.CategoryId == categoryId);
            }
            if (assetStateType != null && assetStateType.Count > 0)
            {
                searchQuery = searchQuery.Where(x => assetStateType.Contains(x.State));
            }
            else
            {
                searchQuery = searchQuery.Where(x => x.State == AssetStateType.Available
                                                       || x.State == AssetStateType.NotAvailable
                                                       || x.State == AssetStateType.Assigned
                                                       || x.State == AssetStateType.WaitingForRecycling
                                                       || x.State == AssetStateType.Recycled
                                                       || x.State == AssetStateType.Recycled);
            }

            var totalRecords = await searchQuery.CountAsync();

            if (!string.IsNullOrEmpty(orderBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Asset, object>>>
                {
                    { "assetname", x => x.AssetName },
                    { "assetcode", x => x.AssetCode },
                    { "installeddate", x => x.InstalledDate },
                    { "state", x => x.State },
                    { "assetlocation", x => x.AssetLocation },
                    { "categoryname", x => x.Category.CategoryName },
                    { "createdon", x => x.CreatedOn },
                    { "lastmodifiedon", x => x.LastModifiedOn }
                };

                if (columnsSelector.ContainsKey(orderBy.ToLower()))
                {
                    if (isDescending.HasValue && isDescending.Value)
                    {
                        //asset code -> asset name -> category name -> state
                        searchQuery = searchQuery.OrderByDescending(columnsSelector[orderBy.ToLower()])
                            .ThenByDescending(x => x.AssetCode)
                            .ThenByDescending(x => x.AssetName)
                            .ThenByDescending(x => x.Category.CategoryName)
                            .ThenByDescending(x => x.State);
                    }
                    else
                    {
                        searchQuery = searchQuery.OrderBy(columnsSelector[orderBy.ToLower()])
                            .ThenBy(x => x.AssetCode)
                            .ThenBy(x => x.AssetName)
                            .ThenBy(x => x.Category.CategoryName)
                            .ThenBy(x => x.State);
                    }
                }
            }
            else
            {
                searchQuery = searchQuery.OrderBy(x => x.AssetCode)
                    .ThenBy(x => x.AssetName)
                    .ThenBy(x => x.Category.CategoryName)
                    .ThenBy(x => x.State);
            }

            var assets = await searchQuery.Skip((pagination.PageIndex - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();
            return (Data: assets, TotalRecords: totalRecords);
        }
    }
}