// pluskal

using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using Cassandra;
using CassandraRepository.Tests;
using Fakes;

namespace CassandraRepository.Benchmarks
{
    [SimpleJob(RunStrategy.Throughput, 1, 1, 2, id: "FastJob")]
    public class CassandraRepositoryBenchrmark
    {
        private Foo[] _bigItems;
        private BaseRepository<Foo> _repositoryWriterAsync;
        private Foo[] _smallItems;
        private CassandraUnitOfWork.CassandraUnitOfWork _unitOfWork;

        [Params(/*100, */1000/*, 10_000 *//*, 100_000*/)]
        public Int32 ItemsCount { get; set; }

        public Int32 Mtu { get; set; } = 1500;

        [Benchmark]
        public async Task InsertsEntity_Big_Async()
        {
            await this.InsertsEntitiesAsync(this.ItemsCount, this._bigItems);
        }

        [Benchmark]
        public async Task InsertsEntity_Small_Async()
        {
            await this.InsertsEntitiesAsync(this.ItemsCount, this._smallItems);
        }

        [GlobalSetup]
        public void Setup()
        {
            var cluster = Cluster.Builder().AddContactPoint("127.0.0.1").WithDefaultKeyspace(nameof(CassandraRepositoryTests)).Build();
            cluster.ConnectAndCreateDefaultKeyspaceIfNotExists();

            this._unitOfWork = new CassandraUnitOfWork.CassandraUnitOfWork(cluster, new CassandraEntityMappings());
            var repository = new BaseRepository<Foo>(this._unitOfWork);
            this._repositoryWriterAsync = repository;
            this._smallItems = CreateItems(this.ItemsCount, this.Mtu);
            this._bigItems = CreateItems(this.ItemsCount, this.Mtu * 1000);
        }

        private static Foo[] CreateItems(Int32 itemsCount, Int32 nameSize)
        {
            var items = new Foo[itemsCount];

            for (var i = 0; i < itemsCount; i++) items[i] = new Foo {Id = Guid.NewGuid(), Name = new String('a', nameSize)};

            return items;
        }

        private async Task InsertsEntitiesAsync(Int32 itemsCount, Foo[] items)
        {
            var tasks = new Task[itemsCount];
            for (var i = 0; i < itemsCount; i++) tasks[i] = this._repositoryWriterAsync.InsertAsync(items[i]);

            await Task.WhenAll(tasks);
            await this._unitOfWork.SaveChangesAsync();
        }
    }
}