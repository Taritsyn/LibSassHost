using System;
#if !NETSTANDARD1_3
using System.Runtime.Serialization;
using System.Security.Permissions;
#endif

namespace LibSassHost
{
	/// <summary>
	/// The exception that is thrown during a Sass compilation
	/// </summary>
#if !NETSTANDARD1_3
	[Serializable]
#endif
	public sealed class SassСompilationException : SassException
	{
		/// <summary>
		/// Error code
		/// </summary>
		private int _errorCode;

		/// <summary>
		/// Name of file, where the error occurred
		/// </summary>
		private string _file = string.Empty;

		/// <summary>
		/// Line number
		/// </summary>
		private int _lineNumber;

		/// <summary>
		/// Column number
		/// </summary>
		private int _columnNumber;

		/// <summary>
		/// Source fragment
		/// </summary>
		private string _sourceFragment = string.Empty;

		/// <summary>
		/// Gets or sets a error code
		///		1 - normal errors like parsing or <code>eval</code> errors;
		///		2 - bad allocation error (memory error);
		///		3 - "untranslated" C++ exception (<code>throw std::exception</code>);
		///		4 - legacy string exceptions (<code>throw const char*</code> or <code>std::string</code>);
		///		5 - some other unknown exception.
		/// </summary>
		public int ErrorCode
		{
			get { return _errorCode; }
			set { _errorCode = value; }
		}

		/// <summary>
		/// Gets or sets a error status
		/// </summary>
		[Obsolete("Use a `ErrorCode` property")]
		public int Status
		{
			get { return _errorCode; }
			set { _errorCode = value; }
		}

		/// <summary>
		/// Gets or sets a error text
		/// </summary>
		[Obsolete("Use a `Description` property")]
		public string Text
		{
			get { return Description; }
			set { Description = value; }
		}

		/// <summary>
		/// Gets or sets a name of file, where the error occurred
		/// </summary>
		public string File
		{
			get { return _file; }
			set { _file = value; }
		}

		/// <summary>
		/// Gets or sets a line number
		/// </summary>
		public int LineNumber
		{
			get { return _lineNumber; }
			set { _lineNumber = value; }
		}

		/// <summary>
		/// Gets or sets a column number
		/// </summary>
		public int ColumnNumber
		{
			get { return _columnNumber; }
			set { _columnNumber = value; }
		}

		/// <summary>
		/// Gets or sets a source fragment
		/// </summary>
		public string SourceFragment
		{
			get { return _sourceFragment; }
			set { _sourceFragment = value; }
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="SassСompilationException"/> class
		/// with a specified error message
		/// </summary>
		/// <param name="message">The message that describes the error</param>
		public SassСompilationException(string message)
			: base(message)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="SassСompilationException"/> class
		/// with a specified error message and a reference to the inner exception
		/// that is the cause of this exception
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception</param>
		/// <param name="innerException">The exception that is the cause of the current exception</param>
		public SassСompilationException(string message, Exception innerException)
			: base(message, innerException)
		{ }
#if !NETSTANDARD1_3

		/// <summary>
		/// Initializes a new instance of the <see cref="SassСompilationException"/> class with serialized data
		/// </summary>
		/// <param name="info">The object that holds the serialized data</param>
		/// <param name="context">The contextual information about the source or destination</param>
		private SassСompilationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			if (info != null)
			{
				_errorCode = info.GetInt32("ErrorCode");
				_file = info.GetString("File");
				_lineNumber = info.GetInt32("LineNumber");
				_columnNumber = info.GetInt32("ColumnNumber");
				_sourceFragment = info.GetString("SourceFragment");
			}
		}


		#region Exception overrides

		/// <summary>
		/// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo"/> to populate with data</param>
		/// <param name="context">The destination (see <see cref="StreamingContext"/>) for this serialization</param>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException(nameof(info));
			}

			base.GetObjectData(info, context);
			info.AddValue("ErrorCode", _errorCode);
			info.AddValue("File", _file);
			info.AddValue("LineNumber", _lineNumber);
			info.AddValue("ColumnNumber", _columnNumber);
			info.AddValue("SourceFragment", _sourceFragment);
		}

		#endregion
#endif
	}
}