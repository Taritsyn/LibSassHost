namespace LibSassHost
{
	/// <summary>
	/// Defines a interface of file manager
	/// </summary>
	public interface IFileManager
	{
		/// <summary>
		/// Gets a value that indicates if the file manager supports conversion of
		/// relative paths to an application absolute paths
		/// </summary>
		bool SupportsConversionToAbsolutePath
		{
			get;
		}


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
		/// Converts a relative path to an application absolute path
		/// </summary>
		/// <param name="path">The relative path</param>
		/// <returns>The absolute path representation of the specified relative path</returns>
		string ToAbsolutePath(string path);

		/// <summary>
		/// Opens a file, reads all lines of the file, and then closes the file
		/// </summary>
		/// <param name="path">The file to open for reading</param>
		/// <returns>The string containing all lines of the file</returns>
		string ReadFile(string path);
	}
}