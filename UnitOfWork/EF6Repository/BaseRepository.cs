namespace EF6Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using BaseDataModel;

    using EF6UnitOfWork;

    using Repository;

    using UnitOfWork;

    using static System.Diagnostics.Contracts.Contract;

    public class BaseRepository<T> : 
        IRepositoryWriter<T>, IRepositoryReader<T>
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

        public virtual T Delete(Guid id)
        {
            var item = GetById(id);
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
            return _dbSet.ToList();
        }
    }
}
