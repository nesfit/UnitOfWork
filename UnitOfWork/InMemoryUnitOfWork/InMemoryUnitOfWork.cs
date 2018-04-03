// pluskal

using System;
using System.Threading.Tasks;
using UnitOfWork;

namespace InMemoryUnitOfWork
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