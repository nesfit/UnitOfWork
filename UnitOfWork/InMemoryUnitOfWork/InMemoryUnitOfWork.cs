using System.Threading.Tasks;

namespace UnitOfWork.InMemoryUnitOfWork
{
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        public void Dispose()
        {
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