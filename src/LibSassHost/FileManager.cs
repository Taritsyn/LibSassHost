using System;
using System.IO;

using LibSassHost.Helpers;
using LibSassHost.Resources;
using LibSassHost.Utilities;

namespace LibSassHost
{
	/// <summary>
	/// File manager
	/// </summary>
	public sealed class FileManager : IFileManager
	{
		/// <summary>
		/// Instance of file manager
		/// </summary>
		private static readonly Lazy<FileManager> _instance = new Lazy<FileManager>(() => new FileManager());

		/// <summary>
		/// Current working directory of the application
		/// </summary>
		private readonly string _currentDirectoryName;

		/// <summary>
		/// Gets a instance of file manager
		/// </summary>
		public static IFileManager Instance
		{
			get { return _instance.Value; }
		}


		/// <summary>
		/// Private constructor for implementation Singleton pattern
		/// </summary>
		private FileManager()
		{
			_currentDirectoryName = GetDefaultDirectory();
		}


		/// <summary>
		/// Gets a default working directory of the application
		/// </summary>
		/// <returns>The string containing the path of the default working directory</returns>
		private static string GetDefaultDirectory()
		{
			string defaultDirectoryName = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;

			// Convert back slashes to forward slashes
			defaultDirectoryName = PathHelpers.ProcessBackSlashes(defaultDirectoryName);

			return defaultDirectoryName;
		}

		/// <summary>
		/// Determines whether the beginning of specified path matches the drive letter
		/// </summary>
		/// <param name="path">The path</param>
		/// <returns>true if path starts with the drive letter; otherwise, false</returns>
		private static bool PathStartsWithDriveLetter(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path",
					string.Format(Strings.Common_ArgumentIsNull, "path"));
			}

			bool result = path.Length >= 2 && path[0].IsAlpha() && path[1] == ':';

			return result;
		}


		#region IFileManager implementation

		public string GetCurrentDirectory()
		{
			return _currentDirectoryName;
		}

		public bool FileExists(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path",
					string.Format(Strings.Common_ArgumentIsNull, "path"));
			}

			bool result = File.Exists(path);

			return result;
		}

		public bool IsAbsolutePath(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path",
					string.Format(Strings.Common_ArgumentIsNull, "path"));
			}

			if (Utils.IsWindows() && PathStartsWithDriveLetter(path))
			{
				return true;
			}

			int charPosition = 0;
			char charValue;

			// check if we have a protocol
			if (path.TryGetChar(charPosition, out charValue) && charValue.IsAlpha())
			{
				charPosition++;

				// skip over all alphanumeric characters
				while (path.TryGetChar(charPosition, out charValue) && charValue.IsAlphaNumeric())
				{
					charPosition++;
				}

				charPosition = charValue == ':' ? charPosition + 1 : 0;
			}

			path.TryGetChar(charPosition, out charValue);
			bool result = charValue == '/';

			return result;
		}

		public string ToAbsolutePath(string path)
		{
			return path;
		}

		public string ReadFile(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path",
					string.Format(Strings.Common_ArgumentIsNull, "path"));
			}

			string content = File.ReadAllText(path);

			return content;
		}

		#endregion
	}
}