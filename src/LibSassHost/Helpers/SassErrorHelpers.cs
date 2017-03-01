using System;
using System.Globalization;
using System.Text;

using LibSassHost.Resources;
using LibSassHost.Utilities;

namespace LibSassHost.Helpers
{
	/// <summary>
	/// Sass error helpers
	/// </summary>
	public static class SassErrorHelpers
	{
		/// <summary>
		/// Generates a detailed error message
		/// </summary>
		/// <param name="sassСompilationException">Sass compilation exception</param>
		/// <returns>Detailed error message</returns>
		public static string Format(SassСompilationException sassСompilationException)
		{
			string message = sassСompilationException.Text;
			string filePath = sassСompilationException.File;
			int lineNumber = sassСompilationException.LineNumber;
			int columnNumber = sassСompilationException.ColumnNumber;
			string sourceCode = sassСompilationException.Source;
			string sourceFragment = SourceCodeNavigator.GetSourceFragment(sourceCode,
				new SourceCodeNodeCoordinates(lineNumber, columnNumber));

			var errorMessage = new StringBuilder();
			errorMessage.AppendFormatLine("{0}: {1}", Strings.ErrorDetails_Message, message);
			if (!string.IsNullOrWhiteSpace(filePath))
			{
				errorMessage.AppendFormatLine("{0}: {1}", Strings.ErrorDetails_File, filePath);
			}
			if (lineNumber > 0)
			{
				errorMessage.AppendFormatLine("{0}: {1}", Strings.ErrorDetails_LineNumber,
					lineNumber.ToString(CultureInfo.InvariantCulture));
			}
			if (columnNumber > 0)
			{
				errorMessage.AppendFormatLine("{0}: {1}", Strings.ErrorDetails_ColumnNumber,
					columnNumber.ToString(CultureInfo.InvariantCulture));
			}
			if (!string.IsNullOrWhiteSpace(sourceFragment))
			{
				errorMessage.AppendFormatLine("{1}:{0}{0}{2}", Environment.NewLine,
					Strings.ErrorDetails_SourceFragment,
					sourceFragment);
			}

			return errorMessage.ToString();
		}
	}
}