namespace Repository.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using BaseDataModel;

    [ContractClassFor(typeof(IRepositoryReader<>))]
    internal sealed class RepositoryReaderContract<T>: IRepositoryReader<T> where T : class, IDataModel, new()
    {
        T IRepositoryReader<T>.GetById(Guid id)
        {
            return default(T);
        }

        IEnumerable<T> IRepositoryReader<T>.GetAll()
        {
            return default(IEnumerable<T>);
        }
    }
}