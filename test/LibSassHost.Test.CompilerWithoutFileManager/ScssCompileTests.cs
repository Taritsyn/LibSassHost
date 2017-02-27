using LibSassHost.Test.Common;

namespace LibSassHost.Test.CompilerWithoutFileManager
{
	public sealed class ScssCompileTests : ScssCompileTestsBase
	{
		protected override SassCompiler CreateCompiler()
		{
			return new SassCompiler(null);
		}
	}
}