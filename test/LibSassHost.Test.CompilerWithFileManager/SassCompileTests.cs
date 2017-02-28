using LibSassHost.Test.Common;

namespace LibSassHost.Test.CompilerWithFileManager
{
	public sealed class SassCompileTests : SassCompileTestsBase
	{
		protected override SassCompiler CreateCompiler()
		{
			return new SassCompiler(FileManager.Instance);
		}
	}
}