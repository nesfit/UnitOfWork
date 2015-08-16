namespace AdoDataMapperAbstract
{
    using System.Data;

    using BaseDataModel;

    public interface IAdoDataMapper<T> where T: class, IDataModel, new()
    {
        T Map(IDataReader reader);
    }
}