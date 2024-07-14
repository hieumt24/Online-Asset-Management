using AssetManagement.Domain.Common.Specifications;
using AssetManagement.Domain.Entites;
using System.Linq.Expressions;

namespace AssetManagement.Domain.Specifications
{
    public class AssetSpecification : BaseSpecification<Asset>
    {
        public AssetSpecification(Expression<Func<Asset, bool>> criteria) : base(criteria)
        {
        }

        public AssetSpecification(Expression<Func<Asset, bool>> criteria, List<string> includeStrings) : base(criteria, includeStrings)
        {
        }
    }
}