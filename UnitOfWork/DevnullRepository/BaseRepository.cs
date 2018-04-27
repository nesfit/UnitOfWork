using System;
using System.Collections.Generic;
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

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(Guid id)
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

        public Task<TEntity> InsertAsync(TEntity item) => Task.FromResult(item);

        public Task<IEnumerable<TEntity>> InsertRangeAsync(IEnumerable<TEntity> items) => Task.FromResult(items);

        public Task UpdateAsync(TEntity item)
        {
            throw new NotImplementedException();
        }
    }
}