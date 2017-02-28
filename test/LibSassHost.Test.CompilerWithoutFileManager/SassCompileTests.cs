using LibSassHost.Test.Common;

namespace LibSassHost.Test.CompilerWithoutFileManager
{
	public sealed class SassCompileTests : CompileWithoutFileManagerTestsBase
	{
		public SassCompileTests()
			: base(SyntaxType.Sass)
		{ }
	}
}