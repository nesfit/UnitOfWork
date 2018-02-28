namespace AdoRepository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using AdoDataMapperAbstract;

    using AdoDbCommandProviderAbstract;

    using BaseDataModel;

    using Repository;

    public class BaseRepository<T> : 
        IRepositoryWriter<T>, IRepositoryReader<T>
        where T : class, IDataModel, new()
    {
        private readonly IDbConnection _connection;
        private readonly IAdoDbCommandProvider<T> _commandProvider;
        private readonly IAdoDataMapper<T> _dataMapper; 

        public BaseRepository(
            IDbConnection connection, 
            IAdoDbCommandProvider<T> commandProvider, 
            IAdoDataMapper<T> dataMapper)
        {
            this._connection = connection;
            this._commandProvider = commandProvider;
            this._dataMapper = dataMapper;
        }

        public T Insert(T item)
        {
            T result = null;
            IDbCommand command = this._commandProvider.InsertCommand(this._connection, null, item);
            
            command.ExecuteNonQuery();
            result = item;

            return result;
        }

        public IEnumerable<T> InsertRange(IEnumerable<T> items)
        {
            var insertRange = items as T[] ?? items.ToArray();
            foreach (var item in insertRange)
            {
                this.Insert(item);
            }
            return insertRange;
        }

        public void Update(T item)
        {
            IDbCommand command = this._commandProvider.UpdateCommand(this._connection, null, item);

            command.ExecuteNonQuery();
        }

        /// <exception cref="ArgumentException">Item with specified Id not found.</exception>
        public T Delete(Guid id)
        {
            T item = this.GetById(id, false);

            if (item == null)
            {
                throw new ArgumentException($"Item of type [{typeof(T).FullName}] with Id = [{id}] not found");
            }

            return this.Delete(item);
        }

        public T Delete(T item)
        {
            T result = null;
            IDbCommand command = this._commandProvider.DeleteCommand(this._connection, null, item);

            command.ExecuteNonQuery();
            result = item;

            return result;
        }

        public T GetById(Guid id)
        {
            return this.GetById(id, true);
        }

        public IEnumerable<T> GetAll()
        {
            IList<T> result = new List<T>();
            IDbCommand command = this._commandProvider.SelectAllCommand(this._connection, null);
            try
            {
                this.OpenConnection();
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(this._dataMapper.Map(reader));
                    }
                }
            }
            finally
            {
                this._connection.Close();
            }
            return result;
        }

        #region Private Methods

        private void OpenConnection()
        {
            if (this._connection.State != ConnectionState.Open)
            {
                this._connection.Open();
            }
        }

        private T GetById(Guid id, Boolean closeConnection)
        {
            T result = null;
            IDbCommand command = this._commandProvider.SelectByIdCommand(this._connection, null, id);
            try
            {
                this.OpenConnection();
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = this._dataMapper.Map(reader);
                    }
                }
            }
            finally
            {
                if (closeConnection)
                {
                    this._connection.Close();
                }
            }
            return result;
        }

        #endregion Private Methods
    }
}
