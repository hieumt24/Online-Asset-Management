using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Models.DTOs.Users.Requests
{
    public class UpdateUserRequestDto
    {
        public Guid UserId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime JoinedDate { get; set; }
        public List<int> RoleIds { get; set; } = new List<int>();
    }
}

