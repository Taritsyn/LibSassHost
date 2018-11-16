using System;
#if !NETSTANDARD1_3
using System.Runtime.Serialization;
#endif

namespace LibSassHost
{
	/// <summary>
	/// The exception that is thrown when a loading of Sass compiler is failed
	/// </summary>
#if !NETSTANDARD1_3
	[Serializable]
#endif
	public sealed class SassCompilerLoadException : SassException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SassCompilerLoadException"/> class
		/// with a specified error message
		/// </summary>
		/// <param name="message">The message that describes the error</param>
		public SassCompilerLoadException(string message)
			: base(message)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="SassCompilerLoadException"/> class
		/// with a specified error message and a reference to the inner exception
		/// that is the cause of this exception
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception</param>
		/// <param name="innerException">The exception that is the cause of the current exception</param>
		public SassCompilerLoadException(string message, Exception innerException)
			: base(message, innerException)
		{ }
#if !NETSTANDARD1_3

		/// <summary>
		/// Initializes a new instance of the <see cref="SassCompilerLoadException"/> class with serialized data
		/// </summary>
		/// <param name="info">The object that holds the serialized data</param>
		/// <param name="context">The contextual information about the source or destination</param>
		private SassCompilerLoadException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
#endif
	}
}