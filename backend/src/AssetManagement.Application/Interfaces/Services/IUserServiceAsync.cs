using AssetManagement.Application.Filter;
using AssetManagement.Application.Models.DTOs.Users;
using AssetManagement.Application.Models.DTOs.Users.Requests;
using AssetManagement.Application.Models.DTOs.Users.Responses;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.Interfaces.Services
{
    public interface IUserServiceAsync
    {
        Task<Response<UserDto>> AddUserAsync(AddUserRequestDto request);

        Task<PagedResponse<List<UserResponseDto>>> GetAllUsersAsync(PaginationFilter pagination, string? search, EnumLocation adminLocation, RoleType? roleType, string? orderBy, bool? isDescending, string? route);

        Task<Response<UserResponseDto>> GetUserByIdAsync(Guid userId);

        Task<Response<UserDto>> EditUserAsync(EditUserRequestDto request);

        Task<Response<UserDto>> DisableUserAsync(Guid userId);

        Task<Response<UserDto>> ResetPasswordAsync(Guid userId);

        Task<Response<UserResponseDto>> GetUserByStaffCodeAsync(string staffCode);

        Task<Response<bool>> IsValidDisableUser(Guid userId);
    }
}