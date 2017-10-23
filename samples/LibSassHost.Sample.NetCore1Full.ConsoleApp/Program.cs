using LibSassHost.Sample.Logic;

namespace LibSassHost.Sample.NetCore1Full.ConsoleApp
{
	class Program : CompilationExampleBase
	{
		static void Main(string[] args)
		{
			CompileContent();
			CompileFile();
		}
	}
}