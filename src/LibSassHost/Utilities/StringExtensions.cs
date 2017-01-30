using System;

namespace LibSassHost.Utilities
{
	/// <summary>
	/// Extensions for String
	/// </summary>
	internal static class StringExtensions
	{
		/// <summary>
		/// Replaces tabs by specified number of spaces
		/// </summary>
		/// <param name="source">String value</param>
		/// <param name="tabSize">Number of spaces in tab</param>
		/// <returns>Processed string value</returns>
		public static string TabsToSpaces(this string source, int tabSize)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			string result = source.Replace("\t", "".PadRight(tabSize));

			return result;
		}
	}
}