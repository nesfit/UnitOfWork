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
      return new TEntity[0];
    }

    public TEntity GetById(Guid id)
    {
      return null;
    }

    public IEnumerable<TEntity> GetAllWhere(Expression<Func<TEntity, Boolean>> predicate)
    {
      return new TEntity[0];
    }


    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
      return await Task.FromResult(new TEntity[0]);
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
      return await Task.FromResult<TEntity>(null);
    }

    public async Task<IEnumerable<TEntity>> GetAllWhereAsync(Expression<Func<TEntity, Boolean>> predicate)
    {
      return await Task.FromResult(new TEntity[0]);
    }

    public TEntity Delete(Guid id)
    {
      return null;
    }

    public TEntity Delete(TEntity item)
    {
      return item;
    }

    public TEntity Insert(TEntity item)
    {
      return item;
    }

    public IEnumerable<TEntity> InsertRange(IEnumerable<TEntity> items)
    {
      return items;
    }

    public void Update(TEntity item)
    {
    }

    public async Task<TEntity> DeleteAsync(Guid id)
    {
      return await Task.FromResult<TEntity>(null);
    }

    public async Task<TEntity> DeleteAsync(TEntity item)
    {
      return await Task.FromResult(item);
    }

    public async Task<TEntity> InsertAsync(TEntity item)
    {
      return await Task.FromResult(item).ConfigureAwait(false);
    }

    public async Task<IEnumerable<TEntity>> InsertRangeAsync(IEnumerable<TEntity> items)
    {
      return await Task.FromResult(items).ConfigureAwait(false);
    }

    public async Task UpdateAsync(TEntity item)
    {
      await Task.CompletedTask;
    }
  }
}