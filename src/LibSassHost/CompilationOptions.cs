using System.Collections.Generic;

namespace LibSassHost
{
	/// <summary>
	/// Compilation options
	/// </summary>
	public sealed class CompilationOptions
	{
		/// <summary>
		/// Gets or sets a list of additional <code>@import</code> file extensions
		/// </summary>
		public IList<string> AdditionalImportExtensions
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a list of include paths
		/// </summary>
		public IList<string> IncludePaths
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a indent type
		/// </summary>
		public IndentType IndentType
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a number of spaces or tabs to be used for indentation
		/// </summary>
		public int IndentWidth
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to embed <code>sourceMappingUrl</code> as data uri
		/// </summary>
		public bool InlineSourceMap
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a line feed type
		/// </summary>
		public LineFeedType LineFeedType
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to disable <code>sourceMappingUrl</code> in css output
		/// </summary>
		public bool OmitSourceMapUrl
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a output style for the generated css code
		/// </summary>
		public OutputStyle OutputStyle
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a precision for fractional numbers
		/// </summary>
		public int Precision
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to emit comments in the generated CSS
		/// indicating the corresponding source line
		/// </summary>
		public bool SourceComments
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to enable source map generation
		/// </summary>
		public bool SourceMap
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to create file urls for sources
		/// </summary>
		public bool SourceMapFileUrls
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to include contents in maps
		/// </summary>
		public bool SourceMapIncludeContents
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value will be emitted as <code>sourceRoot</code> in the source map information
		/// </summary>
		public string SourceMapRootPath
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs a instance of the Sass compilation options
		/// </summary>
		public CompilationOptions()
		{
			AdditionalImportExtensions = new List<string>
			{
				".css"
			};
			IncludePaths = new List<string>();
			IndentType = IndentType.Space;
			IndentWidth = 2;
			InlineSourceMap = false;
			LineFeedType = LineFeedType.Lf;
			OmitSourceMapUrl = false;
			OutputStyle = OutputStyle.Nested;
			Precision = 5;
			SourceComments = false;
			SourceMap = false;
			SourceMapFileUrls = false;
			SourceMapIncludeContents = false;
			SourceMapRootPath = string.Empty;
		}
	}
}