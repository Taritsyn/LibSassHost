using LibSassHost.Sample.Logic;

namespace LibSassHost.Sample.Net7.ConsoleApp
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