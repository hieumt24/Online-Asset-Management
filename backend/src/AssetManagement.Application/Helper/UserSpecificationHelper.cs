using AssetManagement.Application.Filter;
using AssetManagement.Domain.Common.Specifications;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Specifications;
using System.Linq.Expressions;

namespace AssetManagement.Application.Helper
{
    public static class UserSpecificationHelper
    {
        public static ISpecification<User> CreateSpecification(PaginationFilter pagination, string? orderBy, bool? isDescending)
        {
            Expression<Func<User, bool>> criteria = user => true;

            var spec = new UserSpecification(criteria);

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
                spec.ApplyOrderBy(GetOrderByExpression("fullname"));
            }
            spec.ApplyPaging(pagination.PageSize * (pagination.PageIndex - 1), pagination.PageSize);

            return spec;
        }

        public static Expression<Func<User, object>> GetOrderByExpression(string orderBy)
        {
            switch (orderBy.ToLower())
            {
                case "fullname":
                    return u => u.FirstName + " " + u.LastName;

                case "firstname":
                    return u => u.FirstName;

                case "lastname":
                    return u => u.LastName;

                case "username":
                    return u => u.Username;

                case "role":
                    return u => u.Role;

                case "dateofbirth":
                    return u => u.DateOfBirth;

                case "joinedDate":
                    return u => u.JoinedDate;

                case "gender":
                    return u => u.Gender;

                case "staffcode":
                    return u => u.StaffCode;

                case "lastmodifiedon":
                    return u => u.LastModifiedOn;

                case "createdon":
                    return u => u.CreatedOn;

                default:
                    return u => u.FirstName;
            }
        }

        public static ISpecification<User> GetUserByStaffCode(string staffCode)
        {
            return new UserSpecification(user => user.StaffCode == staffCode);
        }
    }
}