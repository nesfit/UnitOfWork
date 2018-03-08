using System;
using BenchmarkDotNet.Running;

namespace CassandraRepository.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<CassandraRepositoryBenchrmark>();

            //Console.ReadKey();
        }
    }
}
