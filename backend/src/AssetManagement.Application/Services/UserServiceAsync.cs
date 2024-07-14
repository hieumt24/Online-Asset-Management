using AssetManagement.Application.Common;
using AssetManagement.Application.Filter;
using AssetManagement.Application.Helper;
using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Users;
using AssetManagement.Application.Models.DTOs.Users.Requests;
using AssetManagement.Application.Models.DTOs.Users.Responses;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Entites;
using AssetManagement.Domain.Enums;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AssetManagement.Application.Services
{
    public class UserServiceAsync : IUserServiceAsync
    {
        private readonly IMapper _mapper;
        private readonly IUserRepositoriesAsync _userRepositoriesAsync;
        private readonly IValidator<AddUserRequestDto> _addUserValidator;
        private readonly IValidator<EditUserRequestDto> _editUserValidator;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IUriService _uriService;
        private readonly IAssignmentRepositoriesAsync _assignmentRepositoriesAsync;
        private readonly ITokenRepositoriesAsync _tokenRepositoriesAsync;
        private readonly IBlackListTokensRepositoriesAsync _blackListTokensRepositoriesAsync;

        public UserServiceAsync(IUserRepositoriesAsync userRepositoriesAsync,
            IMapper mapper,
            IValidator<AddUserRequestDto> addUserValidator,
            IValidator<EditUserRequestDto> editUserValidator,
            IUriService uriService,
            IAssignmentRepositoriesAsync assignmentRepositoriesAsync,
            ITokenRepositoriesAsync tokenRepositoriesAsync,
            IBlackListTokensRepositoriesAsync blackListTokensRepositoriesAsync
        )
        {
            _mapper = mapper;
            _userRepositoriesAsync = userRepositoriesAsync;
            _addUserValidator = addUserValidator;
            _editUserValidator = editUserValidator;
            _passwordHasher = new PasswordHasher<User>();
            _uriService = uriService;
            _assignmentRepositoriesAsync = assignmentRepositoriesAsync;
            _tokenRepositoriesAsync = tokenRepositoriesAsync;
            _blackListTokensRepositoriesAsync = blackListTokensRepositoriesAsync;
        }

        public async Task<Response<UserDto>> AddUserAsync(AddUserRequestDto request)
        {
            var validationResult = await _addUserValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return new Response<UserDto> { Succeeded = false, Errors = errors };
            }

            try
            {
                var userDomain = _mapper.Map<User>(request);

                userDomain.Username = await _userRepositoriesAsync.GenerateUsername(userDomain.FirstName, userDomain.LastName);
                userDomain.PasswordHash = _passwordHasher.HashPassword(userDomain, _userRepositoriesAsync.GeneratePassword(userDomain.Username, userDomain.DateOfBirth));
                userDomain.IsDeleted = false;
                userDomain.CreatedOn = DateTime.Now;
                userDomain.LastModifiedOn = DateTime.Now;

                var user = await _userRepositoriesAsync.AddAsync(userDomain);

                var userDto = _mapper.Map<UserDto>(user);

                return new Response<UserDto>();
            }
            catch (Exception ex)
            {
                return new Response<UserDto> { Succeeded = false, Errors = { ex.Message } };
            }
        }

        public async Task<Response<UserResponseDto>> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var user = await _userRepositoriesAsync.GetByIdAsync(userId);
                if (user == null)
                {
                    return new Response<UserResponseDto>("User not found");
                }

                var userDto = _mapper.Map<UserResponseDto>(user);
                return new Response<UserResponseDto> { Data = userDto, Succeeded = true };
            }
            catch (Exception ex)
            {
                return new Response<UserResponseDto> { Succeeded = false, Errors = { ex.Message } };
            }
        }

        public async Task<PagedResponse<List<UserResponseDto>>> GetAllUsersAsync(PaginationFilter pagination, string? search, EnumLocation adminLocation, RoleType? roleType, string? orderBy, bool? isDescending, string? route)
        {
            try
            {
                if (pagination == null)
                {
                    pagination = new PaginationFilter();
                }
                //queryable user filter condition
                //var filterUser = await _userRepositoriesAsync.FilterUserAsync(adminLocation, search, roleType);
                //var totalRecords = await filterUser.CountAsync();

                //var specUser = UserSpecificationHelper.CreateSpecification(pagination, orderBy, isDescending);

                //var users = await SpecificationEvaluator<User>.GetQuery(filterUser, specUser).ToListAsync();
                var users = await _userRepositoriesAsync.GetAllMatchingUserAsync(adminLocation, search, roleType, orderBy, isDescending, pagination);

                var userDtos = _mapper.Map<List<UserResponseDto>>(users.Data);

                var pagedResponse = PaginationHelper.CreatePagedReponse(userDtos, pagination, users.TotalRecords, _uriService, route);
                return pagedResponse;
            }
            catch (Exception ex)
            {
                return new PagedResponse<List<UserResponseDto>> { Succeeded = false, Errors = { ex.Message } };
            }
        }

        public async Task<Response<UserDto>> EditUserAsync(EditUserRequestDto request)
        {
            try
            {
                var validationResult = await _editUserValidator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return new Response<UserDto> { Succeeded = false, Errors = errors };
                }

                var existingUser = await _userRepositoriesAsync.GetByIdAsync(request.UserId);
                if (existingUser == null)
                {
                    return new Response<UserDto>("User not found");
                }

                if (existingUser.Role == RoleType.Admin && request.Role == RoleType.Staff)
                {
                    return new Response<UserDto> { Succeeded = false, Message = "Cannot edit admin's role." };
                }

                existingUser.DateOfBirth = request.DateOfBirth;
                existingUser.Gender = request.Gender;
                existingUser.JoinedDate = request.JoinedDate;
                existingUser.Role = request.Role;
                existingUser.LastModifiedOn = DateTime.Now;

                await _userRepositoriesAsync.UpdateAsync(existingUser);

                var updatedUserDto = _mapper.Map<UserDto>(existingUser);
                return new Response<UserDto> { Succeeded = true, Data = updatedUserDto };
            }
            catch (Exception ex)
            {
                return new Response<UserDto> { Succeeded = false, Errors = { ex.Message } };
            }
        }

        public async Task<Response<UserDto>> DisableUserAsync(Guid userId)
        {
            try
            {
                var user = await _userRepositoriesAsync.GetByIdAsync(userId);
                if (user == null)
                {
                    return new Response<UserDto> { Succeeded = false, Message = "User not found" };
                }
                //Check user have assignment

                var assignments = await _assignmentRepositoriesAsync.GetAssignmentsByUserId(user.Id);
                if (assignments.Any())
                {
                    return new Response<UserDto> { Succeeded = false, Message = "There are valid assignments belonging to this user. Please close all assignments before disabling user." };
                }
                user.LastModifiedOn = DateTime.Now;
                var disableUser = await _userRepositoriesAsync.DeleteAsync(user.Id);
                if (disableUser == null)
                {
                    return new Response<UserDto> { Succeeded = false, Message = "Disable user failed" };
                }

                var token = await _tokenRepositoriesAsync.FindByUserIdAsync(disableUser.Id);
                if (token != null)
                {
                    await _blackListTokensRepositoriesAsync.AddAsync(new BlackListToken
                    {
                        Token = token.Value,
                        CreatedOn = DateTime.Now
                    });
                }

                return new Response<UserDto> { Succeeded = true, Message = "Disable user successfully" };
            }
            catch (Exception ex)
            {
                return new Response<UserDto> { Succeeded = false, Errors = { ex.Message } };
            }
        }

        public async Task<Response<UserDto>> ResetPasswordAsync(Guid userId)
        {
            try
            {
                var existingUser = await _userRepositoriesAsync.GetByIdAsync(userId);
                if (existingUser == null)
                {
                    return new Response<UserDto>("User not found");
                }

                string newPassword = _userRepositoriesAsync.GeneratePassword(existingUser.Username, existingUser.DateOfBirth);
                existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, newPassword);

                await _userRepositoriesAsync.UpdateAsync(existingUser);

                var token = await _tokenRepositoriesAsync.FindByUserIdAsync(existingUser.Id);
                if (token != null)
                {
                    await _blackListTokensRepositoriesAsync.AddAsync(new BlackListToken
                    {
                        Token = token.Value,
                        CreatedOn = DateTime.Now
                    });
                }

                var updatedUserDto = _mapper.Map<UserDto>(existingUser);
                return new Response<UserDto> { Succeeded = true, Message = "Reset password successfully" };
            }
            catch (Exception ex)
            {
                return new Response<UserDto> { Succeeded = false, Errors = { ex.Message } };
            }
        }

        public async Task<Response<UserResponseDto>> GetUserByStaffCodeAsync(string staffCode)
        {
            try
            {
                var userWithStaffCode = UserSpecificationHelper.GetUserByStaffCode(staffCode);
                if (userWithStaffCode == null)
                {
                    return new Response<UserResponseDto>("User not found");
                }
                var user = await _userRepositoriesAsync.FirstOrDefaultAsync(userWithStaffCode);
                var userDto = _mapper.Map<UserResponseDto>(user);
                return new Response<UserResponseDto> { Succeeded = true, Data = userDto };
            }
            catch (Exception ex)
            {
                return new Response<UserResponseDto> { Succeeded = false, Errors = { ex.Message } };
            }
        }

        public async Task<Response<bool>> IsValidDisableUser(Guid userId)
        {
            try
            {
                var exsitedUser = await _userRepositoriesAsync.GetByIdAsync(userId);
                if (exsitedUser == null)
                {
                    return new Response<bool> { Succeeded = false, Message = "User not found" };
                }
                var assignments = await _assignmentRepositoriesAsync.GetAssignmentsByUserId(exsitedUser.Id);
                if (assignments.Any())
                {
                    return new Response<bool> { Succeeded = false, Message = "There are valid assignments belonging to this user. Please close all assignments before disabling user." };
                }
                return new Response<bool> { Succeeded = true };
            }
            catch (Exception ex)
            {
                return new Response<bool> { Succeeded = false, Errors = { ex.Message } };
            }
        }
    }
}