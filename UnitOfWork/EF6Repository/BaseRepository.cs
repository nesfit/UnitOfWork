namespace EF6Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using BaseDataModel;
    using Repository;

    public class BaseRepository<T> : 
        IRepositoryWriter<T>, IRepositoryReader<T>
        where T : class, IDataModel
    {
        private readonly DbContext _context;
        private readonly IDbSet<T> _dbSet; 

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual T Insert(T item)
        {
            _context.Entry(item).State = EntityState.Added;
            return item;
        }

        public IEnumerable<T> InsertRange(IEnumerable<T> items)
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

        public T Delete(Guid id)
        {
            var item = GetById(id);
            return Delete(item);
        }

        public T Delete(T item)
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
            return _dbSet.ToList();
        }
    }
}
