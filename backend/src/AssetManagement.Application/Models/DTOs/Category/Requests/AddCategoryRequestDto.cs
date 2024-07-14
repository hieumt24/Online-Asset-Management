using AssetManagement.Domain.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Application.Models.DTOs.Category.Requests
{
    public class AddCategoryRequestDto
    {
        [Required]
        public string CategoryName { get; set; } = string.Empty;
        [Required]
        public string Prefix { get; set; } = string.Empty;
    }
}
