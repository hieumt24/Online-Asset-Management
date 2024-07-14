using AssetManagement.Application.Filter;
using AssetManagement.Application.Models.DTOs.Reports.Responses;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Interfaces.Services
{
    public interface IReportServices
    {
        Task<PagedResponse<ReportResponseDto>> FilterReportAsync(PaginationFilter pagination, EnumLocation location, string? orderBy, bool? isDescending, string? route);

        Task<PagedResponse<List<ReportResponseDto>>> GetReportPaginationAsync(EnumLocation location, PaginationFilter pagination, string? search, string? orderBy, bool? isDescending, string? route);

        Task<Response<List<ReportResponseDto>>> GetReportAsync(EnumLocation location);

        Task<byte[]> ExportReportToExcelAsync(EnumLocation location);
    }
}