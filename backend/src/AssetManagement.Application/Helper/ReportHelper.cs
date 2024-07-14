using AssetManagement.Application.Models.DTOs.Reports.Responses;
using System.Reflection;

namespace AssetManagement.Application.Helper
{
    public static class ReportHelper
    {
        public static List<ReportResponseDto> ApplySorting(List<ReportResponseDto> reports, string? orderBy, bool? isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = "categoryName";
                isDescending = false;
            }

            var propertyInfo = typeof(ReportResponseDto).GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null)
                return reports;

            reports = isDescending == true
                ? reports.OrderByDescending(r => propertyInfo.GetValue(r, null)).ToList()
                : reports.OrderBy(r => propertyInfo.GetValue(r, null)).ToList();

            return reports;
        }
    }
}