namespace LibSassHost
{
	/// <summary>
	/// Defines a interface of file manager
	/// </summary>
	public interface IFileManager
	{
		/// <summary>
		/// Gets or sets a flag for whether to use case-sensitive file names
		/// </summary>
		bool UseCaseSensitiveFileNames { get; set; }


		/// <summary>
		/// Gets a current working directory of the application
		/// </summary>
		/// <returns>The string containing the path of the current working directory</returns>
		string GetCurrentDirectory();

		/// <summary>
		/// Determines whether the specified file exists
		/// </summary>
		/// <param name="path">The file to check</param>
		/// <returns>true if the caller has the required permissions and path contains
		/// the name of an existing file; otherwise, false</returns>
		bool FileExists(string path);

		/// <summary>
		/// Gets a boolean value indicating whether the specified path is absolute
		/// </summary>
		/// <param name="path">The path to check</param>
		/// <returns>true if path is an absolute path; otherwise, false</returns>
		bool IsAbsolutePath(string path);

		/// <summary>
		/// Gets a directory name for the specified path string
		/// </summary>
		/// <param name="path">The path of a file or directory</param>
		/// <returns>The string containing directory name for path</returns>
		string GetDirectoryName(string path);

		/// <summary>
		/// Gets a file name and extension of the specified path string
		/// </summary>
		/// <param name="path">The path string from which to obtain the file name and extension</param>
		/// <returns>The consisting of the characters after the last directory character in path</returns>
		string GetFileName(string path);

		/// <summary>
		/// Gets a logical clean path
		/// </summary>
		/// <param name="path">The path of a file or directory</param>
		/// <returns>The logical clean path</returns>
		string GetCanonicalPath(string path);

		/// <summary>
		/// Combines a base path with a relative path to return a complete path to a file
		/// </summary>
		/// <param name="basePath">The base path</param>
		/// <param name="relativePath">The path to the file, relative to the base path</param>
		/// <returns>The complete path to a file</returns>
		string CombinePaths(string basePath, string relativePath);

		/// <summary>
		/// Converts a relative path to an application absolute path using the specified application path
		/// </summary>
		/// <param name="relativePath">The relative path</param>
		/// <param name="currentDirectoryPath">The path of the current working directory</param>
		/// <returns>The absolute path representation of relative path</returns>
		string ToAbsolutePath(string relativePath, string currentDirectoryPath);

		/// <summary>
		/// Returns a path that is relative to the given base path and base will first be
		/// resolved against current working directory to make them absolute
		/// </summary>
		/// <param name="fromPath">The starting path to return the relative path from</param>
		/// <param name="toPath">The ending path to return the relative path to</param>
		/// <param name="currentDirectoryPath">The path of the current working directory</param>
		/// <returns>The relative path from <paramref name="fromPath" /> to <paramref name="toPath" /></returns>
		string MakeRelativePath(string fromPath, string toPath, string currentDirectoryPath);

		/// <summary>
		/// Opens a file, reads all lines of the file, and then closes the file
		/// </summary>
		/// <param name="path">The file to open for reading</param>
		/// <returns>The string containing all lines of the file</returns>
		string ReadFile(string path);
	}
}