using AssetManagement.Application.Common;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Interfaces.Repositories
{
    public interface IReturnRequestRepositoriesAsync : IBaseRepositoryAsync<ReturnRequest>
    {
        Task<IQueryable<ReturnRequest>> FilterReturnRequestAsync(EnumLocation adminLocation, string? search, EnumReturnRequestState? returnState, DateTime? returnedDateFrom, DateTime? returnedDateTo);
        Task<ReturnRequest> GetReturnRequestByIdWithAssignmentAsync(Guid returnRequestId);
    }
}