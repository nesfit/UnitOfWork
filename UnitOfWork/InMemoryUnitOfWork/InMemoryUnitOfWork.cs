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

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }
    }
}