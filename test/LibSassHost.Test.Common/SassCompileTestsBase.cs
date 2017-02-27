namespace LibSassHost.Test.Common
{
	public abstract class SassCompileTestsBase : CompileTestsBase
	{
		protected SassCompileTestsBase()
			: base(SyntaxType.Sass)
		{ }
	}
}