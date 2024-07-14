using AssetManagement.Application.Filter;
using AssetManagement.Application.Models.DTOs.Assignments;
using AssetManagement.Application.Models.DTOs.Assignments.Reques;
using AssetManagement.Application.Models.DTOs.Assignments.Request;
using AssetManagement.Application.Models.DTOs.Assignments.Requests;
using AssetManagement.Application.Models.DTOs.Assignments.Response;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Interfaces.Services
{
    public interface IAssignmentServicesAsync
    {
        Task<Response<AssignmentDto>> AddAssignmentAsync(AddAssignmentRequestDto request);

        Task<Response<AssignmentResponseDto>> GetAssignmentByIdAsync(Guid assignmentId);

        Task<Response<AssignmentDto>> EditAssignmentAsync(EditAssignmentRequestDto request, Guid assignmentId);

        Task<Response<AssignmentDto>> DeleteAssignmentAsync(Guid assignmentId);

        Task<PagedResponse<List<AssignmentResponseDto>>> GetAssignmentsOfUser(PaginationFilter paginationFilter, Guid userId, string? search, EnumAssignmentState? assignmentState, DateTime? dateFrom, DateTime? dateTo, string? orderBy, bool? isDescending, string? route);

        Task<PagedResponse<List<AssignmentResponseDto>>> GetAssetAssign(PaginationFilter paginationFilter, Guid assetId, string? search, EnumAssignmentState? assignmentState, DateTime? dateFrom, DateTime? dateTo, string? orderBy, bool? isDescending, string? route);

        Task<Response<AssignmentDto>> ChangeAssignmentStateAsync(ChangeStateAssignmentDto request);

        Task<PagedResponse<List<AssignmentResponseDto>>> GetAllAssignmentsAsync(PaginationFilter paginationFilter, string? search, EnumAssignmentState? assignmentState, DateTime? dateFrom, DateTime? dateTo, EnumLocation adminLocation, string? orderBy, bool? isDescending, string? route);
       
    }
}