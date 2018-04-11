using System;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Mapping;

namespace UnitOfWork.CassandraUnitOfWork
{
    public class CassandraUnitOfWork : IUnitOfWork
    {
        private static Boolean _setUpDone;

        public CassandraUnitOfWork(ICluster cluster, Mappings mappings)
        {
            DoOneTimeSetUp(mappings);
            this.Session = cluster.Connect();
        }

        public ISession Session { get; }

        public void Dispose()
        {
            this.Session?.Dispose();
        }

        public void SaveChanges()
        {
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }

        private static void DoOneTimeSetUp(Mappings mappings)
        {
            if (_setUpDone) return;

            MappingConfiguration.Global.Define(mappings);
            _setUpDone = true;
        }
    }
}