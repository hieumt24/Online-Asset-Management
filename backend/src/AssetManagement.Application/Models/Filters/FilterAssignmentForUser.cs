using AssetManagement.Application.Filter;
using AssetManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Application.Models.Filters
{
    public class FilterAssignmentForUser
    {
        public PaginationFilter pagination { get; set; }
        public Guid userId { get; set; }
        public string? search { get; set; }
        public EnumAssignmentState? assignmentState { get; set; }

        [DataType(DataType.Date)]
        public DateTime? dateFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime? dateTo { get; set; }

        public string? orderBy { get; set; }
        public bool? isDescending { get; set; } = false;
    }
}