using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using LibSassHost.Resources;

namespace LibSassHost
{
	/// <summary>
	/// Base class of file manager
	/// </summary>
	public abstract class FileManagerBase : IFileManager
	{
		/// <summary>
		/// Regular expression for working with multiple forward slashes
		/// </summary>
		private static readonly Regex _multipleForwardSlashesRegex = new Regex("/{2,}");


		/// <summary>
		/// Converts a back slashes to forward slashes
		/// </summary>
		/// <param name="path">Path with back slashes</param>
		/// <returns>Path with forward slashes</returns>
		protected static string BackSlashesToForwardSlashes(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path",
					string.Format(Strings.Common_ArgumentIsNull, "path"));
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
		protected static string NormalizePath(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path",
					string.Format(Strings.Common_ArgumentIsNull, "path"));
			}

			if (string.IsNullOrWhiteSpace(path))
			{
				return path;
			}

			string resultPath = path;

			if (resultPath.IndexOf("./", StringComparison.Ordinal) != -1)
			{
				string[] pathParts = resultPath.Split('/');
				int pathPartCount = pathParts.Length;
				if (pathPartCount == 0)
				{
					return path;
				}

				var resultPathParts = new List<string>();

				for (int pathPartIndex = 0; pathPartIndex < pathPartCount; pathPartIndex++)
				{
					string pathPart = pathParts[pathPartIndex];

					switch (pathPart)
					{
						case "..":
							int resultPathPartCount = resultPathParts.Count;
							int resultPathPartLastIndex = resultPathPartCount - 1;

							if (resultPathPartCount == 0 || resultPathParts[resultPathPartLastIndex] == "..")
							{
								resultPathParts.Add(pathPart);
							}
							else
							{
								resultPathParts.RemoveAt(resultPathPartLastIndex);
							}
							break;
						case ".":
							break;
						default:
							resultPathParts.Add(pathPart);
							break;
					}
				}

				resultPath = string.Join("/", resultPathParts);
				resultPathParts.Clear();
			}

			// Collapse multiple forward slashes into a single one
			resultPath = _multipleForwardSlashesRegex.Replace(resultPath, "/");

			return resultPath;
		}

		/// <summary>
		/// Finds a last directory seperator
		/// </summary>
		/// <param name="path">The path of a file or directory</param>
		/// <returns>Position of last directory seperator</returns>
		protected static int FindLastDirectorySeparator(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path",
					string.Format(Strings.Common_ArgumentIsNull, "path"));
			}

			int lastDirectorySeparatorPosition;
			int forwardSlashPosition = path.LastIndexOf('/');
			int backSlashPosition = path.LastIndexOf('\\');

			if (forwardSlashPosition != -1 && backSlashPosition != -1)
			{
				lastDirectorySeparatorPosition = Math.Max(forwardSlashPosition, backSlashPosition);
			}
			else if (forwardSlashPosition != -1)
			{
				lastDirectorySeparatorPosition = forwardSlashPosition;
			}
			else
			{
				lastDirectorySeparatorPosition = backSlashPosition;
			}

			return lastDirectorySeparatorPosition;
		}

		#region IFileManager implementation

		public virtual bool UseCaseSensitiveFileNames
		{
			get;
			set;
		}


		public abstract string GetCurrentDirectory();

		public abstract bool FileExists(string path);

		public abstract bool IsAbsolutePath(string path);

		public virtual string GetDirectoryName(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path",
					string.Format(Strings.Common_ArgumentIsNull, "path"));
			}

			int lastDirectorySeparatorPosition = FindLastDirectorySeparator(path);
			string directoryName = (lastDirectorySeparatorPosition != -1) ?
				path.Substring(0, lastDirectorySeparatorPosition + 1) : string.Empty;

			return directoryName;
		}

		public virtual string GetFileName(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path",
					string.Format(Strings.Common_ArgumentIsNull, "path"));
			}

			int lastDirectorySeparatorPosition = FindLastDirectorySeparator(path);
			string fileName = (lastDirectorySeparatorPosition != -1) ?
				path.Substring(lastDirectorySeparatorPosition + 1) : path;

			return fileName;
		}

		public virtual string GetCanonicalPath(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path",
					string.Format(Strings.Common_ArgumentIsNull, "path"));
			}

			string canonicalPath = path;

			// Convert backslashes to forward slashes
			canonicalPath = BackSlashesToForwardSlashes(canonicalPath);

			// Normalize path
			canonicalPath = NormalizePath(canonicalPath);

			return canonicalPath;
		}

		public virtual string CombinePaths(string basePath, string relativePath)
		{
			if (basePath == null)
			{
				throw new ArgumentNullException("basePath",
					string.Format(Strings.Common_ArgumentIsNull, "basePath"));
			}

			if (relativePath == null)
			{
				throw new ArgumentNullException("relativePath",
					string.Format(Strings.Common_ArgumentIsNull, "relativePath"));
			}

			// Convert backslashes to forward slashes
			string processedBasePath = BackSlashesToForwardSlashes(basePath);
			string processedRelativePath = BackSlashesToForwardSlashes(relativePath);

			if (string.IsNullOrWhiteSpace(processedBasePath))
			{
				return processedRelativePath;
			}

			if (string.IsNullOrWhiteSpace(processedRelativePath))
			{
				return processedBasePath;
			}

			if (IsAbsolutePath(processedRelativePath))
			{
				return processedRelativePath;
			}

			string combinedPath = processedBasePath;

			if (combinedPath.EndsWith("/"))
			{
				if (processedRelativePath.StartsWith("/"))
				{
					processedRelativePath = processedRelativePath.TrimStart('/');
				}
			}
			else
			{
				if (!processedRelativePath.StartsWith("/"))
				{
					combinedPath += '/';
				}
			}

			combinedPath += processedRelativePath;

			// Normalize path
			combinedPath = NormalizePath(combinedPath);

			return combinedPath;
		}

		public virtual string ToAbsolutePath(string relativePath, string currentDirectoryPath)
		{
			if (relativePath == null)
			{
				throw new ArgumentNullException("relativePath",
					string.Format(Strings.Common_ArgumentIsNull, "relativePath"));
			}

			if (currentDirectoryPath == null)
			{
				throw new ArgumentNullException("currentDirectoryPath",
					string.Format(Strings.Common_ArgumentIsNull, "currentDirectoryPath"));
			}

			string absolutePath = IsAbsolutePath(relativePath) ?
				relativePath : CombinePaths(currentDirectoryPath, relativePath);

			// Make a canonical path
			string canonicalAbsolutePath = GetCanonicalPath(absolutePath);

			return canonicalAbsolutePath;
		}

		public virtual string MakeRelativePath(string fromPath, string toPath, string currentDirectoryPath)
		{
			if (fromPath == null)
			{
				throw new ArgumentNullException("fromPath",
					string.Format(Strings.Common_ArgumentIsNull, "fromPath"));
			}

			if (toPath == null)
			{
				throw new ArgumentNullException("toPath",
					string.Format(Strings.Common_ArgumentIsNull, "toPath"));
			}

			if (currentDirectoryPath == null)
			{
				throw new ArgumentNullException("currentDirectoryPath",
					string.Format(Strings.Common_ArgumentIsNull, "currentDirectoryPath"));
			}

			string absoluteFromPath = ToAbsolutePath(fromPath, currentDirectoryPath);
			string absoluteToPath = ToAbsolutePath(toPath, currentDirectoryPath);

			// Absolute path must have a drive letter, and we know that we
			// can only create relative paths if both are on the same drive
			if (absoluteFromPath[0] != absoluteToPath[0])
			{
				return absoluteToPath;
			}

			int pathDifferencePosition = 0;
			int minPathLength = Math.Min(absoluteToPath.Length, absoluteFromPath.Length);
			StringComparison comparisonType = UseCaseSensitiveFileNames ?
				StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

			for (int charIndex = 0; charIndex < minPathLength; charIndex++)
			{
				if (!string.Equals(
					absoluteToPath[charIndex].ToString(),
					absoluteFromPath[charIndex].ToString(),
					comparisonType))
				{
					break;
				}

				if (absoluteToPath[charIndex] == '/')
				{
					pathDifferencePosition = charIndex + 1;
				}
			}

			string strippedFromPath = absoluteFromPath.Substring(pathDifferencePosition);
			string strippedToPath = absoluteToPath.Substring(pathDifferencePosition);

			int leftPosition = 0;
			int rightPosition = 0;
			int directoryCount = 0;

			for (; rightPosition < strippedFromPath.Length; rightPosition++)
			{
				if (strippedFromPath[rightPosition] == '/')
				{
					if (strippedFromPath.Substring(leftPosition, 2) != "..")
					{
						directoryCount++;
					}
					else if (directoryCount > 1)
					{
						directoryCount--;
					}
					else
					{
						directoryCount = 0;
					}

					leftPosition = rightPosition + 1;
				}
			}

			var resultBuilder = new StringBuilder();

			for (int directoryIndex = 0; directoryIndex < directoryCount; directoryIndex++)
			{
				resultBuilder.Append("../");
			}

			resultBuilder.Append(strippedToPath);

			string result = resultBuilder.ToString();
			resultBuilder.Clear();

			return result;
		}

		public abstract string ReadFile(string path);

		#endregion
	}
}