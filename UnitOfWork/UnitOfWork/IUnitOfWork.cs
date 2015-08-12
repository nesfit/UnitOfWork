using System;
using System.Threading.Tasks;
using BaseDataModel;
using Repository;

namespace UnitOfWork
{
    using System.Threading;

    /// <summary>
    /// Unit Of Work interface
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Begins transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commits transaction
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback transaction
        /// </summary>
        void Rollback();

        /// <summary>
        /// Saves changes into the persistence storage
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Saves changes into the persistence storage asynchronously
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task SaveChangesAsync();

        /// <summary>
        /// Saves changes into the persistence storage asynchronously
        /// </summary>
        /// <param name="cancellationToken">Token to cancel operation</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}