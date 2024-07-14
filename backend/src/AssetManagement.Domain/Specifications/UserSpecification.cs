using AssetManagement.Domain.Common.Specifications;
using AssetManagement.Domain.Entites;
using System.Linq.Expressions;

namespace AssetManagement.Domain.Specifications
{
    public class UserSpecification : BaseSpecification<User>
    {
        public UserSpecification(Expression<Func<User, bool>> criteria) : base(criteria)
        {
        }

        public UserSpecification(Expression<Func<User, bool>> criteria, List<string> includeStrings) : base(criteria, includeStrings)
        {
        }
    }
}