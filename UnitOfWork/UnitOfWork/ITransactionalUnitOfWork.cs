namespace UnitOfWork
{
    /// <summary>
    ///     Transactional Unit Of Work interface
    /// </summary>
    public interface ITransactionalUnitOfWork : IUnitOfWork
    {
        /// <summary>
        ///     Begins transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        ///     Commits transaction
        /// </summary>
        void Commit();

        /// <summary>
        ///     Rollback transaction
        /// </summary>
        void Rollback();
    }
}