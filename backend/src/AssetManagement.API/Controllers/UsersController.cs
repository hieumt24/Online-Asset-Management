using AssetManagement.API.CustomActionFilters;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Users;
using AssetManagement.Application.Models.DTOs.Users.Requests;
using AssetManagement.Application.Models.Filters;
using AssetManagement.Application.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.API.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserServiceAsync _userService;
        private readonly IValidator<AddUserRequestDto> _validator;

        public UsersController(IUserServiceAsync userService, IValidator<AddUserRequestDto> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] AddUserRequestDto request)
        {
            var response = await _userService.AddUserAsync(request);
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("filter-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers([FromBody] UserFilter userFilter)
        {
            string route = Request.Path.Value;
            var response = await _userService.GetAllUsersAsync(userFilter.pagination, userFilter.search, userFilter.adminLocation, userFilter.roleType, userFilter.orderBy, userFilter.isDescending, route);
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{userId:guid}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var response = await _userService.GetUserByIdAsync(userId);
            if (!response.Succeeded)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut]
        [ValidateModel]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser([FromBody] EditUserRequestDto request)

        {
            if (request == null)
            {
                return BadRequest(new Response<UserDto> { Succeeded = false, Errors = { "Invalid request data" } });
            }

            var result = await _userService.EditUserAsync(request);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete]
        [Route("disable/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DisableUser(Guid id)
        {
            var response = await _userService.DisableUserAsync(id);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("resetPassword/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPassword(Guid userId)
        {
            var response = await _userService.ResetPasswordAsync(userId);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        [Route("staffCode/{staffCode}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetUserByStaffCode(string staffCode)
        {
            var response = await _userService.GetUserByStaffCodeAsync(staffCode);
            if (!response.Succeeded)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("isValidDisableUser/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IsValidDisableUser(Guid userId)
        {
            var response = await _userService.IsValidDisableUser(userId);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
