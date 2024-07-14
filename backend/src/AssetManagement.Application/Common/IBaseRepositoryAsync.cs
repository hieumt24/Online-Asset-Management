using AssetManagement.Domain.Common.Specifications;

namespace AssetManagement.Application.Common
{
    public interface IBaseRepositoryAsync<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);

        Task<List<T>> ListAllAsync();

        Task<IList<T>> ListAsync(ISpecification<T> spec);

        Task<T> FirstOrDefaultAsync(ISpecification<T> spec);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<T> DeleteAsync(Guid id);

        Task<T> DeletePermanentAsync(Guid id);

        Task<int> CountAsync(ISpecification<T> spec);
    }
}