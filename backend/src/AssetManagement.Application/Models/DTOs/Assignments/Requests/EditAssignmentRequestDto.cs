using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Models.DTOs.Assignments.Reques
{
    public class EditAssignmentRequestDto
    {
        public Guid AssignedIdTo { get; set; }
        public Guid AssignedIdBy { get; set; }
        public Guid AssetId { get; set; }
        public DateTime AssignedDate { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}