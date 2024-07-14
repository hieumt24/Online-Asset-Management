using AssetManagement.Application.Common;
using AssetManagement.Application.Filter;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Interfaces.Repositories
{
    public interface IAssignmentRepositoriesAsync : IBaseRepositoryAsync<Assignment>
    {
        Task<IQueryable<Assignment>> FilterAssignmentAsync(EnumLocation location, string? search, EnumAssignmentState? assignmentState, DateTime? dateFrom, DateTime? dateTo);

        Task<(IEnumerable<Assignment> Data, int TotalRecords)> GetAllMatchingAssignmentAsync(EnumLocation location, string? search, EnumAssignmentState? assignmentState, DateTime? dateFrom, DateTime? dateTo, string? orderBy, bool? isDescending, PaginationFilter pagination);

        Task<IQueryable<Assignment>> GetAssignmentsByUserId(Guid userId);

        Task<IQueryable<Assignment>> GetAssignmentsByAssetId(Guid assetId);

        Task<Assignment> GetAssignemntByIdAsync(Guid assignmentId);

        Task<IQueryable<Assignment>> FilterAssignmentOfUserAsync(Guid userId, string? search, EnumAssignmentState? assignmentState, DateTime? dateFrom, DateTime? dateTo);

        Task<IQueryable<Assignment>> FilterAssignmentByAssetIdAsync(Guid assetId, string? search, EnumAssignmentState? assignmentState, DateTime? dateFrom, DateTime? dateTo);

        Task<Assignment> FindExistingAssignment(Guid assetId);
    }
}