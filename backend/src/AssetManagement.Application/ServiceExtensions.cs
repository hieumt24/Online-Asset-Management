using AssetManagement.Application.Interfaces.Services;
using AssetManagement.Application.Mappings;
using AssetManagement.Application.Models.DTOs.Assets.Requests;
using AssetManagement.Application.Models.DTOs.Assignments.Reques;
using AssetManagement.Application.Models.DTOs.Assignments.Request;
using AssetManagement.Application.Models.DTOs.Category.Requests;
using AssetManagement.Application.Models.DTOs.ReturnRequests.Request;
using AssetManagement.Application.Models.DTOs.Users.Requests;
using AssetManagement.Application.Services;
using AssetManagement.Application.Validations.Asset;
using AssetManagement.Application.Validations.Assignment;
using AssetManagement.Application.Validations.Category;
using AssetManagement.Application.Validations.ReturnRequest;
using AssetManagement.Application.Validations.User;
using AssetManagement.Domain.Common.Settings;
using AssetManagement.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AssetManagement.Application
{
    public class ServiceExtensions
    {
        public static void ConfigureServices(IServiceCollection service, IConfiguration configuration)
        {
            service.AddScoped<IAssetServiceAsync, AssetService>();
            service.AddScoped<ICategoryServiceAsync, CategoryService>();

            service.AddScoped<IValidator<AddCategoryRequestDto>, AddCategoryRequestValidation>();
            service.AddScoped<IValidator<AddAssetRequestDto>, AddAssetRequestValidation>();
            service.AddScoped<IValidator<UpdateCategoryRequestDto>, UpdateCategoryRequestValidation>();
            service.AddScoped<IValidator<EditAssetRequestDto>, EditAssetRequestValidation>();

            //assignment
            service.AddScoped<IAssignmentServicesAsync, AssignmentServiceAsync>();
            service.AddScoped<IValidator<AddAssignmentRequestDto>, AddAssignmentRequestValidation>();
            service.AddScoped<IValidator<EditAssignmentRequestDto>, EditAssignmentRequestValidation>();

            //return request
            service.AddScoped<IReturnRequestServiceAsync, ReturnRequestService>();
            service.AddScoped<IValidator<AddReturnRequestDto>, AddReturnRequestValidator>();

            service.AddAutoMapper(typeof(GeneralProfile));
            service.AddScoped<IValidator<AddUserRequestDto>, AddUserRequestValidation>();
            service.AddScoped<IValidator<EditUserRequestDto>, EditUserRequestValidation>();
            service.AddScoped<IValidator<ChangePasswordRequest>, PasswordValidation>();
            service.AddScoped<ITokenService, TokenService>();
            service.AddScoped<IAccountServicecs, AccountService>();
            service.AddScoped<IUserServiceAsync, UserServiceAsync>();
            service.AddScoped<IReportServices, ReportService>();
            service.AddSingleton<IUriService>(options =>
            {
                var accessor = options.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });

            var jwtSettings = service.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            service.AddSingleton(jwtSettings);
            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };
                });

            service.AddAuthorization(options =>
            {
                options.AddPolicy($"{RoleType.Admin}", policy => policy.RequireRole("Admin"));
                options.AddPolicy($"{RoleType.Staff}", policy => policy.RequireRole("Staff"));
            });
        }
    }
}