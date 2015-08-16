namespace AdoTableNameProviderAbstract
{
    using BaseDataModel;

    public interface IAdoTableNameProvider
    {
        string GetTableName<T>() where T : class, IDataModel, new();
    }
}