using AssetManagement.Application.Common;
using AssetManagement.Domain.Entites;

namespace AssetManagement.Application.Interfaces.Repositories
{
    public interface IBlackListTokensRepositoriesAsync : IBaseRepositoryAsync<BlackListToken>
    {
        public Task<BlackListToken> GetByToken(string token);
    }
}