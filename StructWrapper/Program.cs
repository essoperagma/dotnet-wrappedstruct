using BenchmarkDotNet.Running;

namespace StructWrapper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            _ = BenchmarkRunner.Run<Benchmarks>();
        }
    }
}