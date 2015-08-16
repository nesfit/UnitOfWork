namespace AdoRepository.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using AdoDataMapperAbstract;

    using AdoDbCommandProviderAbstract;

    using Fakes;

    using Moq;

    using Repository;

    using Xunit;

    public class BaseRepositoryTests:IDisposable
    {
        private readonly IAdoDbCommandProvider<Foo> _commandProvider;
        private readonly IAdoDataMapper<Foo> _dataMapper;

        private IDbTransaction _transaction;
        private readonly IDbConnection _connection;
        private readonly IRepositoryWriter<Foo> _repositoryWriter;
        private readonly IRepositoryReader<Foo> _repositoryReader;

        public BaseRepositoryTests()
        {
            _connection = new SqlConnection(@"Data Source=(localdb)\v11.0.;
                                                Initial Catalog=AdoUoWTestDb;
                                                Integrated Security=True");

            #region Mock
            
            var dataMapperStup = new Mock<IAdoDataMapper<Foo>>();
            dataMapperStup.Setup(f => f.Map(It.IsAny<IDataReader>()))
                .Returns(
                    (IDataReader reader) =>
                    new Foo
                        {
                            Id = Guid.Parse(reader["Id"].ToString()),
                            Name = reader["Name"].ToString()
                        });
            _dataMapper = dataMapperStup.Object;

            var commandProviderStud = new Mock<IAdoDbCommandProvider<Foo>>();
            commandProviderStud.Setup(f => f.SelectByIdCommand(It.IsAny<IDbConnection>(), It.IsAny<IDbTransaction>(), It.IsAny<Guid>()))
                .Returns(
                    (IDbConnection connection, IDbTransaction transaction, Guid id) =>
                        {
                            IDbCommand command = new SqlCommand();

                            command.Transaction = _transaction;
                            command.Connection = connection;
                            command.CommandType = CommandType.Text;
                            command.CommandText = "SELECT * FROM [Foos] WHERE Id = @Id";
                            command.Parameters.Add(
                                new SqlParameter { ParameterName = "@Id", DbType = DbType.Guid, Value = id });

                            return command;
                        });
            commandProviderStud.Setup(f => f.DeleteCommand(It.IsAny<IDbConnection>(), It.IsAny<IDbTransaction>(), It.IsAny<Foo>()))
                .Returns(
                    (IDbConnection connection, IDbTransaction transaction, Foo item) =>
                    {
                        IDbCommand command = new SqlCommand();

                        command.Connection = connection;
                        command.Transaction = _transaction;
                        command.CommandType = CommandType.Text;
                        command.CommandText = "DELETE FROM [Foos] WHERE Id = @Id";
                        command.Parameters.Add(
                            new SqlParameter { ParameterName = "@Id", DbType = DbType.Guid, Value = item.Id });
                        
                        return command;
                    });
            commandProviderStud.Setup(f => f.InsertCommand(It.IsAny<IDbConnection>(), It.IsAny<IDbTransaction>(), It.IsAny<Foo>()))
                .Returns(
                    (IDbConnection connection, IDbTransaction transaction, Foo item) =>
                    {
                        IDbCommand command = new SqlCommand();

                        command.Connection = connection;
                        command.Transaction = _transaction;
                        command.CommandType = CommandType.Text;
                        command.CommandText = "INSERT INTO [Foos] VALUES(@Id,@Name)";
                        command.Parameters.Add(
                            new SqlParameter { ParameterName = "@Id", DbType = DbType.Guid, Value = item.Id });
                        command.Parameters.Add(
                            new SqlParameter { ParameterName = "@Name", DbType = DbType.String, Value = item.Name });

                        return command;
                    });

            _commandProvider = commandProviderStud.Object;

            #endregion

            var repository = new BaseRepository<Foo>(_connection, _commandProvider, _dataMapper);
            _repositoryWriter = repository;
            _repositoryReader = repository;

            _connection.Open();
        }

        #region Test CRUD

        [Fact]
        public void GetsByIdReturnsNullWhenIdNotFound()
        {
            //Arrange
            Guid wrongId = Guid.NewGuid();

            //Act
            var foo = _repositoryReader.GetById(wrongId);

            //Assert
            Assert.Null(foo);
        }

        [Fact]
        public void GetsByIdReturnsItemCorrectlyWhenFound()
        {
            //Arrange
            var foo = new Foo { Id = Guid.NewGuid(), Name = "A" };
            _repositoryWriter.Insert(foo);

            //Act
            var fooDb = _repositoryReader.GetById(foo.Id);

            //Assert
            Assert.NotNull(fooDb);
            Assert.Equal(foo.Id, fooDb.Id);
            Assert.Equal("A", fooDb.Name);
        }
        
        [Fact]
        public void InsertsNewRecordCorrectly()
        {
            //Arrange
            var foo = new Foo {Id = Guid.NewGuid(), Name = "B"};

            //Act
            BeginTransaction();
            _repositoryWriter.Insert(foo);
            CommitTransaction();
            var fooFromDb = _repositoryReader.GetById(foo.Id);

            //Assert
            Assert.NotNull(fooFromDb);
            Assert.Equal(foo.Id, fooFromDb.Id);
            Assert.Equal("B", fooFromDb.Name);
        }

        [Fact]
        public void InsertsRangeCorrectly()
        {
            //Arrange
            BeginTransaction();
            var foo1 = new Foo { Id = Guid.NewGuid(), Name = "A" };
            var foo2 = new Foo { Id = Guid.NewGuid(), Name = "B" };

            //Act
            _repositoryWriter.InsertRange(new[] { foo1, foo2 });
            CommitTransaction();
            var foo1FromDb = _repositoryReader.GetById(foo1.Id);
            var foo2FromDb = _repositoryReader.GetById(foo2.Id);

            //Assert
            Assert.NotNull(foo1FromDb);
            Assert.Equal(foo1.Id, foo1FromDb.Id);
            Assert.Equal("A", foo1FromDb.Name);

            Assert.NotNull(foo2FromDb);
            Assert.Equal(foo2.Id, foo2FromDb.Id);
            Assert.Equal("B", foo2FromDb.Name);
        }

        [Fact]
        public void DeletesRecordByIdCorrectly()
        {
            //Arrange
            BeginTransaction();
            var foo = new Foo { Id = Guid.NewGuid(), Name = "B" };
            _repositoryWriter.Insert(foo);
            CommitTransaction();

            //Act
            BeginTransaction();
            _repositoryWriter.Delete(foo.Id);
            CommitTransaction();
            var fooFromDb = _repositoryReader.GetById(foo.Id);

            //Assert
            Assert.Null(fooFromDb);
        }

        [Fact]
        public void ThrowsExceptionWhenRecordToDeleteByIdNotFound()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => _repositoryWriter.Delete(Guid.NewGuid()));
        }

        [Fact]
        public void DeletesRecordCorrectly()
        {
            //Arrange
            BeginTransaction();
            var foo = new Foo { Id = Guid.NewGuid(), Name = "B" };
            _repositoryWriter.Insert(foo);
            CommitTransaction();

            //Act
            _repositoryWriter.Delete(foo);
            var fooFromDb = _repositoryReader.GetById(foo.Id);

            //Assert
            Assert.Null(fooFromDb);
        }

        #endregion Test CRUD

        #region Test PreConditions


        [Fact]
        public void PreConditionFailedWhenTryingToInsertNull()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    _repositoryWriter.Insert(null);
                });
        }

        [Fact]
        public void PreConditionFailedWhenTryingToInsertNullRange()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    _repositoryWriter.InsertRange(null);
                });
        }

        [Fact]
        public void PreConditionFailedWhenTryingToInsertEmptyRange()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentException>(
                () =>
                {
                    _repositoryWriter.InsertRange(new List<Foo>());
                });
        }

        [Fact]
        public void PreConditionFailedWhenTryingToUpdateNullItem()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    _repositoryWriter.Update(null);
                });
        }

        [Fact]
        public void PreConditionFailedWhenTryingToDeleteNullItem()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    _repositoryWriter.Delete(null);
                });
        }

        #endregion Test PreConditions

        #region Private Methods

        private void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }

        private void CommitTransaction()
        {
            _transaction?.Commit();
            _transaction = null;
        }

        #endregion Private Methods

        public void Dispose()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
