using System.Data;
using UnitOfWork.BaseDataEntity;

namespace UnitOfWork.AdoDataMapperAbstract
{
    public interface IAdoDataMapper<T> where T : class, IDataEntity, new()
    {
        T Map(IDataReader reader);
    }
}