using AssetManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Application.Models.DTOs.Assignments.Request
{
    public class AddAssignmentRequestDto
    {
        [Required]
        public Guid AssignedIdTo { get; set; }
        [Required]
        public Guid AssignedIdBy { get; set; }
        [Required]
        public Guid AssetId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime AssignedDate { get; set; }
        public string Note { get; set; } = string.Empty;
        [Required]
        public EnumLocation Location { get; set; }
        [Required]
        public EnumAssignmentState State { get; set; } = EnumAssignmentState.WaitingForAcceptance;

    }
}
