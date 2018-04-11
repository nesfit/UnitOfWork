using System.Data.Common;
using System.Data.Entity;
using UnitOfWork.Fakes;

namespace UnitOfWork.EF6Fakes
{
    public class FooContext : DbContext
    {
        public FooContext(DbConnection connection)
            : base(connection, true)
        {
        }

        public IDbSet<Foo> Foos { get; set; }
    }
}