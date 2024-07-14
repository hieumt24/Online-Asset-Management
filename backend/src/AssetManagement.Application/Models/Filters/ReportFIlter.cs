using AssetManagement.Application.Filter;
using AssetManagement.Application.Helper;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Models.Filters
{
    public class ReportFIlter
    {
        public EnumLocation Location { get; set; }
        public string? search { get; set; }
        public PaginationFilter Pagination { get; set; }
        public string? OrderBy { get; set; }
        public bool? IsDescending { get; set; }
    }
}