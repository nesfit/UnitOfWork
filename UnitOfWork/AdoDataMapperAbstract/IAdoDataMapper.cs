using System.Data;
using BaseDataEntity;

namespace AdoDataMapperAbstract
{
    public interface IAdoDataMapper<T> where T : class, IDataEntity, new()
    {
        T Map(IDataReader reader);
    }
}