// pluskal

using System;
using System.Linq;
using System.Threading.Tasks;
using ArangoDBRepository;
using ArangoDBRepository.Tests;
using Fakes;
using Repository;
using UnitOfWork;
using Xunit;

namespace ArangoDBUnitOfWork.Tests
{
    public class ArangoDBUnitOfWorkTests : IClassFixture<ArangoRepositoryTestsFixtureData>
    {
        private readonly IRepositoryReader<Foo> _repositoryReader;
        private readonly IRepositoryWriter<Foo> _repositoryWriter;
        private readonly IUnitOfWork _unitOfWork;

        public ArangoDBUnitOfWorkTests(ArangoRepositoryTestsFixtureData fixtureData)
        {
            this._unitOfWork = fixtureData.UnitOfWork;

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
            this._unitOfWork.BeginTransaction();
            this._repositoryWriter.Insert(foo1);
            this._unitOfWork.SaveChanges();
            this._repositoryWriter.Insert(foo2);
            this._unitOfWork.SaveChanges();
            this._unitOfWork.Commit();

            //Assert
            var all = this._repositoryReader.GetAll().ToArray();
            Assert.Equal(initialCount + 2, all.Count());
            Assert.Contains(foo1, all);
            Assert.Contains(foo2, all);
        }

        [Fact]
        public void DisposeWorksCorrectly()
        {
            //Arrange
            var unitOfWork = this._unitOfWork;
            var repository = this._repositoryReader as BaseRepository<Foo>;

            //Act
            Assert.NotNull(repository);
            repository.Insert(new FooArango {Id = Guid.NewGuid()});
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
            this._unitOfWork.BeginTransaction();
            this._repositoryWriter.Insert(foo1);
            this._unitOfWork.SaveChanges();
            this._repositoryWriter.Insert(foo2);
            this._unitOfWork.SaveChanges();
            this._unitOfWork.Rollback();

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