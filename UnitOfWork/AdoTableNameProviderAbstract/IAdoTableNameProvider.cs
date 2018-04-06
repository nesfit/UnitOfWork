// pluskal

using System;
using BaseDataEntity;

namespace AdoTableNameProviderAbstract
{
    public interface IAdoTableNameProvider
    {
        String GetTableName<T>() where T : class, IDataEntity, new();
    }
}