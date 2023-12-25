using LibSassHost.Sample.Logic;

namespace LibSassHost.Sample.Net8.ConsoleApp
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