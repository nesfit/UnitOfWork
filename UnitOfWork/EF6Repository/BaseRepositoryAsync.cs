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

    public class BaseRepositoryAsync<T> : 
        IRepositoryWriterAsync<T>, IRepositoryReaderAsync<T>
        where T : class, IDataModel, new()
    {
        private readonly DbContext _context;
        private readonly IDbSet<T> _dbSet;

        public BaseRepositoryAsync(IUnitOfWork unitOfWork)
        {
            Requires<ArgumentException>(
                unitOfWork is Ef6UnitOfWork,
                "IUnitOfWork is not implemented by Ef6UnitOfWork class");

            var unitOfWork1 = (Ef6UnitOfWork)unitOfWork;
            _context = unitOfWork1.DbContext;
            _dbSet = _context.Set<T>();
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

        public virtual async Task<T> DeleteAsync(Guid id)
        {
            var item = await GetByIdAsync(id);
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
