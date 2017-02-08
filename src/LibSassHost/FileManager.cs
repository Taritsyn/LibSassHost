using System;
using System.IO;
using System.Text.RegularExpressions;

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
		/// Current working directory of the application
		/// </summary>
		private readonly string _currentDirectoryName;


		/// <summary>
		/// Constructs a instance of file manager
		/// </summary>
		public FileManager()
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

			bool result = Utils.IsWindows() ?
				_pathWithDriveLetterRegex.IsMatch(path) : path.StartsWith("/");

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