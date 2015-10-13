using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using LibSassHost.Helpers;

namespace LibSassHost
{
	/// <summary>
	/// File manager
	/// </summary>
	public sealed class FileManager : IFileManager
	{
		/// <summary>
		/// Regular expression for working with the path with a drive letter
		/// </summary>
		private static readonly Regex _pathWithDriveLetterRegex = new Regex(@"^[a-zA-Z]:");

		/// <summary>
		/// Default instance of file manager
		/// </summary>
		private static readonly Lazy<FileManager> _default
			= new Lazy<FileManager>(() => new FileManager());

		/// <summary>
		/// Current instance of file manager
		/// </summary>
		private static IFileManager _current;

		/// <summary>
		/// Gets or sets a instance of file manager
		/// </summary>
		public static IFileManager Current
		{
			get
			{
				return _current ?? _default.Value;
			}
			set
			{
				_current = value;
			}
		}


		/// <summary>
		/// Static constructor
		/// </summary>
		static FileManager()
		{
			AssemblyResolver.Initialize();
		}

		/// <summary>
		/// Constructs a instance of file manager
		/// </summary>
		public FileManager()
		{
			UseCaseSensitiveFileNames = false;
		}


		#region IFileManager implementation

		public bool UseCaseSensitiveFileNames
		{
			get;
			set;
		}


		public string GetCurrentDirectory()
		{
			string currentDirectoryPath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;

			// Convert back slashes to forward slashes
			currentDirectoryPath = PathHelpers.BackSlashesToForwardSlashes(currentDirectoryPath);

			return currentDirectoryPath;
		}

		public bool FileExists(string path)
		{
			return File.Exists(path);
		}

		public bool IsAbsolutePath(string path)
		{
			return _pathWithDriveLetterRegex.IsMatch(path);
		}

		public string GetDirectoryName(string path)
		{
			string directoryName = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar;

			// Convert backslashes to forward slashes
			directoryName = PathHelpers.BackSlashesToForwardSlashes(directoryName);

			return directoryName;
		}

		public string GetFileName(string path)
		{
			return Path.GetFileName(path);
		}

		public string GetCanonicalPath(string path)
		{
			string canonicalPath = path;

			// Convert backslashes to forward slashes
			canonicalPath = PathHelpers.BackSlashesToForwardSlashes(canonicalPath);

			// Normalize path
			canonicalPath = PathHelpers.NormalizePath(canonicalPath);

			return canonicalPath;
		}

		public string CombinePaths(string basePath, string relativePath)
		{
			// Convert backslashes to forward slashes
			string processedBasePath = PathHelpers.BackSlashesToForwardSlashes(basePath);
			string processedRelativePath = PathHelpers.BackSlashesToForwardSlashes(relativePath);

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
			if (!processedBasePath.EndsWith("/"))
			{
				combinedPath += '/';
			}
			combinedPath += processedRelativePath;

			// Normalize path
			combinedPath = PathHelpers.NormalizePath(combinedPath);

			return combinedPath;
		}

		public string ToAbsolutePath(string relativePath, string currentDirectoryPath)
		{
			string absolutePath = IsAbsolutePath(relativePath) ?
				relativePath : CombinePaths(currentDirectoryPath, relativePath);

			// Make a canonical path
			string canonicalAbsolutePath = GetCanonicalPath(absolutePath);

			return canonicalAbsolutePath;
		}

		public string MakeRelativePath(string fromPath, string toPath, string currentDirectoryPath)
		{
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

		public string ReadFile(string path)
		{
			return File.ReadAllText(path);
		}

		#endregion
	}
}