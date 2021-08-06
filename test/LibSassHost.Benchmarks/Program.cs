using BenchmarkDotNet.Running;

namespace LibSassHost.Benchmarks
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			BenchmarkRunner.Run<ScssCompilationBenchmark>();
		}
	}
}