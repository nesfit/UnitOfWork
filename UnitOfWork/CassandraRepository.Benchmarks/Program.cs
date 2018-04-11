using System;
using BenchmarkDotNet.Running;

namespace UnitOfWork.CassandraRepository.Benchmarks
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