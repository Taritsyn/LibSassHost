namespace LibSassHost.Internal
{
	internal sealed class SassErrorInfo
	{
		public int Status
		{
			get;
			set;
		}

		public string Text
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}

		public string File
		{
			get;
			set;
		}

		public int Line
		{
			get;
			set;
		}

		public int Column
		{
			get;
			set;
		}

		public string Source
		{
			get;
			set;
		}
	}
}