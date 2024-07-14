using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Models.DTOs.Users.Responses
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime JoinedDate { get; set; }

        public GenderEnum Gender { get; set; }

        public RoleType Role { get; set; }

        public string StaffCode { get; set; }

        public string Username { get; set; }

        public EnumLocation Location { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset LastModifiedOn { get; set; }
    }
}