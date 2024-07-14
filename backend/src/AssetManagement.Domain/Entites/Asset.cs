using AssetManagement.Domain.Common.Models;
using AssetManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Domain.Entites
{
    public class Asset : BaseEntity
    {
        [Required]
        public string AssetCode { get; set; } = string.Empty;

        [Required]
        public string AssetName { get; set; } = string.Empty;

        [Required]
        public string Specification { get; set; } = string.Empty;

        [Required]
        public DateTime InstalledDate { get; set; }

        [Required]
        public AssetStateType State { get; set; }

        [Required]
        public EnumLocation AssetLocation { get; set; }

        // relationship
        public Guid CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<Assignment>? Assignments { get; set; }
    }
}