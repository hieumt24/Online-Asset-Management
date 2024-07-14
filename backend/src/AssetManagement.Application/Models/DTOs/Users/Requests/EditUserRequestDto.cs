using AssetManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Application.Models.DTOs.Users.Requests
{
    public class EditUserRequestDto
    {
        public Guid UserId { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public GenderEnum Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoinedDate { get; set; }
        public RoleType Role { get; set; } 
    }
}

