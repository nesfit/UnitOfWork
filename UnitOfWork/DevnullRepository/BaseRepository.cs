using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnitOfWork.BaseDataEntity;
using UnitOfWork.Repository;

namespace UnitOfWork.DevnullRepository
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IDataEntity
    {
        public IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public TEntity GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAllWhere(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public TEntity Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public TEntity Delete(TEntity item)
        {
            throw new NotImplementedException();
        }

        public TEntity Insert(TEntity item) => item;

        public IEnumerable<TEntity> InsertRange(IEnumerable<TEntity> items) => items;

        public void Update(TEntity item)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> DeleteAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> InsertAsync(TEntity item) => await Task.FromResult(item).ConfigureAwait(false);

        public async Task<IEnumerable<TEntity>> InsertRangeAsync(IEnumerable<TEntity> items) => await Task.FromResult(items).ConfigureAwait(false);

        public Task UpdateAsync(TEntity item)
        {
            throw new NotImplementedException();
        }
    }
}