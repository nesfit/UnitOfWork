using System;
using BenchmarkDotNet.Running;

namespace CassandraRepository.Benchmarks
{
    public class Program
    {
        public static void Main(String[] args)
        {
            BenchmarkRunner.Run<CassandraRepositoryBenchrmark>();

            //Console.ReadKey();
        }
    }
}