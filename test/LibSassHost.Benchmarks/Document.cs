namespace LibSassHost.Benchmarks
{
	public sealed class Document
	{
		public string RelativePath
		{
			get;
			set;
		}

		public string AbsolutePath
		{
			get;
			set;
		}

		public string Content
		{
			get;
			set;
		}


		public Document()
			: this(string.Empty)
		{ }

		public Document(string relativePath)
		{
			RelativePath = relativePath;
		}
	}
}