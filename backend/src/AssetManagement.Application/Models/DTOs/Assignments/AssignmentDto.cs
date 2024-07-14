using AssetManagement.Domain.Common.Models;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Models.DTOs.Assignments
{
    public class AssignmentDto : BaseEntity
    {
        public Guid Id { get; set; }
        public string? AssetCode { get; set; }
        public string? AssetName { get; set; }
        public string? AssignedTo { get; set; }
        public string? AssignedBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public EnumLocation Location { get; set; }
        public EnumAssignmentState State { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}