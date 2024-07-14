using AssetManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Application.Models.DTOs.Assets.Requests
{
    public class AddAssetRequestDto
    {
        [Required]
        public string AdminId { get; set; }

        [Required]
        public string AssetName { get; set; } = string.Empty;

        [Required]
        public string Specification { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime InstalledDate { get; set; } = DateTime.Now;

        [Required]
        public AssetStateType State { get; set; }

        [Required]
        public EnumLocation AssetLocation { get; set; }

        // relationship
        public Guid CategoryId { get; set; }
    }
}