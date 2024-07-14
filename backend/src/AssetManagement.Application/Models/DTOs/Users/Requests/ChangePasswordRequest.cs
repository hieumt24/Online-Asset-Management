using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Application.Models.DTOs.Users.Requests
{
    public class ChangePasswordRequest
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}