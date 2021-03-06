﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnitOfWork.BaseDataEntity;
using UnitOfWork.Repository;

namespace UnitOfWork.InMemoryRepository
{
    public class BaseRepository<TEntity> : IRepository<TEntity>, IRepositoryReader<TEntity>, IRepositoryReaderAsync<TEntity>, IRepositoryWriter<TEntity>,
        IRepositoryWriterAsync<TEntity> where TEntity : class, IDataEntity
    {
        private readonly List<TEntity> _data = new List<TEntity>();

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentException("IUnitOfWork cannot be null");
            if (!(unitOfWork is InMemoryUnitOfWork.InMemoryUnitOfWork))
                throw new ArgumentException("IUnitOfWork is not implemented by InMemoryUnitOfWork class");
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this._data;
        }

        public TEntity GetById(Guid id)
        {
            return this._data.FirstOrDefault(entity => entity.Id == id);
        }

        public TEntity GetSingleWhere(Expression<Func<TEntity, Boolean>> predicate)
        {
            return this._data.FirstOrDefault(predicate.Compile());
        }

        public IEnumerable<TEntity> GetAllWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return this._data.Where(predicate.Compile());
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Task.FromResult(this.GetAll()).ConfigureAwait(false);
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await Task.FromResult(this.GetById(id)).ConfigureAwait(false);
        }

        public async Task<TEntity> GetSingleWhereAsync(Expression<Func<TEntity, Boolean>> predicate)
        {
            return await Task.FromResult(this.GetSingleWhere(predicate)).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>> GetAllWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.FromResult(this.GetAllWhere(predicate)).ConfigureAwait(false);
        }

        public TEntity Delete(Guid id)
        {
            var item = this.GetById(id);
            if (item == null)
                throw new ArgumentException($"Item with {id} was not found, thus cannot be deleted.");

            this.Delete(item);
            return item;
        }

        public TEntity Delete(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException("TEntity item cannot be null.");

            var itemToBeRemoved = this._data.FirstOrDefault(i => i.Id == item.Id);
            if (itemToBeRemoved != null) this._data.Remove(itemToBeRemoved);

            return item;
        }

        public TEntity Insert(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException("TEntity item cannot be null.");

            this._data.Add(item);
            return item;
        }

        public IEnumerable<TEntity> InsertRange(IEnumerable<TEntity> items)
        {
            if (items == null)
                throw new ArgumentNullException("TEntity item cannot be null.");
            // Todo rewrite to appropriate bulk implementation
            var insertRange = items as TEntity[] ?? items.ToArray();
            this._data.AddRange(insertRange);

            return insertRange;
        }

        public void Update(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException("TEntity item cannot be null.");

            this.Delete(item);
            this.Insert(item);
        }

        public async Task<TEntity> DeleteAsync(Guid id)
        {
            var item = await this.GetByIdAsync(id).ConfigureAwait(false);
            if (item == null)
                throw new ArgumentException($"Item with {id} was not found, thus cannot be deleted.");

            await this.DeleteAsync(item).ConfigureAwait(false);
            return item;
        }

        public async Task<TEntity> DeleteAsync(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException("TEntity item cannot be null.");

            return await Task.FromResult(this.Delete(item)).ConfigureAwait(false);
        }

        public async Task<TEntity> InsertAsync(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException("TEntity item cannot be null.");

            await Task.FromResult(this.Insert(item)).ConfigureAwait(false);

            return item;
        }

        public async Task<IEnumerable<TEntity>> InsertRangeAsync(IEnumerable<TEntity> items)
        {
            if (items == null)
                throw new ArgumentNullException("TEntity item cannot be null.");

            var insertRangeAsync = items as TEntity[] ?? items.ToArray();
            await Task.FromResult(this.InsertRange(insertRangeAsync)).ConfigureAwait(false);

            return insertRangeAsync;
        }

        public async Task UpdateAsync(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException("TEntity item cannot be null.");

            await this.DeleteAsync(item).ConfigureAwait(false);
            await this.InsertAsync(item).ConfigureAwait(false);
        }
    }
}