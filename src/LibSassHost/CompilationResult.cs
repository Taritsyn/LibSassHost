using System.Collections.Generic;

namespace LibSassHost
{
	/// <summary>
	/// Compilation result
	/// </summary>
	public sealed class CompilationResult
	{
		/// <summary>
		/// Gets or sets a compilated Sass-code
		/// </summary>
		public string CompiledContent
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a list of included files
		/// </summary>
		public IList<string> IncludedFilePaths
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a source map
		/// </summary>
		public string SourceMap
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs a instance of the compilation result
		/// </summary>
		public CompilationResult()
		{
			CompiledContent = string.Empty;
			IncludedFilePaths = new List<string>();
			SourceMap = string.Empty;
		}
	}
}