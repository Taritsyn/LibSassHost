using System;
using System.Linq;
using System.Text;

using LibSassHost.Resources;

namespace LibSassHost.Helpers
{
	/// <summary>
	/// Path helpers
	/// </summary>
	public static class PathHelpers
	{
		/// <summary>
		/// Converts a back slashes to forward slashes
		/// </summary>
		/// <param name="path">Path with back slashes</param>
		/// <returns>Path with forward slashes</returns>
		public static string ProcessBackSlashes(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException(
					nameof(path),
					string.Format(Strings.Common_ArgumentIsNull, nameof(path))
				);
			}

			if (string.IsNullOrWhiteSpace(path))
			{
				return path;
			}

			string result = path.Replace('\\', '/');

			return result;
		}
	}
}