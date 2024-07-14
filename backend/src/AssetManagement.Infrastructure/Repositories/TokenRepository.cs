using AssetManagement.Application.Interfaces.Repositories;
using AssetManagement.Domain.Entites;
using AssetManagement.Infrastructure.Common;
using AssetManagement.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Infrastructure.Repositories
{
    public class TokenRepository : BaseRepositoryAsync<Token>, ITokenRepositoriesAsync
    {
        public TokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Token> FindByUserIdAsync(Guid userId)
        {
            return await _dbContext.Token.FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}