using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Domain.Entites;
using AssetManagement.Infrastructure.Common;
using AssetManagement.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Infrastructure.Repositories
{
    public class BlackListTokensRepositories : BaseRepositoryAsync<BlackListToken>, IBlackListTokensRepositoriesAsync
    {
        public BlackListTokensRepositories(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<BlackListToken> GetByToken(string token)
        {
            return await _dbContext.BlackListTokens.FirstOrDefaultAsync(x => x.Token == token);
        }
    }
}