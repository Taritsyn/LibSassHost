using LibSassHost.Test.Common;

namespace LibSassHost.Test.CompilerWithoutFileManager
{
	public sealed class SassCompileTests : SassCompileTestsBase
	{
		protected override SassCompiler CreateCompiler()
		{
			return new SassCompiler(null);
		}
	}
}