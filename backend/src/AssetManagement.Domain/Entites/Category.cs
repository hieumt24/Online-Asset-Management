using AssetManagement.Domain.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Domain.Entites
{
    public class Category : BaseEntity
    {
        [Required]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        public string Prefix { get; set; } = string.Empty; 

        public virtual ICollection<Asset>? Assets { get; set; }
    }
}
