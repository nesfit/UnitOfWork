using System;
using System.Collections.Generic;
using BaseDataModel;

namespace Repository
{
    using System.Diagnostics.Contracts;

    using Repository.Contracts;

    /// <summary>
    /// Generic repository reader interface
    /// </summary>
    /// <typeparam name="T">Type must be a class and inherited from IDataModel interface</typeparam>
    [ContractClass(typeof(RepositoryReaderContract<>))]
    public interface IRepositoryReader<T> where T : class, IDataModel, new()
    {
        /// <summary>
        /// Gets item by id
        /// </summary>
        /// <param name="id">Id of item to get</param>
        /// <returns>Item if found or null otherwise</returns>
        T GetById(Guid id);

        /// <summary>
        /// Gets all items in persistence storage
        /// </summary>
        /// <returns>All items of type T in persistence storage</returns>
        IEnumerable<T> GetAll();
    }
}