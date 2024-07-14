using AssetManagement.Application.Common;
using AssetManagement.Domain.Common.Models;
using AssetManagement.Domain.Common.Specifications;
using AssetManagement.Infrastructure.Contexts;
using AssetManagement.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AssetManagement.Infrastructure.Common
{
    public class BaseRepositoryAsync<T> : IBaseRepositoryAsync<T> where T : BaseEntity
    {
        public readonly ApplicationDbContext _dbContext;

        public BaseRepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException("An error occurred while adding the entity.", ex);
            }
            return entity;
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<T> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }
            entity.IsDeleted = true;
            await UpdateAsync(entity);
            return entity;
        }

        public async Task<T> DeletePermanentAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }

            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
            {
                return null;
            }
            return entity;
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<IList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).Property("RowVersion").OriginalValue = entity.RowVersion;
            _dbContext.Entry(entity).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var exceptionEntry = ex.Entries.Single();
                var clientValues = (T)exceptionEntry.Entity;
                var databaseEntry = exceptionEntry.GetDatabaseValues();

                if (databaseEntry == null)
                {
                    throw new NotFoundRepositoryException("The entity being updated has been deleted by another user.");
                }

                var databaseValues = (T)databaseEntry.ToObject();

                foreach (var property in exceptionEntry.OriginalValues.Properties)
                {
                    var databaseValue = databaseValues.GetType().GetProperty(property.Name).GetValue(databaseValues);
                    var clientValue = clientValues.GetType().GetProperty(property.Name).GetValue(clientValues);

                    if (databaseValue != null && !databaseValue.Equals(clientValue))
                    {
                        exceptionEntry.Property(property.Name).IsModified = true;
                        exceptionEntry.Property(property.Name).OriginalValue = databaseValue;
                    }
                }

                entity.RowVersion = databaseValues.RowVersion;

                throw new ConcurrencyException("The entity has been modified by another user. The current database values have been applied to your entity.", ex);
            }
            return entity;
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }
    }
}