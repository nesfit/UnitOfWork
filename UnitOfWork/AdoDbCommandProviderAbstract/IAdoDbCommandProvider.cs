namespace AdoDbCommandProviderAbstract
{
    using System;
    using System.Data;

    using BaseDataModel;

    public interface IAdoDbCommandProvider<T> where T: class, IDataModel, new()
    {
        IDbCommand InsertCommand(IDbConnection connection, IDbTransaction transaction, T item);

        IDbCommand DeleteCommand(IDbConnection connection, IDbTransaction transaction, T item);

        IDbCommand UpdateCommand(IDbConnection connection, IDbTransaction transaction, T item);

        IDbCommand SelectByIdCommand(IDbConnection connection, IDbTransaction transaction, Guid id);

        IDbCommand SelectAllCommand(IDbConnection connection, IDbTransaction transaction);
    }
}