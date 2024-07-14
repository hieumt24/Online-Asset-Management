using AssetManagement.Domain.Common.Models;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Models.DTOs.Assignments.Response
{
    public class AssignmentResponseDto
    {
        public Guid Id { get; set; }

        public Guid AssetId { get; set; }
        public Guid AssignedToId { get; set; }
        public Guid AssignedById { get; set; }
        public string? AssetCode { get; set; }
        public string? AssetName { get; set; }
        public string? Specification { get; set;}
        public string? AssignedTo { get; set; }
        public string? AssignedBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public EnumLocation Location { get; set; }
        public EnumAssignmentState State { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? LastModifiedOn { get; set; }
    }
}