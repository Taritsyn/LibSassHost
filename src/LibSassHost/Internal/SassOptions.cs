namespace LibSassHost.Internal
{
	internal sealed class SassOptions
	{
		public string[] AdditionalImportExtensions
		{
			get;
			set;
		}

		public string IncludePath
		{
			get;
			set;
		}

		public string Indent
		{
			get;
			set;
		}

		public string LineFeed
		{
			get;
			set;
		}

		public bool OmitSourceMapUrl
		{
			get;
			set;
		}

		public OutputStyle OutputStyle
		{
			get;
			set;
		}

		public int Precision
		{
			get;
			set;
		}

		public bool SourceComments
		{
			get;
			set;
		}

		public bool SourceMapContents
		{
			get;
			set;
		}

		public bool SourceMapEmbed
		{
			get;
			set;
		}

		public bool SourceMapFileUrls
		{
			get;
			set;
		}

		public string SourceMapRoot
		{
			get;
			set;
		}
	}
}