using System;

namespace LibSassHost
{
	/// <summary>
	/// The exception that is thrown during a Sass compilation
	/// </summary>
	public sealed class SassСompilationException : Exception
	{
		/// <summary>
		/// Gets or sets a error status
		/// </summary>
		public int Status
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a error text
		/// </summary>
		public string Text
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a name of file, where the error occurred
		/// </summary>
		public string File
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a line number
		/// </summary>
		public int LineNumber
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a column number
		/// </summary>
		public int ColumnNumber
		{
			get;
			set;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="SassСompilationException"/> class
		/// with a specified error message
		/// </summary>
		/// <param name="message">The message that describes the error</param>
		public SassСompilationException(string message)
			: base(message)
		{
			Status = 0;
			Text = string.Empty;
			File = string.Empty;
			LineNumber = 0;
			ColumnNumber = 0;
		}
	}
}