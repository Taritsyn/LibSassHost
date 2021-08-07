using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace LibSassHost.Benchmarks
{
	[MemoryDiagnoser]
	public class ScssFileCompilationBenchmark : ScssCompilationBenchmarkBase
	{
		[Benchmark]
		[Arguments(false)]
		[Arguments(true)]
		public void Compile(bool withFileManager)
		{
			Document document = Documents[DocumentName];

			SetFileManager(withFileManager);
			CompilationResult result = SassCompiler.CompileFile(document.AbsolutePath);
		}
	}
}