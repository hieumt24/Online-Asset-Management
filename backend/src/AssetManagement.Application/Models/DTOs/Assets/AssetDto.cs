using AssetManagement.Domain.Common.Models;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Models.DTOs.Assets
{
    public class AssetDto : BaseEntity
    {
        public string AssetCode { get; set; } = string.Empty;
        public string AssetName { get; set; } = string.Empty;
        public string Specification { get; set; } = string.Empty;
        public DateTime InstalledDate { get; set; }
        public AssetStateType State { get; set; }
        public EnumLocation AssetLocation { get; set; }
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}