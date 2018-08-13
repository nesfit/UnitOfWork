using System.Threading.Tasks;
using ArangoDB.Client;

namespace UnitOfWork.ArangoDBUnitOfWork
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

        public async Task SaveChangesAsync()
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}