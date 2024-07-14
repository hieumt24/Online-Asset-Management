using AssetManagement.Application.Filter;
using AssetManagement.Domain.Common.Specifications;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Specifications;
using System.Linq.Expressions;

namespace AssetManagement.Application.Helper
{
    public class AssignmentSpecificationHelper
    {
        public static ISpecification<Assignment> AssignmentSpecificationWithAsset(PaginationFilter filter, string? orderBy, bool? isDescending)
        {
            Expression<Func<Assignment, bool>> criteria = assignment => true;
            var spec = new AssignmentSpecification(criteria);
            spec.Includes.Add(x => x.Asset);
            spec.Includes.Add(x => x.AssignedBy);
            spec.Includes.Add(x => x.AssignedTo);
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
            else
            {
                spec.ApplyOrderByDescending(GetOrderByExpression("createdon"));
            }
            spec.ApplyPaging(filter.PageSize * (filter.PageIndex - 1), filter.PageSize);
            return spec;
        }

        public static Expression<Func<Assignment, object>> GetOrderByExpression(string orderBy)
        {
            return orderBy.ToLower() switch
            {
                "assetcode" => u => u.Asset.AssetCode,
                "assetname" => u => u.Asset.AssetName,
                "assignedto" => u => u.AssignedTo.Username,
                "assignedby" => u => u.AssignedBy.Username,
                "assigneddate" => u => u.AssignedDate,
                "createdon" => u => u.CreatedOn,
                "lastmodifiedon" => u => u.LastModifiedOn,
                "state" => u => u.State,
                _ => u => u.CreatedOn
            };
        }
    }
}