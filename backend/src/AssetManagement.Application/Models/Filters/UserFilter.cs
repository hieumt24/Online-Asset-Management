using AssetManagement.Application.Filter;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Models.Filters
{
    public class UserFilter
    {
        public PaginationFilter pagination { get; set; }
        public string? search { get; set; }
        public EnumLocation adminLocation { get; set; }
        public RoleType? roleType { get; set; }
        public string? orderBy { get; set; }
        public bool isDescending { get; set; } = false;
    }
}