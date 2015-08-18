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
            _connection = connection;
            _commandProvider = commandProvider;
            _dataMapper = dataMapper;
        }

        public T Insert(T item)
        {
            T result = null;
            IDbCommand command = _commandProvider.InsertCommand(_connection, null, item);
            
            command.ExecuteNonQuery();
            result = item;

            return result;
        }

        public IEnumerable<T> InsertRange(IEnumerable<T> items)
        {
            var insertRange = items as T[] ?? items.ToArray();
            foreach (var item in insertRange)
            {
                Insert(item);
            }
            return insertRange;
        }

        public void Update(T item)
        {
            IDbCommand command = _commandProvider.UpdateCommand(_connection, null, item);

            command.ExecuteNonQuery();
        }

        /// <exception cref="ArgumentException">Item with specified Id not found.</exception>
        public T Delete(Guid id)
        {
            T item = GetById(id, false);

            if (item == null)
            {
                throw new ArgumentException($"Item of type [{typeof(T).FullName}] with Id = [{id}] not found");
            }

            return Delete(item);
        }

        public T Delete(T item)
        {
            T result = null;
            IDbCommand command = _commandProvider.DeleteCommand(_connection, null, item);

            command.ExecuteNonQuery();
            result = item;

            return result;
        }

        public T GetById(Guid id)
        {
            return GetById(id, true);
        }

        public IEnumerable<T> GetAll()
        {
            IList<T> result = new List<T>();
            IDbCommand command = _commandProvider.SelectAllCommand(_connection, null);
            try
            {
                OpenConnection();
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(_dataMapper.Map(reader));
                    }
                }
            }
            finally
            {
                _connection.Close();
            }
            return result;
        }

        #region Private Methods

        private void OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        private T GetById(Guid id, Boolean closeConnection)
        {
            T result = null;
            IDbCommand command = _commandProvider.SelectByIdCommand(_connection, null, id);
            try
            {
                OpenConnection();
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = _dataMapper.Map(reader);
                    }
                }
            }
            finally
            {
                if (closeConnection)
                {
                    _connection.Close();
                }
            }
            return result;
        }

        #endregion Private Methods
    }
}
