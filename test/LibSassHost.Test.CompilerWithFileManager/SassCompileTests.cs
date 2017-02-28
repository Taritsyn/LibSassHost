using LibSassHost.Test.Common;

namespace LibSassHost.Test.CompilerWithFileManager
{
	public sealed class SassCompileTests : CompileWithFileManagerTestsBase
	{
		public SassCompileTests()
			: base(SyntaxType.Sass)
		{ }
	}
}