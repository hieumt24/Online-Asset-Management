using AssetManagement.Application.Filter;
using AssetManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Application.Models.Filters
{
    public class ReturnRequestFilter
    {
        public PaginationFilter pagination { get; set; }
        public EnumReturnRequestState? returnState { get; set; }

        [DataType(DataType.Date)]
        public DateTime? returnedDateFrom { get; set; }

        public DateTime? returnedDateTo {  get; set; }

        public string? search { get; set; }
        public string? orderBy { get; set; }
        public bool? isDescending { get; set; } = false;
        public EnumLocation location { get; set; }
    }
}