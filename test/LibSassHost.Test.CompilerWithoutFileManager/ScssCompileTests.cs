using LibSassHost.Test.Common;

namespace LibSassHost.Test.CompilerWithoutFileManager
{
	public sealed class ScssCompileTests : CompileWithoutFileManagerTestsBase
	{
		public ScssCompileTests()
			: base(SyntaxType.Scss)
		{ }
	}
}