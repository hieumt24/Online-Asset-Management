using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Models.DTOs.ReturnRequests
{
    public class ReturnRequestDto
    {
        public Guid AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }
        public Guid RequestedBy { get; set; }
        public User? RequestedUser { get; set; }
        public Guid AcceptedBy { get; set; }
        public User? AcceptedUser { get; set; }
        public EnumReturnRequestState ReturnState { get; set; }
    }
}
