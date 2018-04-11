using System.Threading.Tasks;
using ArangoDB.Client;
using UnitOfWork;

namespace ArangoDBUnitOfWork
{
    public class ArangoDBUnitOfWork : IUnitOfWork
    {
        public ArangoDBUnitOfWork(DatabaseSharedSetting databaseSharedSetting)
        {
            this.Database = new ArangoDatabase(databaseSharedSetting);
        }

        public ArangoDatabase Database { get; set; }

        public void Dispose()
        {
            this.Database?.Dispose();
        }

        public void SaveChanges()
        {
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }
    }
}