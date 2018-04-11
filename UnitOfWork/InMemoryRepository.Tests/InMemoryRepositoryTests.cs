using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fakes;
using Moq;
using Repository;
using UnitOfWork;
using Xunit;

namespace InMemoryRepository.Tests
{
    public class InMemoryRepositoryTests
    {
        private readonly List<Foo> _foosTable;
        private readonly IRepositoryReader<Foo> _repositoryReader;
        private readonly IRepositoryReaderAsync<Foo> _repositoryReaderAsync;
        private readonly IRepositoryWriter<Foo> _repositoryWriter;
        private readonly IRepositoryWriterAsync<Foo> _repositoryWriterAsync;
        private readonly IUnitOfWork _unitOfWork;

        //private dynamic _context;

        public InMemoryRepositoryTests()
        {
            var unitOfWork = new InMemoryUnitOfWork.InMemoryUnitOfWork();
            var repository = new BaseRepository<Foo>(unitOfWork);
            this._unitOfWork = unitOfWork;
            this._foosTable = GetInstanceField(typeof(BaseRepository<Foo>), repository, "_data") as List<Foo>;

            this._repositoryWriter = repository;
            this._repositoryReader = repository;
            this._repositoryWriterAsync = repository;
            this._repositoryReaderAsync = repository;
        }

        /// <summary>
        ///     Uses reflection to get the field value from an object.
        /// </summary>
        /// <param name="type">The instance type.</param>
        /// <param name="instance">The instance object.</param>
        /// <param name="fieldName">The field's name which is to be fetched.</param>
        /// <returns>The field value from the object.</returns>
        internal static Object GetInstanceField(Type type, Object instance, String fieldName)
        {
            const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                           | BindingFlags.Static;
            var field = type.GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }

        private IEnumerable<Foo> Contains(Foo item)
        {
            yield return this._foosTable.FirstOrDefault(entity => entity.Id == item.Id);
        }

        private void DeleteAll()
        {
            this._foosTable.Clear();
        }

        private Int64 GetCount()
        {
            return this._foosTable.Count();
        }

        #region Test CRUD 

        [Fact]
        public void InsertsEntity()
        {
            //Arrange
            var initialCount = this.GetCount();
            var foo = new Foo {Id = Guid.NewGuid()};

            //Act
            this._repositoryWriter.Insert(foo);
            this._unitOfWork.SaveChanges();

            //Assert
            Assert.Equal(initialCount + 1, this.GetCount());
            Assert.Contains(foo, this.Contains(foo));
        }

        [Fact]
        public void InsertsRange()
        {
            //Arrange
            var initialCount = this.GetCount();
            var foo1 = new Foo {Id = Guid.NewGuid()};
            var foo2 = new Foo {Id = Guid.NewGuid()};

            //Act
            this._repositoryWriter.InsertRange(new[] {foo1, foo2});
            this._unitOfWork.SaveChanges();

            //Assert
            Assert.Equal(initialCount + 2, this.GetCount());
            Assert.Contains(foo1, this.Contains(foo1));
            Assert.Contains(foo2, this.Contains(foo2));
        }

        [Fact]
        public void DeletesEntity()
        {
            //Arrange
            var foo = new Foo {Id = Guid.NewGuid()};
            this._repositoryWriter.Insert(foo);
            this._unitOfWork.SaveChanges();
            var initialCount = this.GetCount();

            //Act
            this._repositoryWriter.Delete(foo);
            this._unitOfWork.SaveChanges();

            //Assert
            Assert.Equal(initialCount - 1, this.GetCount());
            Assert.DoesNotContain(foo, this.Contains(foo));
        }

        [Fact]
        public void DeletesEntityById()
        {
            //Arrange
            var foo = new Foo {Id = Guid.NewGuid()};
            this._repositoryWriter.Insert(foo);
            this._unitOfWork.SaveChanges();
            var initialCount = this.GetCount();

            //Act
            this._repositoryWriter.Delete(foo.Id);
            this._unitOfWork.SaveChanges();

            //Assert
            Assert.Equal(initialCount - 1, this.GetCount());
            Assert.DoesNotContain(foo, this.Contains(foo));
        }

        [Fact]
        public void ThrowsExceptionWhenRecordToDeleteByIdNotFound()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => this._repositoryWriter.Delete(Guid.NewGuid()));
        }

        [Fact]
        public void UpdatesEntity()
        {
            //Arrange
            var foo = new Foo {Id = Guid.NewGuid(), Name = "A"};
            this._repositoryWriter.Insert(foo);
            this._unitOfWork.SaveChanges();

            //Act
            foo.Name = "B";
            this._repositoryWriter.Update(foo);
            this._unitOfWork.SaveChanges();
            var item = this._repositoryReader.GetById(foo.Id);

            //Assert
            Assert.Equal("B", item.Name);
        }

        [Fact]
        public void GetsItemById()
        {
            //Arrange
            var foo = new Foo {Id = Guid.NewGuid()};
            this._repositoryWriter.Insert(foo);
            this._unitOfWork.SaveChanges();

            //Act
            var item = this._repositoryReader.GetById(foo.Id);

            //Assert
            Assert.Equal(foo, item);
        }

        [Fact]
        public void GetsAll()
        {
            //Arrange
            this.DeleteAll();
            var foo1 = new Foo {Id = Guid.NewGuid()};
            var foo2 = new Foo {Id = Guid.NewGuid()};
            this._repositoryWriter.InsertRange(new[] {foo1, foo2});
            this._unitOfWork.SaveChanges();

            //Act
            var items = this._repositoryReader.GetAll().ToArray();

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
            var initialCount = this.GetCount();
            var foo = new Foo {Id = Guid.NewGuid()};

            //Act
            await this._repositoryWriterAsync.InsertAsync(foo);
            await this._unitOfWork.SaveChangesAsync();

            //Assert
            Assert.Equal(initialCount + 1, this.GetCount());
            Assert.Contains(foo, this.Contains(foo));
        }

        [Fact]
        public async Task InsertsRangeAsync()
        {
            //Arrange
            var initialCount = this.GetCount();
            var foo1 = new Foo {Id = Guid.NewGuid()};
            var foo2 = new Foo {Id = Guid.NewGuid()};

            //Act
            await this._repositoryWriterAsync.InsertRangeAsync(new[] {foo1, foo2});
            await this._unitOfWork.SaveChangesAsync();

            //Assert
            Assert.Equal(initialCount + 2, this.GetCount());
            Assert.Contains(foo1, this.Contains(foo1));
            Assert.Contains(foo2, this.Contains(foo2));
        }

        [Fact]
        public async Task DeletesEntityAsync()
        {
            //Arrange
            var foo = new Foo {Id = Guid.NewGuid()};
            await this._repositoryWriterAsync.InsertAsync(foo);
            this._unitOfWork.SaveChanges();
            var initialCount = this.GetCount();

            //Act
            await this._repositoryWriterAsync.DeleteAsync(foo);
            await this._unitOfWork.SaveChangesAsync();

            //Assert
            Assert.Equal(initialCount - 1, this.GetCount());
            Assert.DoesNotContain(foo, this.Contains(foo));
        }

        [Fact]
        public async Task DeletesEntityByIdAsync()
        {
            //Arrange
            var foo = new Foo {Id = Guid.NewGuid()};
            await this._repositoryWriterAsync.InsertAsync(foo);
            await this._unitOfWork.SaveChangesAsync();
            var initialCount = this.GetCount();

            //Act
            await this._repositoryWriterAsync.DeleteAsync(foo.Id);
            await this._unitOfWork.SaveChangesAsync();

            //Assert
            Assert.Equal(initialCount - 1, this.GetCount());
            Assert.DoesNotContain(foo, this.Contains(foo));
        }

        [Fact]
        public async Task ThrowsExceptionWhenRecordToDeleteByIdAsyncNotFound()
        {
            //Arrange

            //Act

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await this._repositoryWriterAsync.DeleteAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task UpdatesEntityAsync()
        {
            //Arrange
            var foo = new Foo {Id = Guid.NewGuid(), Name = "A"};
            await this._repositoryWriterAsync.InsertAsync(foo);
            await this._unitOfWork.SaveChangesAsync();

            //Act
            foo.Name = "B";
            await this._repositoryWriterAsync.UpdateAsync(foo);
            await this._unitOfWork.SaveChangesAsync();
            var item = await this._repositoryReaderAsync.GetByIdAsync(foo.Id);

            //Assert
            Assert.Equal("B", item.Name);
        }

        [Fact]
        public async Task GetsItemByIdAsync()
        {
            //Arrange
            var foo = new Foo {Id = Guid.NewGuid()};
            await this._repositoryWriterAsync.InsertAsync(foo);
            await this._unitOfWork.SaveChangesAsync();

            //Act
            var item = await this._repositoryReaderAsync.GetByIdAsync(foo.Id);

            //Assert
            Assert.Equal(foo, item);
        }

        [Fact]
        public async Task GetsAllAsync()
        {
            //Arrange
            this.DeleteAll();
            var foo1 = new Foo {Id = Guid.NewGuid()};
            var foo2 = new Foo {Id = Guid.NewGuid()};
            await this._repositoryWriterAsync.InsertRangeAsync(new[] {foo1, foo2});
            await this._unitOfWork.SaveChangesAsync();

            //Act
            var items = await this._repositoryReaderAsync.GetAllAsync();

            //Assert
            var foos = items as Foo[] ?? items.ToArray();
            Assert.Contains(foo1, foos);
            Assert.Contains(foo2, foos);
        }

        #endregion CRUD Async

        #region Test PreConditions

        [Fact]
        public void PreConditionFailsWhenNotCassandraUnitOfWorkPassedToConstructor()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentException>(
                () => { new BaseRepository<Foo>(new Mock<IUnitOfWork>().Object); });
        }

        [Fact]
        public void PreConditionFailedWhenTryingToInsertNull()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentNullException>(() => { this._repositoryWriter.Insert(null); });
        }

        [Fact]
        public void PreConditionFailedWhenTryingToInsertNullRange()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentNullException>(() => { this._repositoryWriter.InsertRange(null); });
        }

        [Fact]
        public void PreConditionFailedWhenTryingToUpdateNullItem()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentNullException>(() => { this._repositoryWriter.Update(null); });
        }

        [Fact]
        public void PreConditionFailedWhenTryingToDeleteNullItem()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentNullException>(() => { this._repositoryWriter.Delete(null); });
        }

        #endregion Test PreConditions
    }
}