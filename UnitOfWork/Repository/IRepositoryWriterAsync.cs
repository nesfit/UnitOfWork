using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitOfWork.BaseDataEntity;

namespace UnitOfWork.Repository
{
    /// <summary>
    /// Generic writer repository interface
    /// </summary>
    /// <typeparam name="T">Type must be a class and inherited from IDataEntity interface</typeparam>
    public interface IRepositoryWriterAsync<T> where T : class, IDataEntity
    {
        /// <summary>
        ///     Asynchronously deletes item
        /// </summary>
        /// <param name="id">Id of item to delete</param>
        /// <returns></returns>
        Task<T> DeleteAsync(Guid id);
        
        /// <summary>
        ///     Asynchronously deletes item
        /// </summary>
        /// <param name="item">Item to delete</param>
        /// <returns></returns>
        Task<T> DeleteAsync(T item);
        
        /// <summary>
        ///     Asynchronously  inserts new item
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns>Newly created item</returns>
        Task<T> InsertAsync(T item);
        
        /// <summary>
        ///     Asynchronously inserts range of items
        /// </summary>
        /// <param name="items">Items to add</param>
        /// <returns>Newly created items</returns>
        IEnumerable<T> InsertRange(IEnumerable<T> items);
        Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> items);
        
        /// <summary>
        ///     Asynchronously  updates existing item
        /// </summary>
        /// <param name="item">Item to update</param>
        Task UpdateAsync(T item);
    }
}