using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ArangoDB.Client;
using UnitOfWork.BaseDataEntity;
using UnitOfWork.Repository;

namespace UnitOfWork.ArangoDBRepository
{
    public class BaseRepository<TEntity> : IRepositoryReader<TEntity>, IRepositoryReaderAsync<TEntity>,
        IRepositoryWriter<TEntity>, IRepositoryWriterAsync<TEntity> where TEntity : class, IDataEntity
    {
        public BaseRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentException("IUnitOfWork cannot be null");
            if (!(unitOfWork is ArangoDBUnitOfWork.ArangoDBUnitOfWork))
                throw new ArgumentException("IUnitOfWork is not implemented by CassandraUnitOfWork class");

            this.Database = ((ArangoDBUnitOfWork.ArangoDBUnitOfWork) unitOfWork).Database;
        }


        public IArangoDatabase Database { get; set; }

        public IEnumerable<TEntity> GetAll()
        {
            return this.Database.Query<TEntity>().AsEnumerable();
        }

        public TEntity GetById(Guid id)
        {
            return this.GetSingleWhere(entity => entity.Id == id);
        }

        public TEntity GetSingleWhere(Expression<Func<TEntity, Boolean>> predicate)
        {
            return this.Database.Query<TEntity>().FirstOrDefault(predicate);
        }

        public IEnumerable<TEntity> GetAllWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Database.Query<TEntity>().Where(predicate).AsEnumerable();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await this.Database.Query<TEntity>().ToListAsync().ConfigureAwait(false);
        }

        public Task<TEntity> GetByIdAsync(Guid id)
        {
            return this.GetSingleWhereAsync(entity => entity.Id == id);
        }

        public Task<TEntity> GetSingleWhereAsync(Expression<Func<TEntity, Boolean>> predicate)
        {
            return this.Database.Query<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(() => this.GetAllWhere(predicate));
        }

        public TEntity Delete(Guid id)
        {
            var item = this.GetById(id);
            if (item == null)
                throw new ArgumentException($"Item with {id} was not found, thus cannot be deleted.");

            this.Database.RemoveById<TEntity>(id.ToString());
            return item;
        }

        public TEntity Delete(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException("TEntity item cannot be null.");

            this.Database.Remove<TEntity>(item);
            return item;
        }

        public TEntity Insert(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException("TEntity item cannot be null.");

            this.Database.Insert<TEntity>(item);
            return item;
        }

        public IEnumerable<TEntity> InsertRange(IEnumerable<TEntity> items)
        {
            if (items == null)
                throw new ArgumentNullException("TEntity item cannot be null.");

            var insertRange = items.ToList();
            this.Database.InsertMultiple<TEntity>(insertRange);

            return insertRange;
        }

        public void Update(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException("TEntity item cannot be null.");

            this.Database.UpdateById<TEntity>(item.Id.ToString(), item);
        }

        public async Task<TEntity> DeleteAsync(Guid id)
        {
            var item = await this.GetByIdAsync(id);
            if (item == null)
                throw new ArgumentException($"Item with {id} was not found, thus cannot be deleted.");

            await this.Database.RemoveByIdAsync<TEntity>(id.ToString()).ConfigureAwait(false);

            return item;
        }

        public async Task<TEntity> DeleteAsync(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException("TEntity item cannot be null.");

            await this.Database.RemoveAsync<TEntity>(item).ConfigureAwait(false);
            return item;
        }

        public async Task<TEntity> InsertAsync(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException("TEntity item cannot be null.");

            await this.Database.InsertAsync<TEntity>(item).ConfigureAwait(false);
            return item;
        }

        public async Task<IEnumerable<TEntity>> InsertRangeAsync(IEnumerable<TEntity> items)
        {
            if (items == null)
                throw new ArgumentNullException("TEntity item cannot be null.");

            var insertRangeAsync = items.ToList();
            await this.Database.InsertMultipleAsync<TEntity>(insertRangeAsync).ConfigureAwait(false);

            return insertRangeAsync;
        }

        public async Task UpdateAsync(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException("TEntity item cannot be null.");

            await this.Database.UpdateByIdAsync<TEntity>(item.Id.ToString(), item).ConfigureAwait(false);
        }
    }
}