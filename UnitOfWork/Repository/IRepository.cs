using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseDataModel;
using Specification;

namespace Repository
{
    /// <summary>
    /// Generic CRUD repository interface
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
        /// <returns>A task that represents the asynchronous operation. The task result contains default ( T ) if source is empty; otherwise, the found element in persistence storage</returns>
        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all items in persistence storage
        /// </summary>
        /// <returns>All items of type T in persistence storage</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Gets all items in persistence storage asyncronously
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains all items of type T in persistence storage</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Filters items by specification
        /// </summary>
        /// <param name="specification">Specification for filtering</param>
        /// <returns>All items satisfied by specification of type T in persistence storage</returns>
        IEnumerable<T> Filter(ISpecification<T> specification);

        /// <summary>
        /// Filters items by specification asyncronously
        /// </summary>
        /// <param name="specification">Specification for filtering</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains all items satisfied by specification of type T in persistence storage</returns>
        Task<IEnumerable<T>> FilterAsync(ISpecification<T> specification);
    }
}