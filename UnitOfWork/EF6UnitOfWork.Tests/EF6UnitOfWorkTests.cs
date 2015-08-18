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

    using global::Fakes;

    using global::UnitOfWork;
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
            _context = new FooContext(connection);

            _unitOfWork = new Ef6UnitOfWork(_context, IsolationLevel.Unspecified);

            var repository = new BaseRepository<Foo>(_unitOfWork);
            _repositoryWriter = repository;
            _repositoryReader = repository;
        }
        
        [Fact]
        public void SavesChanges()
        {
            //Arrange
            var initialCount = _repositoryReader.GetAll().Count();
            var foo1 = new Foo { Id = Guid.NewGuid() };
            var foo2 = new Foo { Id = Guid.NewGuid() };

            //Act
            _repositoryWriter.Insert(foo1);
            _repositoryWriter.Insert(foo2);
            _unitOfWork.SaveChanges();

            //Assert
            var all = _repositoryReader.GetAll();
            Assert.Equal(initialCount + 2, all.Count());
            Assert.Contains(foo1, all);
            Assert.Contains(foo2, all);
        }

        [Fact]
        public async Task SavesChangesAsync()
        {
            //Arrange
            var initialCount = _repositoryReader.GetAll().Count();
            var foo1 = new Foo { Id = Guid.NewGuid() };
            var foo2 = new Foo { Id = Guid.NewGuid() };

            //Act
            _repositoryWriter.Insert(foo1);
            _repositoryWriter.Insert(foo2);
            await _unitOfWork.SaveChangesAsync();

            //Assert
            var all = _repositoryReader.GetAll();
            Assert.Equal(initialCount + 2, all.Count());
            Assert.Contains(foo1, all);
            Assert.Contains(foo2, all);
        }
        
        [Fact]
        public void CommitsTransactionCorrectly()
        {
            //Arrange
            var initialCount = _repositoryReader.GetAll().Count();
            var foo1 = new Foo { Id = Guid.NewGuid() };
            var foo2 = new Foo { Id = Guid.NewGuid() };

            //Act
            _unitOfWork.BeginTransaction();
            _repositoryWriter.Insert(foo1);
            _unitOfWork.SaveChanges();
            _repositoryWriter.Insert(foo2);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();

            //Assert
            var all = _repositoryReader.GetAll();
            Assert.Equal(initialCount + 2, all.Count());
            Assert.Contains(foo1, all);
            Assert.Contains(foo2, all);
        }

        [Fact]
        public void RollsBackTransactionCorrectly()
        {
            //Arrange
            var initialCount = _repositoryReader.GetAll().Count();
            var foo1 = new Foo { Id = Guid.NewGuid() };
            var foo2 = new Foo { Id = Guid.NewGuid() };

            //Act
            _unitOfWork.BeginTransaction();
            _repositoryWriter.Insert(foo1);
            _unitOfWork.SaveChanges();
            _repositoryWriter.Insert(foo2);
            _unitOfWork.SaveChanges();
            _unitOfWork.Rollback();

            //Assert
            var all = _repositoryReader.GetAll();
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
            var unitOfWork = new Ef6UnitOfWork(_context, IsolationLevel.Unspecified);
            var repository = new BaseRepository<Foo>(unitOfWork);

            //Act
            repository.Insert(new Foo { Id = Guid.NewGuid() });
            unitOfWork.SaveChanges();
            unitOfWork.Dispose();

            //Assert
        }
    }
}
