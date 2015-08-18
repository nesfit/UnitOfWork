namespace EF6Repository.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using Effort;

    using EF6UnitOfWork;

    using Fakes;

    using Moq;

    using Repository;

    using UnitOfWork;

    using Xunit;

    public class BaseRepositoryTests
    {
        private readonly DbContext _context;
        private readonly IRepositoryWriter<Foo> _repositoryWriter;
        private readonly IRepositoryReader<Foo> _repositoryReader;
        private readonly IRepositoryWriterAsync<Foo> _repositoryWriterAsync;
        private readonly IRepositoryReaderAsync<Foo> _repositoryReaderAsync;

        public BaseRepositoryTests()
        {
            DbConnection connection = DbConnectionFactory.CreateTransient();
            _context = new FooContext(connection);

            var repository = new BaseRepository<Foo>(new Ef6UnitOfWork(_context, IsolationLevel.Unspecified));
            _repositoryWriter = repository;
            _repositoryReader = repository;
            _repositoryWriterAsync = repository;
            _repositoryReaderAsync = repository;
        }

        #region Test CRUD 

        [Fact]
        public void InsertsEntity()
        {
            //Arrange
            var initialCount = _context.Set<Foo>().Count();
            var foo = new Foo { Id = Guid.NewGuid() };

            //Act
            _repositoryWriter.Insert(foo);
            _context.SaveChanges();

            //Assert
            Assert.Equal(initialCount + 1, _context.Set<Foo>().Count());
            Assert.Contains(foo, _context.Set<Foo>().AsEnumerable());
        }

        [Fact]
        public void InsertsRange()
        {
            //Arrange
            var initialCount = _context.Set<Foo>().Count();
            var foo1 = new Foo { Id = Guid.NewGuid() };
            var foo2 = new Foo { Id = Guid.NewGuid() };

            //Act
            _repositoryWriter.InsertRange(new[] { foo1, foo2 });
            _context.SaveChanges();

            //Assert
            Assert.Equal(initialCount + 2, _context.Set<Foo>().Count());
            Assert.Contains(foo1, _context.Set<Foo>().AsEnumerable());
            Assert.Contains(foo2, _context.Set<Foo>().AsEnumerable());
        }
        
        [Fact]
        public void DeletesEntity()
        {
            //Arrange
            var foo = new Foo { Id = Guid.NewGuid() };
            _repositoryWriter.Insert(foo);
            _context.SaveChanges();
            var initialCount = _context.Set<Foo>().Count();

            //Act
            _repositoryWriter.Delete(foo);
            _context.SaveChanges();

            //Assert
            Assert.Equal(initialCount - 1, _context.Set<Foo>().Count());
            Assert.DoesNotContain(foo, _context.Set<Foo>().AsEnumerable());
        }

        [Fact]
        public void DeletesEntityById()
        {
            //Arrange
            var foo = new Foo { Id = Guid.NewGuid() };
            _repositoryWriter.Insert(foo);
            _context.SaveChanges();
            var initialCount = _context.Set<Foo>().Count();

            //Act
            _repositoryWriter.Delete(foo.Id);
            _context.SaveChanges();

            //Assert
            Assert.Equal(initialCount - 1, _context.Set<Foo>().Count());
            Assert.DoesNotContain(foo, _context.Set<Foo>().AsEnumerable());
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
        public void UpdatesEntity()
        {
            //Arrange
            var foo = new Foo { Id = Guid.NewGuid(), Name = "A"};
            _repositoryWriter.Insert(foo);
            _context.SaveChanges();

            //Act
            foo.Name = "B";
            _repositoryWriter.Update(foo);
            _context.SaveChanges();
            var item = _repositoryReader.GetById(foo.Id);

            //Assert
            Assert.Equal("B", item.Name);
        }

        [Fact]
        public void GetsItemById()
        {
            //Arrange
            var foo = new Foo { Id = Guid.NewGuid() };
            _repositoryWriter.Insert(foo);
            _context.SaveChanges();

            //Act
            var item = _repositoryReader.GetById(foo.Id);

            //Assert
            Assert.Same(foo, item);
        }

        [Fact]
        public void GetsAll()
        {
            //Arrange
            foreach (var f in _context.Set<Foo>())
            {
                _repositoryWriter.Delete(f);
            }
            var foo1 = new Foo { Id = Guid.NewGuid() };
            var foo2 = new Foo { Id = Guid.NewGuid() };
            _repositoryWriter.InsertRange(new[] { foo1, foo2 });
            _context.SaveChanges();

            //Act
            var items = _repositoryReader.GetAll();

            //Assert
            Assert.Contains(foo1, items);
            Assert.Contains(foo2, items);
        }

        #endregion Test CRUD

        #region Test CRUD Async

        [Fact]
        public async Task InsertsEntityAsync()
        {
            //Arrange
            var initialCount = _context.Set<Foo>().Count();
            var foo = new Foo { Id = Guid.NewGuid() };

            //Act
            await _repositoryWriterAsync.InsertAsync(foo);
            await _context.SaveChangesAsync();

            //Assert
            Assert.Equal(initialCount + 1, _context.Set<Foo>().Count());
            Assert.Contains(foo, _context.Set<Foo>().AsEnumerable());
        }

        [Fact]
        public async Task InsertsRangeAsync()
        {
            //Arrange
            var initialCount = _context.Set<Foo>().Count();
            var foo1 = new Foo { Id = Guid.NewGuid() };
            var foo2 = new Foo { Id = Guid.NewGuid() };

            //Act
            await _repositoryWriterAsync.InsertRangeAsync(new[] { foo1, foo2 });
            await _context.SaveChangesAsync();

            //Assert
            Assert.Equal(initialCount + 2, _context.Set<Foo>().Count());
            Assert.Contains(foo1, _context.Set<Foo>().AsEnumerable());
            Assert.Contains(foo2, _context.Set<Foo>().AsEnumerable());
        }

        [Fact]
        public async Task DeletesEntityAsync()
        {
            //Arrange
            var foo = new Foo { Id = Guid.NewGuid() };
            await _repositoryWriterAsync.InsertAsync(foo);
            _context.SaveChanges();
            var initialCount = _context.Set<Foo>().Count();

            //Act
            await _repositoryWriterAsync.DeleteAsync(foo);
            await _context.SaveChangesAsync();

            //Assert
            Assert.Equal(initialCount - 1, _context.Set<Foo>().Count());
            Assert.DoesNotContain(foo, _context.Set<Foo>().AsEnumerable());
        }

        [Fact]
        public async Task DeletesEntityByIdAsync()
        {
            //Arrange
            var foo = new Foo { Id = Guid.NewGuid() };
            await _repositoryWriterAsync.InsertAsync(foo);
            await _context.SaveChangesAsync();
            var initialCount = _context.Set<Foo>().Count();

            //Act
            await _repositoryWriterAsync.DeleteAsync(foo.Id);
            await _context.SaveChangesAsync();

            //Assert
            Assert.Equal(initialCount - 1, _context.Set<Foo>().Count());
            Assert.DoesNotContain(foo, _context.Set<Foo>().AsEnumerable());
        }

        [Fact]
        public async Task ThrowsExceptionWhenRecordToDeleteByIdAsyncNotFound()
        {
            //Arrange

            //Act

            //Assert
            await
                Assert.ThrowsAsync<ArgumentException>(
                    async () => await _repositoryWriterAsync.DeleteAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task UpdatesEntityAsync()
        {
            //Arrange
            var foo = new Foo { Id = Guid.NewGuid(), Name = "A" };
            await _repositoryWriterAsync.InsertAsync(foo);
            await _context.SaveChangesAsync();

            //Act
            foo.Name = "B";
            await _repositoryWriterAsync.UpdateAsync(foo);
            await _context.SaveChangesAsync();
            var item = await _repositoryReaderAsync.GetByIdAsync(foo.Id);

            //Assert
            Assert.Equal("B", item.Name);
        }

        [Fact]
        public async Task GetsItemByIdAsync()
        {
            //Arrange
            var foo = new Foo { Id = Guid.NewGuid() };
            await _repositoryWriterAsync.InsertAsync(foo);
            await _context.SaveChangesAsync();

            //Act
            var item = await _repositoryReaderAsync.GetByIdAsync(foo.Id);

            //Assert
            Assert.Same(foo, item);
        }

        [Fact]
        public async Task GetsAllAsync()
        {
            //Arrange
            foreach (var f in _context.Set<Foo>())
            {
                await _repositoryWriterAsync.DeleteAsync(f);
            }
            var foo1 = new Foo { Id = Guid.NewGuid() };
            var foo2 = new Foo { Id = Guid.NewGuid() };
            await _repositoryWriterAsync.InsertRangeAsync(new[] { foo1, foo2 });
            await _context.SaveChangesAsync();

            //Act
            var items = await _repositoryReaderAsync.GetAllAsync();

            //Assert
            Assert.Contains(foo1, items);
            Assert.Contains(foo2, items);
        }

        #endregion CRUD Async

        #region Test PreConditions

        [Fact]
        public void PreConditionFailsWhenNotEf6UnitOfWorkPassedToConstructor()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentException>(
                () =>
                    { new BaseRepository<Foo>(new Mock<IUnitOfWork>().Object); });
        }

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
    }
}
