using System;
using System.Collections.Generic;

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
		public static string BackSlashesToForwardSlashes(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path", string.Format(Strings.Common_ArgumentIsNull, "path"));
			}

			if (string.IsNullOrWhiteSpace(path))
			{
				return path;
			}

			string result = path.Replace("\\", "/");

			return result;
		}

		/// <summary>
		/// Normalizes a path
		/// </summary>
		/// <param name="path">path</param>
		/// <returns>Normalized path</returns>
		public static string NormalizePath(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path", string.Format(Strings.Common_ArgumentIsNull, "path"));
			}

			if (string.IsNullOrWhiteSpace(path))
			{
				return string.Empty;
			}

			string[] urlParts = path.Split('/');
			int urlPartCount = urlParts.Length;
			if (urlPartCount == 0)
			{
				return path;
			}

			var resultUrlParts = new List<string>();

			for (int urlPartIndex = 0; urlPartIndex < urlPartCount; urlPartIndex++)
			{
				string urlPart = urlParts[urlPartIndex];

				switch (urlPart)
				{
					case "..":
						int resultUrlPartCount = resultUrlParts.Count;
						int resultUrlPartLastIndex = resultUrlPartCount - 1;

						if (resultUrlPartCount == 0 || resultUrlParts[resultUrlPartLastIndex] == "..")
						{
							resultUrlParts.Add(urlPart);
						}
						else
						{
							resultUrlParts.RemoveAt(resultUrlPartLastIndex);
						}
						break;
					case ".":
						break;
					default:
						resultUrlParts.Add(urlPart);
						break;
				}
			}

			string resultUrl = string.Join("/", resultUrlParts);
			resultUrlParts.Clear();

			return resultUrl;
		}
	}
}