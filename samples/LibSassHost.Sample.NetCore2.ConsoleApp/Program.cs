using LibSassHost.Sample.Logic;

namespace LibSassHost.Sample.NetCore2.ConsoleApp
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