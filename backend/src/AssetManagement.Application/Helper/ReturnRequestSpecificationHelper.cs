using AssetManagement.Application.Filter;
using AssetManagement.Domain.Common.Specifications;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Specifications;
using System.Linq.Expressions;

namespace AssetManagement.Application.Helper
{
    public static class ReturnRequestSpecificationHelper
    {
        public static ISpecification<ReturnRequest> CreateSpecReturnRequest(PaginationFilter pagination, string? orderBy, bool? isDescending)
        {
            Expression<Func<ReturnRequest, bool>> criteria = returnRequest => true;
            var spec = new ReturnRequestSpecification(criteria);
            spec.AddInclude(x => x.AcceptedUser);
            spec.AddInclude(x => x.RequestedUser);
            spec.AddInclude(x => x.Assignment);

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
                spec.ApplyOrderByDescending(u => u.CreatedOn);
            }
            spec.ApplyPaging(pagination.PageSize * (pagination.PageIndex - 1), pagination.PageSize);
            return spec;
        }

        public static Expression<Func<ReturnRequest, object>> GetOrderByExpression(string orderBy)
        {
            return orderBy.ToLower() switch
            {
                "assetcode" => u => u.Assignment.Asset.AssetCode,
                "assetname" => u => u.Assignment.Asset.AssetName,
                "requestedby" => u => u.RequestedUser.Username,
                "assigneddate" => u => u.Assignment.AssignedDate,
                "acceptedby" => u => u.AcceptedUser.Username,
                "returneddate" => u => u.ReturnedDate,
                "state" => u => u.ReturnState,
                "createOn" => u => u.CreatedOn,
                "lastModified" => u => u.LastModifiedOn,
                "" => u => u.CreatedOn
            };
        }
    }
}