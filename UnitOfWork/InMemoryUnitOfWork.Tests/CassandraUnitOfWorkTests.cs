using System;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using Fakes;
using InMemoryRepository;
using Repository;
using UnitOfWork;
using Xunit;

namespace InMemoryUnitOfWork.Tests
{
    public class CassandraUnitOfWorkTests
    {
        private readonly IRepositoryReader<Foo> _repositoryReader;
        private readonly IRepositoryWriter<Foo> _repositoryWriter;
        private readonly IUnitOfWork _unitOfWork;

        public CassandraUnitOfWorkTests()
        {
            this._unitOfWork = new InMemoryUnitOfWork();

            var repository = new BaseRepository<Foo>(this._unitOfWork);
            this._repositoryWriter = repository;
            this._repositoryReader = repository;
        }

        [Fact(Skip = "NotImplemented")]
        public void CommitsTransactionCorrectly()
        {
            //Arrange
            var initialCount = this._repositoryReader.GetAll().Count();
            var foo1 = new Foo {Id = Guid.NewGuid()};
            var foo2 = new Foo {Id = Guid.NewGuid()};

            //Act
            this._repositoryWriter.Insert(foo1);
            this._unitOfWork.SaveChanges();
            this._repositoryWriter.Insert(foo2);
            this._unitOfWork.SaveChanges();

            //Assert
            var all = this._repositoryReader.GetAll();
            Assert.Equal(initialCount + 2, all.Count());
            Assert.Contains(foo1, all);
            Assert.Contains(foo2, all);
        }

        [Fact]
        public void DisposeWorksCorrectly()
        {
            //Arrange
            var cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            var unitOfWork = new InMemoryUnitOfWork();
            var repository = new BaseRepository<Foo>(this._unitOfWork);

            //Act
            repository.Insert(new Foo {Id = Guid.NewGuid()});
            unitOfWork.SaveChanges();
            unitOfWork.Dispose();

            //Assert
        }

        [Fact(Skip = "NotImplemented")]
        public void RollsBackTransactionCorrectly()
        {
            //Arrange
            var initialCount = this._repositoryReader.GetAll().Count();
            var foo1 = new Foo {Id = Guid.NewGuid()};
            var foo2 = new Foo {Id = Guid.NewGuid()};

            //Act
            this._repositoryWriter.Insert(foo1);
            this._unitOfWork.SaveChanges();
            this._repositoryWriter.Insert(foo2);
            this._unitOfWork.SaveChanges();

            //Assert
            var all = this._repositoryReader.GetAll().ToArray();
            Assert.Equal(initialCount, all.Count());
            Assert.DoesNotContain(foo1, all);
            Assert.DoesNotContain(foo2, all);
        }

        [Fact]
        public void SavesChanges()
        {
            //Arrange
            var initialCount = this._repositoryReader.GetAll().Count();
            var foo1 = new Foo {Id = Guid.NewGuid()};
            var foo2 = new Foo {Id = Guid.NewGuid()};

            //Act
            this._repositoryWriter.Insert(foo1);
            this._repositoryWriter.Insert(foo2);
            this._unitOfWork.SaveChanges();

            //Assert
            var all = this._repositoryReader.GetAll().ToArray();
            Assert.Equal(initialCount + 2, all.Count());
            Assert.Contains(foo1, all);
            Assert.Contains(foo2, all);
        }

        [Fact]
        public async Task SavesChangesAsync()
        {
            //Arrange
            var initialCount = this._repositoryReader.GetAll().Count();
            var foo1 = new Foo {Id = Guid.NewGuid()};
            var foo2 = new Foo {Id = Guid.NewGuid()};

            //Act
            this._repositoryWriter.Insert(foo1);
            this._repositoryWriter.Insert(foo2);
            await this._unitOfWork.SaveChangesAsync();

            //Assert
            var all = this._repositoryReader.GetAll().ToArray();
            Assert.Equal(initialCount + 2, all.Count());
            Assert.Contains(foo1, all);
            Assert.Contains(foo2, all);
        }
    }
}