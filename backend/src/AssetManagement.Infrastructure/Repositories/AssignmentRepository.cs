using AssetManagement.Application.Filter;
using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;
using AssetManagement.Infrastructure.Common;
using AssetManagement.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AssetManagement.Infrastructure.Repositories
{
    public class AssignmentRepository : BaseRepositoryAsync<Assignment>, IAssignmentRepositoriesAsync
    {
        public AssignmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IQueryable<Assignment>> FilterAssignmentAsync(EnumLocation location, string? search, EnumAssignmentState? assignmentState, DateTime? dateFrom, DateTime? dateTo)
        {
            var includeAssignment = _dbContext.Assignments.Include(x => x.Asset).Include(x => x.AssignedBy).Include(x => x.AssignedTo);
            var query = includeAssignment.Where(x => x.Location == location && !x.IsDeleted).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Asset.AssetCode.ToLower().Contains(search.ToLower())
                                        || x.Asset.AssetName.ToLower().Contains(search.ToLower())
                                        || x.AssignedBy.Username.ToLower().Contains(search.ToLower())
                                        || x.AssignedTo.Username.ToLower().Contains(search.ToLower()));
            }
            if (assignmentState.HasValue)
            {
                query = query.Where(x => x.State == assignmentState);
            }
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                query = query.Where(p => p.AssignedDate.Date >= dateFrom && p.AssignedDate.Date <= dateTo);
            }
            // if state return request is complete not display in list assignment
            query = query.Where(x => x.ReturnRequest == null || x.ReturnRequest.ReturnState != EnumReturnRequestState.Completed);
            return query;
        }

        public async Task<IQueryable<Assignment>> GetAssignmentsByUserId(Guid userId)
        {
            return _dbContext.Assignments
                    .Include(x => x.Asset)
                    .Where(x => !x.IsDeleted
                        && x.AssignedIdTo == userId
                        && (x.ReturnRequest == null || x.ReturnRequest.ReturnState != EnumReturnRequestState.Completed)
                        && (x.State != EnumAssignmentState.Declined)
                        && (x.AssignedDate <= DateTime.Now));
        }

        public async Task<Assignment> GetAssignemntByIdAsync(Guid assignmentId)
        {
            return await _dbContext.Assignments.Include(x => x.Asset)
                                         .Include(x => x.AssignedTo)
                                         .Include(x => x.AssignedBy)
                                         .Where(x => x.Id == assignmentId && !x.IsDeleted)
                                         .FirstOrDefaultAsync();
        }

        public async Task<Assignment> FindExistingAssignment(Guid assetId)
        {
            return await _dbContext.Assignments
                .FirstOrDefaultAsync(assigment => assigment.AssetId == assetId && !assigment.IsDeleted);
        }

        public async Task<IQueryable<Assignment>> FilterAssignmentOfUserAsync(Guid userId, string? search, EnumAssignmentState? assignmentState, DateTime? dateFrom, DateTime? dateTo)
        {
            var query = _dbContext.Assignments
                        .Include(x => x.Asset)
                        .Where(x => x.AssignedIdTo == userId
                            && (x.ReturnRequestId == null || x.State == EnumAssignmentState.WaitingForReturning)
                            && x.State != EnumAssignmentState.Returned
                            && x.State != EnumAssignmentState.Declined
                            && x.AssignedDate <= DateTime.Now
                            && !x.IsDeleted
                            && (string.IsNullOrEmpty(search) || x.Asset.AssetCode.ToLower().Contains(search.ToLower())
                                                            || x.Asset.AssetName.ToLower().Contains(search.ToLower())
                                                            || x.AssignedBy.Username.ToLower().Contains(search.ToLower()))
                        );
            query = query.Where(x => !assignmentState.HasValue || x.State == assignmentState);
            query = query.Where(x => !(dateFrom.HasValue && dateTo.HasValue) || (x.AssignedDate.Date >= dateFrom && x.AssignedDate.Date <= dateTo));
            return query;
        }

        public async Task<IQueryable<Assignment>> FilterAssignmentByAssetIdAsync(Guid assetId, string? search, EnumAssignmentState? assignmentState, DateTime? dateFrom, DateTime? dateTo)
        {
            var query = _dbContext.Assignments
                        .Include(x => x.Asset)
                        .Where(x => !x.IsDeleted
                            && x.AssetId == assetId
                            && x.State != EnumAssignmentState.Declined);
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Asset.AssetCode.ToLower().Contains(search.ToLower())
                                        || x.Asset.AssetName.ToLower().Contains(search.ToLower())
                                        || x.AssignedTo.Username.ToLower().Contains(search.ToLower()));
            }
            if (assignmentState.HasValue)
            {
                query = query.Where(x => x.State == assignmentState);
            }
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                query = query.Where(p => p.AssignedDate.Date >= dateFrom && p.AssignedDate.Date <= dateTo);
            }

            return query;
        }

        public async Task<IQueryable<Assignment>> GetAssignmentsByAssetId(Guid assetId)
        {
            return _dbContext.Assignments
                  .Include(x => x.Asset)
                  .Where(x => x.AssetId == assetId && !x.IsDeleted && x.State != EnumAssignmentState.Declined);
        }

        public async Task<(IEnumerable<Assignment> Data, int TotalRecords)> GetAllMatchingAssignmentAsync(EnumLocation location, string? search, EnumAssignmentState? assignmentState, DateTime? dateFrom, DateTime? dateTo, string? orderBy, bool? isDescending, PaginationFilter pagination)
        {
            string searchPhraseLower = search?.ToLower() ?? string.Empty;

            var query = _dbContext.Assignments
                        .Include(x => x.Asset)
                        .Include(x => x.AssignedBy)
                        .Include(x => x.AssignedTo)
                        .AsNoTracking()
                        .Where(x => x.Location == location
                            && !x.IsDeleted
                            && (x.ReturnRequestId == null || x.State == EnumAssignmentState.WaitingForReturning)
                            && x.State != EnumAssignmentState.Returned
                            && (string.IsNullOrEmpty(search) || x.Asset.AssetCode.ToLower().Contains(searchPhraseLower)
                                                            || x.Asset.AssetName.ToLower().Contains(searchPhraseLower)
                                                            || x.AssignedBy.Username.ToLower().Contains(searchPhraseLower)
                                                            || x.AssignedTo.Username.ToLower().Contains(searchPhraseLower))

                        );

            query = query.Where(x => !assignmentState.HasValue || x.State == assignmentState);
            query = query.Where(x => !(dateFrom.HasValue && dateTo.HasValue) || (x.AssignedDate.Date >= dateFrom && x.AssignedDate.Date <= dateTo));
            var totalRecords = await query.CountAsync();

            if (!string.IsNullOrEmpty(orderBy))
            {
                // create on -> Assigned Date -> State -> Asset Code -> Asset Name -> Assigned To -> Assigned By
                var columnsSelector = new Dictionary<string, Expression<Func<Assignment, object>>>
                {
                    { "createdon", x => x.CreatedOn },
                    { "assigneddate", x => x.AssignedDate },
                    { "state", x => x.State },
                    { "assetcode", x => x.Asset.AssetCode },
                    { "assetname", x => x.Asset.AssetName },
                    { "assignedto", x => x.AssignedTo.Username },
                    { "assignedby", x => x.AssignedBy.Username },
                    { "lastmodifiedon", x => x.LastModifiedOn }
                };

                if (columnsSelector.ContainsKey(orderBy.ToLower()))
                {
                    if (isDescending.HasValue && isDescending.Value)
                    {
                        query = query.OrderByDescending(columnsSelector[orderBy.ToLower()])
                            .ThenByDescending(x => x.CreatedOn)
                            .ThenByDescending(x => x.AssignedDate)
                            .ThenByDescending(x => x.State)
                            .ThenByDescending(x => x.Asset.AssetCode)
                            .ThenByDescending(x => x.Asset.AssetName)
                            .ThenByDescending(x => x.AssignedTo.Username)
                            .ThenByDescending(x => x.AssignedBy.Username);
                    }
                    else
                    {
                        query = query.OrderBy(columnsSelector[orderBy.ToLower()])
                            .ThenBy(x => x.CreatedOn)
                            .ThenBy(x => x.AssignedDate)
                            .ThenBy(x => x.State)
                            .ThenBy(x => x.Asset.AssetCode)
                            .ThenBy(x => x.Asset.AssetName)
                            .ThenBy(x => x.AssignedTo.Username)
                            .ThenBy(x => x.AssignedBy.Username);
                    }
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.AssignedDate)
                    //.ThenBy(x => x.AssignedDate)
                    .ThenBy(x => x.State)
                    .ThenBy(x => x.Asset.AssetCode)
                    .ThenBy(x => x.Asset.AssetName)
                    .ThenBy(x => x.AssignedTo.Username)
                    .ThenBy(x => x.AssignedBy.Username);
            }

            var assignments = await query
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (Data: assignments, TotalRecords: totalRecords);
        }
    }
}