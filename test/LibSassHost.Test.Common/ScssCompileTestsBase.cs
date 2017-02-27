namespace LibSassHost.Test.Common
{
	public abstract class ScssCompileTestsBase : CompileTestsBase
	{
		protected ScssCompileTestsBase()
			: base(SyntaxType.Scss)
		{ }
	}
}