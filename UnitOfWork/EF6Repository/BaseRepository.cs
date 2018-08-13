using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnitOfWork.BaseDataEntity;
using UnitOfWork.EF6UnitOfWork;
using UnitOfWork.Repository;

namespace UnitOfWork.EF6Repository
{
    public class BaseRepository<T> :
        IRepositoryWriter<T>, IRepositoryReader<T>,
        IRepositoryWriterAsync<T>, IRepositoryReaderAsync<T>
        where T : class, IDataEntity
    {
        private readonly DbContext _context;
        private readonly IDbSet<T> _dbSet;

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentException("IUnitOfWork cannot be null");
            if (!(unitOfWork is EF6UnitOfWork.EF6UnitOfWork))
                throw new ArgumentException("IUnitOfWork is not implemented by Ef6UnitOfWork class");


            this._context = ((EF6UnitOfWork.EF6UnitOfWork) unitOfWork).DbContext;
            this._dbSet = this._context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this._dbSet;
        }

        public virtual T GetById(Guid id)
        {
            var item = this._dbSet.FirstOrDefault(x => x.Id == id);
            return item;
        }

        public virtual IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> predicate)
        {
            return this._dbSet.Where(predicate);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await this._dbSet.ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await this._dbSet.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<T>> GetAllWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.Run(() => this.GetAllWhere(predicate)).ConfigureAwait(false);
        }

        /// <exception cref="ArgumentException">Item with specified Id not found.</exception>
        public virtual T Delete(Guid id)
        {
            var item = this.GetById(id);

            if (item == null) throw new ArgumentException($"Item of type [{typeof(T).FullName}] with Id = [{id}] not found");

            return this.Delete(item);
        }

        public virtual T Delete(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "T item cannot be null.");

            this._context.Entry(item).State = EntityState.Deleted;
            return item;
        }

        public virtual T Insert(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "T item cannot be null.");

            this._context.Entry(item).State = EntityState.Added;
            return item;
        }

        public virtual IEnumerable<T> InsertRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), "T item cannot be null.");

            var insertRange = items as T[] ?? items.ToArray();
            foreach (var item in insertRange) this.Insert(item);
            return insertRange;
        }

        public virtual void Update(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "T item cannot be null.");

            this._context.Entry(item).State = EntityState.Modified;
        }

        /// <exception cref="ArgumentException">Item with specified Id not found.</exception>
        public virtual async Task<T> DeleteAsync(Guid id)
        {
            var item = await this.GetByIdAsync(id).ConfigureAwait(false);

            if (item == null) throw new ArgumentException($"Item of type [{typeof(T).FullName}] with Id = [{id}] not found");

            return await this.DeleteAsync(item).ConfigureAwait(false);
        }

        public virtual async Task<T> DeleteAsync(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "T item cannot be null.");

            this._context.Entry(item).State = EntityState.Deleted;
            return await Task.FromResult(item).ConfigureAwait(false);
        }

        public virtual async Task<T> InsertAsync(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "T item cannot be null.");

            this._context.Entry(item).State = EntityState.Added;
            return await Task.FromResult(item).ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items), "T item cannot be null.");

            var insertRange = items as T[] ?? items.ToArray();
            foreach (var item in insertRange) await this.InsertAsync(item).ConfigureAwait(false);
            return insertRange;
        }

        public virtual async Task UpdateAsync(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "T item cannot be null.");

            this._context.Entry(item).State = EntityState.Modified;
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}