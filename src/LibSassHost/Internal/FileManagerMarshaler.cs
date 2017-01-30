using LibSassHost.Internal.Native;

namespace LibSassHost.Internal
{
	internal static class FileManagerMarshaler
	{
		private static Func_String _getCurrentDirectoryDelegate;
		private static Func_String_Boolean _fileExistsDelegate;
		private static Func_String_Boolean _isAbsolutePathDelegate;
		private static Func_String_String _toAbsolutePathDelegate;
		private static Func_String_String _readFileDelegate;


		public static void SetFileManager(IFileManager fileManager)
		{
			if (fileManager != null)
			{
				_getCurrentDirectoryDelegate = fileManager.GetCurrentDirectory;
				_fileExistsDelegate = fileManager.FileExists;
				_isAbsolutePathDelegate = fileManager.IsAbsolutePath;
				_toAbsolutePathDelegate = fileManager.ToAbsolutePath;
				_readFileDelegate = fileManager.ReadFile;

				Sass_Api.sass_file_manager_set_get_current_directory_delegate(_getCurrentDirectoryDelegate);
				Sass_Api.sass_file_manager_set_file_exists_delegate(_fileExistsDelegate);
				Sass_Api.sass_file_manager_set_is_absolute_path_delegate(_isAbsolutePathDelegate);
				Sass_Api.sass_file_manager_set_to_absolute_path_delegate(_toAbsolutePathDelegate);
				Sass_Api.sass_file_manager_set_read_file_delegate(_readFileDelegate);
			}
			else
			{
				UnsetFileManager();
			}
		}

		public static void UnsetFileManager()
		{
			Sass_Api.sass_file_manager_unset_get_current_directory_delegate();
			Sass_Api.sass_file_manager_unset_file_exists_delegate();
			Sass_Api.sass_file_manager_unset_is_absolute_path_delegate();
			Sass_Api.sass_file_manager_unset_to_absolute_path_delegate();
			Sass_Api.sass_file_manager_unset_read_file_delegate();

			_getCurrentDirectoryDelegate = null;
			_fileExistsDelegate = null;
			_isAbsolutePathDelegate = null;
			_toAbsolutePathDelegate = null;
			_readFileDelegate = null;
		}
	}
}