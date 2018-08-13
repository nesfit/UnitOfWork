using System.Threading.Tasks;

namespace UnitOfWork.DevnullUnitOfWork
{
    public class DevnullUnitOfWork : IUnitOfWork
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