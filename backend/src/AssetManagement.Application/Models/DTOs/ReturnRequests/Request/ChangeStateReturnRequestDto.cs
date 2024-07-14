using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Models.DTOs.ReturnRequests.Request
{
    public class ChangeStateReturnRequestDto
    {
        public Guid ReturnRequestId { get; set; }
        public EnumReturnRequestState NewState { get; set; }
        public Guid AcceptedBy { get; set; }
    }
}
