namespace EF6UnitOfWork.Tests.Fakes
{
    using System.Data.Common;
    using System.Data.Entity;

    using global::Fakes;

    public class FooContext : DbContext
    {
        public FooContext(DbConnection connection)
            : base(connection, true)
        {
        }

        public IDbSet<Foo> Foos { get; set; } 
    }
}