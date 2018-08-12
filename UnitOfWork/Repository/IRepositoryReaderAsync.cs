using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnitOfWork.BaseDataEntity;

namespace UnitOfWork.Repository
{
    /// <summary>
    ///     Generic asynchronous repository reader interface
    /// </summary>
    /// <typeparam name="T">Type must be a class and inherited from IDataEntity interface</typeparam>
    public interface IRepositoryReaderAsync<T> where T : class, IDataEntity
    {
        /// <summary>
        ///     Gets all items in persistence storage asyncronously
        /// </summary>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result contains all items of type T in persistence
        ///     storage
        /// </returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        ///     Gets item by id asyncronously
        /// </summary>
        /// <param name="id">Id of item to get</param>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result contains default ( T ) if source is empty;
        ///     otherwise, the found element in persistence storage
        /// </returns>
        Task<T> GetByIdAsync(Guid id);
        
        /// <summary>
        ///     Get all items filtered by a predicate
        /// </summary>
        /// <param name="predicate">Filter predicate</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains all items of type T in persistence storage which satisfy a predicate</returns>
        Task<IEnumerable<T>> GetAllWhereAsync(Expression<Func<T, bool>> predicate);
    }
}