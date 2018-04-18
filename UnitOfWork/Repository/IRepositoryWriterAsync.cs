using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitOfWork.BaseDataEntity;

namespace UnitOfWork.Repository
{
    // <summary>
    /// Generic writer repository interface
    /// </summary>
    /// <typeparam name="T">Type must be a class and inherited from IDataEntity interface</typeparam>
    public interface IRepositoryWriterAsync<T> where T : class, IDataEntity
    {
        /// <exception cref="ArgumentException">Item with specified Id not found.</exception>
        Task<T> DeleteAsync(Guid id);

        Task<T> DeleteAsync(T item);
        Task<T> InsertAsync(T item);
        Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> items);
        Task UpdateAsync(T item);
    }
}