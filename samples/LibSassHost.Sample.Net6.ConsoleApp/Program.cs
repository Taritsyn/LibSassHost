using LibSassHost.Sample.Logic;

namespace LibSassHost.Sample.Net6.ConsoleApp
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