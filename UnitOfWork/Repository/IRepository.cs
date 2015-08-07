using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseDataModel;

namespace Repository
{
    /// <summary>
    /// Basic interface for CRUD repository
    /// </summary>
    /// <typeparam name="T">Type must be inherited from IDataModel interface</typeparam>
    public interface IRepository<T> where T: IDataModel
    {
        /// <summary>
        /// Inserts new item
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns>Newly created item</returns>
        T Insert(T item);

        /// <summary>
        /// Updates existing item
        /// </summary>
        /// <param name="item">Item to update</param>
        void Update(T item);

        /// <summary>
        /// Gets item by id
        /// </summary>
        /// <param name="id">Id of item to get</param>
        /// <returns>Item if found or null otherwise</returns>
        T GetById(Guid id);

        /// <summary>
        /// Gets item by id asyncronously
        /// </summary>
        /// <param name="id">Id of item to get</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains default ( T ) if source is empty; otherwise, the found element in persistence storage.</returns>
        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all items in persistence storage
        /// </summary>
        /// <returns>All items in persistence storage</returns>
        IEnumerable<T> GetAll();
    }
}