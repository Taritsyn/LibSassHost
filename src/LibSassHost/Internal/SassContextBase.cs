namespace LibSassHost.Internal
{
	internal abstract class SassContextBase
	{
		public SassErrorInfo Error
		{
			get;
			set;
		}

		public string[] IncludedFiles
		{
			get;
			set;
		}

		public string InputPath
		{
			get;
			set;
		}

		public bool IsIndentedSyntaxSource
		{
			get;
			set;
		}

		public SassOptions Options
		{
			get;
			set;
		}

		public string OutputPath
		{
			get;
			set;
		}

		public string OutputString
		{
			get;
			set;
		}

		public string SourceMapFile
		{
			get;
			set;
		}

		public string SourceMapString
		{
			get;
			set;
		}
	}
}