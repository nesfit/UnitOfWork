using BaseDataModel;

namespace Repository
{
    /// <summary>
    /// Generic repository writer interface
    /// </summary>
    /// <typeparam name="T">Type must be inherited from IDataModel interface</typeparam>
    public interface IRepositoryWriter<T> where T : IDataModel
    {
        /// <summary>
        /// Inserts new item
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns>Newly created item</returns>
        T Insert(T item);

        /// <summary>
        /// Updates existing item
        /// </summary>
        /// <param name="item">Item to update</param>
        void Update(T item);
    }
}