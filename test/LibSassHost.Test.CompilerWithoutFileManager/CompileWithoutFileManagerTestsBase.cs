using LibSassHost.Test.Common;

namespace LibSassHost.Test.CompilerWithoutFileManager
{
	public abstract class CompileWithoutFileManagerTestsBase : CompileTestsBase
	{
		static CompileWithoutFileManagerTestsBase()
		{
			SassCompiler.FileManager = null;
		}

		protected CompileWithoutFileManagerTestsBase(SyntaxType syntaxType)
			: base(syntaxType)
		{ }
	}
}