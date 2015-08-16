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
    using EF6UnitOfWork.Tests.Fakes;
    using Fakes;
    using Moq;
    using Repository;

    using UnitOfWork;

    using Xunit;

    public class BaseRepositoryAsyncTests
    {
        private readonly DbContext _context;
        private readonly IRepositoryWriterAsync<Foo> _repositoryWriterAsync;
        private readonly IRepositoryReaderAsync<Foo> _repositoryReaderAsync; 

        public BaseRepositoryAsyncTests()
        {
            DbConnection connection = DbConnectionFactory.CreateTransient();
            _context = new FooContext(connection);

            var repository = new BaseRepositoryAsync<Foo>(new Ef6UnitOfWork(_context, IsolationLevel.Unspecified));
            _repositoryWriterAsync = repository;
            _repositoryReaderAsync = repository;
        }

        #region Test CRUD

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
        public async Task UpdatesEntityAsync()
        {
            //Arrange
            var foo = new Foo { Id = Guid.NewGuid(), Name = "A"};
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

        #endregion CRUD

        #region Test PreConditions

        [Fact]
        public void PreConditionFailsWhenNotEf6UnitOfWorkPassedToConstructor()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentException>(
                () =>
                    {
                        new BaseRepositoryAsync<Foo>(new Mock<IUnitOfWork>().Object);
                    });
        }

        [Fact]
        public async Task PreConditionFailedWhenTryingToInsertNull()
        {
            //Arrange

            //Act

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () =>
                    {
                        await _repositoryWriterAsync.InsertAsync(null);
                    });
        }

        [Fact]
        public async Task PreConditionFailedWhenTryingToInsertNullRange()
        {
            //Arrange

            //Act

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () =>
                    {
                        await _repositoryWriterAsync.InsertRangeAsync(null);
                    });
        }

        [Fact]
        public async Task PreConditionFailedWhenTryingToInsertEmptyRange()
        {
            //Arrange

            //Act

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(
                async () =>
                    {
                        await _repositoryWriterAsync.InsertRangeAsync(new List<Foo>());
                    });
        }

        [Fact]
        public async Task PreConditionFailedWhenTryingToUpdateNullItem()
        {
            //Arrange

            //Act

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () =>
                    {
                        await _repositoryWriterAsync.UpdateAsync(null);
                    });
        }

        [Fact]
        public async Task PreConditionFailedWhenTryingToDeleteNullItem()
        {
            //Arrange

            //Act

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () =>
                    {
                        await _repositoryWriterAsync.DeleteAsync(null);
                    });
        }

        #endregion Test PreConditions
    }
}
