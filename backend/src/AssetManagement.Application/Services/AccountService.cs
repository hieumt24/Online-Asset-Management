using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Models.DTOs.Users.Requests;
using AssetManagement.Application.Models.DTOs.Users.Responses;
using AssetManagement.Application.Wrappers;
using AssetManagement.Domain.Entites;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace AssetManagement.Application.Services
{
    public class AccountService : IAccountServicecs
    {
        private readonly IUserRepositoriesAsync _userRepositoriesAsync;
        private readonly ITokenService _tokenService;
        private readonly IValidator<ChangePasswordRequest> _changePasswordValidator;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        private readonly ITokenRepositoriesAsync _tokenRepositoriesAsync;

        public AccountService(IUserRepositoriesAsync userRepositoriesAsync, ITokenService tokenService, IValidator<ChangePasswordRequest> changePasswordValidator, ITokenRepositoriesAsync tokenRepositoriesAsync)
        {
            _changePasswordValidator = changePasswordValidator;
            _userRepositoriesAsync = userRepositoriesAsync;
            _tokenService = tokenService;
            _tokenRepositoriesAsync = tokenRepositoriesAsync;
        }

        public async Task<Response<string>> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var validationResult = await _changePasswordValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return new Response<string> { Succeeded = false, Errors = errors };
            }

            var user = await _userRepositoriesAsync.FindByUsernameAsync(request.Username);
            if (user == null) return new Response<string> { Succeeded = false, Message = "User doesn't exist." };
           
            if (user.IsFirstTimeLogin)
            {
                user.IsFirstTimeLogin = false;
            }
            else
            {
                if (user == null || !_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword).Equals(PasswordVerificationResult.Success))
                {
                    return new Response<string> { Succeeded = false, Message = "Current password is incorrect" };
                }

                // Check if the new password is the same as the current password
                if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.NewPassword).Equals(PasswordVerificationResult.Success))
                {
                    return new Response<string> { Succeeded = false, Message = "New password cannot be the same as the current password" };
                }
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            await _userRepositoriesAsync.UpdateUserAysnc(user);

            return new Response<string> { Succeeded = true, Message = "Password changed successfully" };
        }

        public async Task<Response<AuthenticationResponse>> LoginAsync(AuthenticationRequest request)
        {
            var user = await _userRepositoriesAsync.FindByUsernameAsync(request.Username);
            if (user == null)
            {
                return new Response<AuthenticationResponse> { Succeeded = false, Message = "Invalid username or password" };
            }

            if (user == null)
            {
                return new Response<AuthenticationResponse> { Succeeded = false, Message = "Invalid username or password" };
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                return new Response<AuthenticationResponse> { Succeeded = false, Message = "Invalid username or password" };
            }

            var role = await _userRepositoriesAsync.GetRoleAsync(user.Id);
            var token = _tokenService.GenerateJwtToken(user, role);
            var response = new AuthenticationResponse
            {
                Username = user.Username,
                Role = role.ToString(),
                Token = token
            };
            var exsitingToken = await _tokenRepositoriesAsync.FindByUserIdAsync(user.Id);
            if (exsitingToken != null)
            {
                exsitingToken.Value = token;
                exsitingToken.CreatedOn = DateTime.Now;
                await _tokenRepositoriesAsync.UpdateAsync(exsitingToken);
            }
            else
            {
                await _tokenRepositoriesAsync.AddAsync(new Token
                {
                    UserId = user.Id,
                    Value = token
                });
            }
            
            if (user.IsFirstTimeLogin)
            {
                return new Response<AuthenticationResponse>
                {
                    Succeeded = true,
                    Message = "You need to change your password before login",
                    Data = new AuthenticationResponse
                    {
                        Username = user.Username,
                        Role = user.Role.ToString(),
                        IsFirstTimeLogin = user.IsFirstTimeLogin,
                        Token = token
                    }
                };
            }
            return new Response<AuthenticationResponse>
            {
                Succeeded = true,
                Data = response
            };
        }
    }
}