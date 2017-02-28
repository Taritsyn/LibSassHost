using LibSassHost.Test.Common;

namespace LibSassHost.Test.CompilerWithFileManager
{
	public sealed class ScssCompileTests : ScssCompileTestsBase
	{
		protected override SassCompiler CreateCompiler()
		{
			return new SassCompiler(FileManager.Instance);
		}
	}
}