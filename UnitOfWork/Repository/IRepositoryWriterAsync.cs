using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseDataModel;

namespace Repository
{
    public interface IRepositoryWriterAsync<T> where T : class, IDataModel, new()
    {
        /// <exception cref="ArgumentException">Item with specified Id not found.</exception>
        Task<T> DeleteAsync(Guid id);
        Task<T> DeleteAsync(T item);
        Task<T> InsertAsync(T item);
        Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> items);
        Task UpdateAsync(T item);
    }
}