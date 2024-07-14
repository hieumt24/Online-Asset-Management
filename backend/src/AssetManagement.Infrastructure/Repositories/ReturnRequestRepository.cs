using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;
using AssetManagement.Infrastructure.Common;
using AssetManagement.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AssetManagement.Infrastructure.Repositories
{
    public class ReturnRequestRepository : BaseRepositoryAsync<ReturnRequest>, IReturnRequestRepositoriesAsync
    {
        public ReturnRequestRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IQueryable<ReturnRequest>> FilterReturnRequestAsync(EnumLocation adminLocation, string? search, EnumReturnRequestState? returnState, DateTime? returnedDateFrom, DateTime? returnedDateTo)
        {
            var query = _dbContext.ReturnRequests
                .Include(x => x.AcceptedUser)
                .Include(x => x.RequestedUser)
                .Include(x => x.Assignment)
                .ThenInclude(a => a.Asset);
            var queryByAdmin = query.AsNoTracking().Where(x => x.Location == adminLocation && !x.IsDeleted);
            if (!string.IsNullOrEmpty(search))
            {
                queryByAdmin = queryByAdmin.Where(x => x.Assignment.Asset.AssetCode.ToLower().Contains(search.ToLower())
                || x.Assignment.Asset.AssetName.ToLower().Contains(search.ToLower())
                || x.RequestedUser.Username.ToLower().Contains(search.ToLower())
                );
            }
            if (returnState.HasValue)
            {
                queryByAdmin = queryByAdmin.Where(x => x.ReturnState == returnState);
            }
            if (returnedDateFrom.HasValue && returnedDateTo.HasValue)
            {
                var fromDate = returnedDateFrom.Value.Date;
                var toDate = returnedDateTo.Value.Date;

                queryByAdmin = queryByAdmin.Where(x => x.ReturnedDate.HasValue &&
                                                       x.ReturnedDate.Value.Date >= fromDate &&
                                                       x.ReturnedDate.Value.Date <= toDate);
            }

            return queryByAdmin;
        }

        public async Task<ReturnRequest> GetReturnRequestByIdWithAssignmentAsync(Guid returnRequestId)
        {
            var returnRequestIncludeWithAssignment = _dbContext.ReturnRequests.Include(x => x.Assignment);
            var returnRequest = await returnRequestIncludeWithAssignment.FirstOrDefaultAsync(x => x.Id == returnRequestId && !x.IsDeleted);
            return returnRequest;
        }
    }
}