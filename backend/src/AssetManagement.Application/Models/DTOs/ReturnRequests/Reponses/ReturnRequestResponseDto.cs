using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Models.DTOs.ReturnRequests.Reponses
{
    public class ReturnRequestResponseDto
    {
        public Guid Id { get; set; }
        public string? AssetCode { get; set; }
        public string? AssetName { get; set; }
        public string? RequestedBy { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string? AcceptedBy { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public EnumReturnRequestState? State { get; set; }
        public EnumLocation Location { get; set; }
    }
}