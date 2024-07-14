using AssetManagement.Application.Common;
using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Infrastructure.Common;
using AssetManagement.Infrastructure.Contexts;
using AssetManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AssetManagement.Infrastructure
{
    public class ServiceRegistration
    {
        public static void Configure(IServiceCollection service, IConfiguration configuration)
        {
            service.AddScoped(typeof(IBaseRepositoryAsync<>), typeof(BaseRepositoryAsync<>));
            service.AddScoped<IUserRepositoriesAsync, UserRepository>();
            service.AddScoped<IAssetRepositoriesAsync, AssetRepository>();
            service.AddScoped<ICategoryRepositoriesAsync, CategoryRepository>();
            service.AddScoped<IAssignmentRepositoriesAsync, AssignmentRepository>();
            service.AddScoped<IReturnRequestRepositoriesAsync, ReturnRequestRepository>();
            service.AddScoped<ITokenRepositoriesAsync, TokenRepository>();
            service.AddScoped<IBlackListTokensRepositoriesAsync, BlackListTokensRepositories>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            service.AddDbContext<ApplicationDbContext>(options =>
                           options.UseSqlServer(connectionString));
        }
    }
}