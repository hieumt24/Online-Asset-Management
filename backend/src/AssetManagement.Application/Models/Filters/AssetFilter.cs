using AssetManagement.Application.Filter;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Models.Filters
{
    public class AssetFilter
    {
        public PaginationFilter pagination { get; set; }
        public string? search { get; set; }
        public Guid? categoryId { get; set; }
        public ICollection<AssetStateType?>? assetStateType { get; set; }
        public EnumLocation adminLocation { get; set; }
        public string? orderBy { get; set; }
        public bool? isDescending { get; set; }
    }
}