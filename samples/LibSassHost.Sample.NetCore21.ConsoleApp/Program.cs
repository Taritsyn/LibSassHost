using System.Text;

using LibSassHost.Sample.Logic;

namespace LibSassHost.Sample.NetCore21.ConsoleApp
{
	class Program : CompilationExampleBase
	{
		/// <summary>
		/// Static constructor
		/// </summary>
		static Program()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}


		static void Main(string[] args)
		{
			CompileContent();
			CompileFile();
		}
	}
}