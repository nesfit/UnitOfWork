namespace EF6Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using BaseDataModel;

    using EF6UnitOfWork;

    using Repository;

    using UnitOfWork;

    using static System.Diagnostics.Contracts.Contract;

    public class BaseRepository<T> : 
        IRepositoryWriter<T>, IRepositoryReader<T>,
        IRepositoryWriterAsync<T>, IRepositoryReaderAsync<T>
        where T : class, IDataModel, new()
    {
        private readonly DbContext _context;
        private readonly IDbSet<T> _dbSet; 

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            Requires<ArgumentException>(
                unitOfWork is Ef6UnitOfWork,
                "IUnitOfWork is not implemented by Ef6UnitOfWork class");
            
            _context = ((Ef6UnitOfWork)unitOfWork).DbContext;
            _dbSet = _context.Set<T>();
        }

        public virtual T Insert(T item)
        {
            _context.Entry(item).State = EntityState.Added;
            return item;
        }

        public virtual IEnumerable<T> InsertRange(IEnumerable<T> items)
        {
            var insertRange = items as T[] ?? items.ToArray();
            foreach (var item in insertRange)
            {
                Insert(item);
            }
            return insertRange;
        }

        public virtual void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <exception cref="ArgumentException">Item with specified Id not found.</exception>
        public virtual T Delete(Guid id)
        {
            var item = GetById(id);

            if (item == null)
            {
                throw new ArgumentException($"Item of type [{typeof(T).FullName}] with Id = [{id}] not found");
            }

            return Delete(item);
        }

        public virtual T Delete(T item)
        {
            _context.Entry(item).State = EntityState.Deleted;
            return item;
        }

        public virtual T GetById(Guid id)
        {
            var item = _dbSet.FirstOrDefault(x => x.Id == id);
            return item;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet;
        }

        public virtual Task<T> InsertAsync(T item)
        {
            _context.Entry(item).State = EntityState.Added;
            return Task.FromResult(item);
        }

        public virtual async Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> items)
        {
            var insertRange = items as T[] ?? items.ToArray();
            foreach (var item in insertRange)
            {
                await InsertAsync(item);
            }
            return insertRange;
        }

        public virtual Task UpdateAsync(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        /// <exception cref="ArgumentException">Item with specified Id not found.</exception>
        public virtual async Task<T> DeleteAsync(Guid id)
        {
            var item = await GetByIdAsync(id);

            if (item == null)
            {
                throw new ArgumentException($"Item of type [{typeof(T).FullName}] with Id = [{id}] not found");
            }

            return await DeleteAsync(item);
        }

        public virtual Task<T> DeleteAsync(T item)
        {
            _context.Entry(item).State = EntityState.Deleted;
            return Task.FromResult(item);
        }

        public virtual Task<T> GetByIdAsync(Guid id)
        {
            return _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
