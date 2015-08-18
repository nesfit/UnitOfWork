namespace EF6UnitOfWork
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Threading;
    using System.Threading.Tasks;

    using global::UnitOfWork;

    /// <summary>
    /// Entity Framework IUnitOfWork Implementations
    /// </summary>
    public class Ef6UnitOfWork: IUnitOfWork, IDisposable
    {
        private readonly DbContext _context;
        private readonly IsolationLevel _isolationLevel;
        private DbContextTransaction _transaction;

        public Ef6UnitOfWork(DbContext context, IsolationLevel isolationLevel)
        {
            _context = context;
            _isolationLevel = isolationLevel;
        }

        #region Interfaces Members

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction(_isolationLevel);
        }
        
        public void Commit()
        {
            _transaction?.Commit();
            CleanUpTransaction();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            CleanUpTransaction();
        }

        /// <exception cref="DbUpdateException">An error occurred sending updates to the database.</exception>
        /// <exception cref="DbEntityValidationException">
        ///             The save was aborted because validation of entity property values failed.
        ///             </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        ///             A database command did not affect the expected number of rows. This usually indicates an optimistic 
        ///             concurrency violation; that is, a row has been changed in the database since it was queried.
        ///             </exception>
        /// <exception cref="NotSupportedException">
        ///             An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///             on the same context instance.</exception>
        /// <exception cref="ObjectDisposedException">The context or connection have been disposed.</exception>
        /// <exception cref="InvalidOperationException">
        ///             Some error occurred attempting to process entities in the context either before or after sending commands
        ///             to the database.
        ///             </exception>
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        /// <exception cref="DbUpdateException">An error occurred sending updates to the database.</exception>
        /// <exception cref="DbUpdateConcurrencyException">
        ///             A database command did not affect the expected number of rows. This usually indicates an optimistic 
        ///             concurrency violation; that is, a row has been changed in the database since it was queried.
        ///             </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        ///             A database command did not affect the expected number of rows. This usually indicates an optimistic 
        ///             concurrency violation; that is, a row has been changed in the database since it was queried.
        ///             </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        ///             A database command did not affect the expected number of rows. This usually indicates an optimistic 
        ///             concurrency violation; that is, a row has been changed in the database since it was queried.
        ///             </exception>
        /// <exception cref="DbEntityValidationException">
        ///             The save was aborted because validation of entity property values failed.
        ///             </exception>
        /// <exception cref="NotSupportedException">
        ///             An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        ///             on the same context instance.</exception>
        /// <exception cref="ObjectDisposedException">The context or connection have been disposed.</exception>
        /// <exception cref="InvalidOperationException">
        ///             Some error occurred attempting to process entities in the context either before or after sending commands
        ///             to the database.
        ///             </exception>
        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Interfaces Members

        #region Class Members

        public DbContext DbContext => _context;

        #endregion Class Members

        #region Private Members

        
        private void CleanUpTransaction()
        {
            _transaction = null;
        }
        private void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }
            // get rid of unmanaged resources
        }
        ~Ef6UnitOfWork()
        {
            Dispose(false);
        }

        #endregion Private Members
    }
}