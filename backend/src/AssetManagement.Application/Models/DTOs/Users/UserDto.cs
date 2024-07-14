using AssetManagement.Domain.Common.Models;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.Application.Models.DTOs.Users
{
    public class UserDto : BaseEntity
    {
    
        [Required]
        [MinLength(2, ErrorMessage = "Min length of First Name is 2 characters")]
        [MaxLength(50, ErrorMessage = "Max length of First Name is 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MinLength(2, ErrorMessage = "Min length of Last Name is 2 characters")]
        [MaxLength(50, ErrorMessage = "Max length of Last Name is 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTime JoinedDate { get; set; } = DateTime.Now.Date;

        [Required]
        public GenderEnum Gender { get; set; } = GenderEnum.Unknown;

        [Required]
        public RoleType Role { get; set; } = RoleType.Staff;

        [Required]
        [RegularExpression(@"^SD\d{4}$", ErrorMessage = "StaffCode must be in the format SDxxxx where xxxx are digits.")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string StaffCode { get; set; } = string.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StaffCodeId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public EnumLocation Location { get; set; }

        [Required]
        public bool IsFirstTimeLogin { get; set; } = true;
    }
}