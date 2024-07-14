using AssetManagement.Domain.Common.Models;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Domain.Entites
{
    public class Assignment : BaseEntity
    {
        public Guid AssignedIdTo { get; set; }
        public Guid AssignedIdBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public Guid AssetId { get; set; }
        public Guid? ReturnRequestId { get; set; }
        public EnumLocation? Location { get; set; }
        public EnumAssignmentState State { get; set; } 
        public string Note { get; set; } = string.Empty;

        //relationship 
        public User? AssignedBy { get; set; }
        public User? AssignedTo { get; set; }
        public Asset? Asset { get; set; }
        public ReturnRequest? ReturnRequest { get; set; }
    }
}
