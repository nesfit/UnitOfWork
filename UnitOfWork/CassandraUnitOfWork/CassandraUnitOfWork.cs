using System;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Mapping;

namespace UnitOfWork.CassandraUnitOfWork
{
    public class CassandraUnitOfWork : IUnitOfWork
    {
        private static Boolean _setUpDone;

        public CassandraUnitOfWork(ICluster cluster, Mappings mappings, String keyspace = null)
        {
            DoOneTimeSetUp(mappings);
            this.Session = keyspace != null ? cluster.Connect(keyspace) : cluster.Connect();
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