using LibSassHost.Test.Common;

namespace LibSassHost.Test.CompilerWithFileManager
{
	public sealed class ScssCompileTests : CompileWithFileManagerTestsBase
	{
		public ScssCompileTests()
			: base(SyntaxType.Scss)
		{ }
	}
}