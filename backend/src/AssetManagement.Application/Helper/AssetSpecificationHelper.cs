using AssetManagement.Application.Filter;
using AssetManagement.Domain.Common.Specifications;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;
using AssetManagement.Domain.Specifications;
using System.Linq.Expressions;

namespace AssetManagement.Application.Helper
{
    public class AssetSpecificationHelper
    {
        public static ISpecification<Asset> AssetSpecificationWithCategory(PaginationFilter pagination, string? orderBy, bool? isDescending)
        {
            Expression<Func<Asset, bool>> criteria = asset => true;
            var spec = new AssetSpecification(criteria);
            spec.AddInclude(x => x.Category);
            if (string.IsNullOrEmpty(orderBy))
            {
                spec.ApplyOrderBy(u => u.AssetCode);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                if (isDescending.HasValue && isDescending.Value)
                {
                    spec.ApplyOrderByDescending(GetOrderByExpression(orderBy));
                }
                else
                {
                    spec.ApplyOrderBy(GetOrderByExpression(orderBy));
                }
            }

            spec.ApplyPaging(pagination.PageSize * (pagination.PageIndex - 1), pagination.PageSize);
            return spec;
        }

        public static Expression<Func<Asset, object>> GetOrderByExpression(string orderBy)
        {
            return orderBy.ToLower() switch
            {
                "assetcode" => u => u.AssetCode,
                "assetname" => u => u.AssetName,
                "installeddate" => u => u.InstalledDate,
                "state" => u => u.State,
                "assetlocation" => u => u.AssetLocation,
                "categoryname" => u => u.Category.CategoryName,
                "createdon" => u => u.CreatedOn,
                "lastmodifiedon" => u => u.LastModifiedOn,
                _ => u => u.AssetCode,
            };
        }

        public static ISpecification<Asset> GetAssetByAssetCode(string assetCode)
        {
            var spec = new AssetSpecification(asset => asset.AssetCode == assetCode && !asset.IsDeleted);
            spec.AddInclude(x => x.Category);
            return spec;
        }

        public static ISpecification<Asset> GetAssetById(Guid id)
        {
            var spec = new AssetSpecification(asset => asset.Id == id && !asset.IsDeleted);
            spec.AddInclude(x => x.Category);
            return spec;
        }

        public static ISpecification<Asset> GetAllAssets(EnumLocation location)
        {
            var spec = new AssetSpecification(asset => asset.AssetLocation == location && !asset.IsDeleted);
            return spec;
        }
    }
}