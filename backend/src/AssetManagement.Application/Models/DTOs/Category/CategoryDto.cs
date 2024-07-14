using AssetManagement.Domain.Common.Models;

namespace AssetManagement.Application.Models.DTOs.Category
{
    public class CategoryDto : BaseEntity
    {
        public string CategoryName { get; set; } = string.Empty;
        public string Prefix { get; set; } = string.Empty;
    }
}
