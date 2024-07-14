using AssetManagement.Domain.Common.Specifications;
using AssetManagement.Domain.Entites;
using System.Linq.Expressions;

namespace AssetManagement.Domain.Specifications
{
    public class ReturnRequestSpecification : BaseSpecification<ReturnRequest>
    {
        public ReturnRequestSpecification(Expression<Func<ReturnRequest, bool>> criteria) : base(criteria)
        {
        }

        public ReturnRequestSpecification(Expression<Func<ReturnRequest, bool>> criteria, List<string> includeStrings) : base(criteria, includeStrings)
        {
        }
    }
}