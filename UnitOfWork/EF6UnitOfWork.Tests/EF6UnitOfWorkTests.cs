namespace EF6UnitOfWork.Tests
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using EF6Repository;

    using Fakes;

    using UnitOfWork;
    using Moq;

    using Repository;

    using Xunit;

    public class EF6UnitOfWorkTests
    {
        private readonly DbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryWriter<Foo> _repositoryWriter;
        private readonly IRepositoryReader<Foo> _repositoryReader; 

        public EF6UnitOfWorkTests()
        {
            DbConnection connection = Effort.DbConnectionFactory.CreateTransient();
            this._context = new FooContext(connection);

            this._unitOfWork = new Ef6UnitOfWork(this._context, IsolationLevel.Unspecified);

            var repository = new BaseRepository<Foo>(this._unitOfWork);
            this._repositoryWriter = repository;
            this._repositoryReader = repository;
        }
        
        [Fact]
        public void SavesChanges()
        {
            //Arrange
            var initialCount = this._repositoryReader.GetAll().Count();
            var foo1 = new Foo { Id = Guid.NewGuid() };
            var foo2 = new Foo { Id = Guid.NewGuid() };

            //Act
            this._repositoryWriter.Insert(foo1);
            this._repositoryWriter.Insert(foo2);
            this._unitOfWork.SaveChanges();

            //Assert
            var all = this._repositoryReader.GetAll();
            Assert.Equal(initialCount + 2, all.Count());
            Assert.Contains(foo1, all);
            Assert.Contains(foo2, all);
        }

        [Fact]
        public async Task SavesChangesAsync()
        {
            //Arrange
            var initialCount = this._repositoryReader.GetAll().Count();
            var foo1 = new Foo { Id = Guid.NewGuid() };
            var foo2 = new Foo { Id = Guid.NewGuid() };

            //Act
            this._repositoryWriter.Insert(foo1);
            this._repositoryWriter.Insert(foo2);
            await this._unitOfWork.SaveChangesAsync();

            //Assert
            var all = this._repositoryReader.GetAll();
            Assert.Equal(initialCount + 2, all.Count());
            Assert.Contains(foo1, all);
            Assert.Contains(foo2, all);
        }
        
        [Fact]
        public void CommitsTransactionCorrectly()
        {
            //Arrange
            var initialCount = this._repositoryReader.GetAll().Count();
            var foo1 = new Foo { Id = Guid.NewGuid() };
            var foo2 = new Foo { Id = Guid.NewGuid() };

            //Act
            this._unitOfWork.BeginTransaction();
            this._repositoryWriter.Insert(foo1);
            this._unitOfWork.SaveChanges();
            this._repositoryWriter.Insert(foo2);
            this._unitOfWork.SaveChanges();
            this._unitOfWork.Commit();

            //Assert
            var all = this._repositoryReader.GetAll();
            Assert.Equal(initialCount + 2, all.Count());
            Assert.Contains(foo1, all);
            Assert.Contains(foo2, all);
        }

        [Fact]
        public void RollsBackTransactionCorrectly()
        {
            //Arrange
            var initialCount = this._repositoryReader.GetAll().Count();
            var foo1 = new Foo { Id = Guid.NewGuid() };
            var foo2 = new Foo { Id = Guid.NewGuid() };

            //Act
            this._unitOfWork.BeginTransaction();
            this._repositoryWriter.Insert(foo1);
            this._unitOfWork.SaveChanges();
            this._repositoryWriter.Insert(foo2);
            this._unitOfWork.SaveChanges();
            this._unitOfWork.Rollback();

            //Assert
            var all = this._repositoryReader.GetAll();
            Assert.Equal(initialCount, all.Count());
            Assert.DoesNotContain(foo1, all);
            Assert.DoesNotContain(foo2, all);
        }

        [Fact]
        public void DisposeWorksCorrectly()
        {
            //Arrange
            DbConnection connection = Effort.DbConnectionFactory.CreateTransient();
            var context = new FooContext(connection);
            var unitOfWork = new Ef6UnitOfWork(this._context, IsolationLevel.Unspecified);
            var repository = new BaseRepository<Foo>(unitOfWork);

            //Act
            repository.Insert(new Foo { Id = Guid.NewGuid() });
            unitOfWork.SaveChanges();
            unitOfWork.Dispose();

            //Assert
        }
    }
}
