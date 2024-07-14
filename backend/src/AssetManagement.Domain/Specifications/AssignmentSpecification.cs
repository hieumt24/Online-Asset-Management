using AssetManagement.Domain.Common.Specifications;
using AssetManagement.Domain.Entites;
using System.Linq.Expressions;

namespace AssetManagement.Domain.Specifications
{
    public class AssignmentSpecification : BaseSpecification<Assignment>
    {
        public AssignmentSpecification(Expression<Func<Assignment, bool>> criteria) : base(criteria)
        {
        }

        public AssignmentSpecification(Expression<Func<Assignment, bool>> criteria, List<string> includeStrings) : base(criteria, includeStrings)
        {
        }
    }
}