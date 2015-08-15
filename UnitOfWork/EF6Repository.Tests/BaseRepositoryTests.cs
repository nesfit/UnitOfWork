namespace EF6Repository.Tests
{
    using System;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using Effort;

    using EF6Repository.Tests.Fakes;

    using Moq;

    using Repository;

    using Xunit;

    public class BaseRepositoryTests
    {
        private readonly DbContext _context;
        private readonly IRepositoryWriter<Foo> _repositoryWriter;
        private readonly IRepositoryReader<Foo> _repositoryReader; 

        public BaseRepositoryTests()
        {
            DbConnection connection = DbConnectionFactory.CreateTransient();
            _context = new FooContext(connection);

            var repository = new BaseRepository<Foo>(_context);
            _repositoryWriter = repository;
            _repositoryReader = repository;
        }

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
        
    }
}
