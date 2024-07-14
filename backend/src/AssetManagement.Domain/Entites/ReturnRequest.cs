using AssetManagement.Domain.Common.Models;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Domain.Entites
{
    public class ReturnRequest : BaseEntity
    {
        public Guid AssignmentId { get; set; }
       
        public Guid RequestedBy { get; set; }
        public Guid? AcceptedBy { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public EnumLocation Location { get; set; }
        public EnumReturnRequestState ReturnState { get; set; }
        public User? RequestedUser { get; set; }
        public User? AcceptedUser { get; set; }
        public Assignment? Assignment { get; set; }
    }
}