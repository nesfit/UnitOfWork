// pluskal

using System;
using System.Threading.Tasks;

namespace UnitOfWork
{
    /// <summary>
    ///     Unit Of Work interface
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        ///     Saves changes into the persistence storage
        /// </summary>
        void SaveChanges();

        /// <summary>
        ///     Saves changes into the persistence storage asynchronously
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task SaveChangesAsync();
    }
}