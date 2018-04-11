using System;
using UnitOfWork.BaseDataEntity;

namespace UnitOfWork.AdoTableNameProviderAbstract
{
    public interface IAdoTableNameProvider
    {
        String GetTableName<T>() where T : class, IDataEntity, new();
    }
}