using LibSassHost.Sample.Logic;

namespace LibSassHost.Sample.NetCore21.ConsoleApp
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