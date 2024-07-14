using AssetManagement.Application.Common;
using AssetManagement.Domain.Entites;

namespace AssetManagement.Application.Interfaces.Repositories
{
    public interface ITokenRepositoriesAsync : IBaseRepositoryAsync<Token>
    {
        Task<Token> FindByUserIdAsync(Guid userId);
    }
}