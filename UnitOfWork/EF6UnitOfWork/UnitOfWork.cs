namespace EF6UnitOfWork
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Threading;
    using System.Threading.Tasks;

    using global::UnitOfWork;

    /// <summary>
    /// Entity Framework IUnitOfWork Implementations
    /// </summary>
    public sealed class UnitOfWork: IUnitOfWork, IDisposable
    {
        private readonly DbContext _context;
        private readonly IsolationLevel _isolationLevel;
        private DbTransaction _transaction;

        public UnitOfWork(DbContext context, IsolationLevel isolationLevel)
        {
            _context = context;
            _isolationLevel = isolationLevel;
        }

        public void BeginTransaction()
        {
            if (_transaction == null)
            {
                if (_context.Database.Connection.State != ConnectionState.Open)
                {
                    _context.Database.Connection.Open();
                }
                _transaction = _context.Database.Connection.BeginTransaction(_isolationLevel);
            }
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                if (_context.Database.Connection.State != ConnectionState.Open)
                {
                    await _context.Database.Connection.OpenAsync();
                }
                _transaction = _context.Database.Connection.BeginTransaction(_isolationLevel);
            }
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            RollbackEntityStates();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #region Private Methods

        private void RollbackEntityStates()
        {
            throw new NotImplementedException();
        }

        #endregion Private Methods
    }
}