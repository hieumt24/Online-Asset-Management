using AssetManagement.Application.Common;
using AssetManagement.Application.Filter;
using AssetManagement.Application.Models.DTOs.Users;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Interfaces.Repositories
{
    public interface IUserRepositoriesAsync : IBaseRepositoryAsync<User>
    {
        Task<User> FindByUsernameAsync(string username);

        string GeneratePassword(string userName, DateTime dateOfBirth);

        Task<string> GenerateUsername(string firstName, string lastName);

        Task<RoleType> GetRoleAsync(Guid userId);

        Task<bool> IsUsernameExist(string username);

        IQueryable<User> Query(EnumLocation adminLocation);

        Task<IQueryable<User>> FilterUserAsync(EnumLocation adminLocation, string? search, RoleType? roleType);

        Task<(IEnumerable<User> Data, int TotalRecords)> GetAllMatchingUserAsync(EnumLocation admimLocation, string? search, RoleType? roleType, string? orderBy, bool? isDescending, PaginationFilter pagination);

        Task<User> UpdateUserAysnc(User user);
    }
}