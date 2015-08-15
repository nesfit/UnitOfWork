namespace EF6Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using BaseDataModel;
    using Repository;

    public class BaseRepositoryAsync<T> : 
        IRepositoryWriterAsync<T>, IRepositoryReaderAsync<T>
        where T : class, IDataModel
    {
        private readonly DbContext _context;
        private readonly IDbSet<T> _dbSet; 

        public BaseRepositoryAsync(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
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
