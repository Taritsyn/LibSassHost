#if NET45 || NET471 || NETSTANDARD || NETCOREAPP2_1
using System.Runtime.InteropServices;
#endif
#if NET40

using PolyfillsForOldDotNet.System.Runtime.InteropServices;
#endif

using LibSassHost.Internal.Native;

namespace LibSassHost.Internal
{
	internal static class FileManagerMarshaler
	{
		private static Func_StringAnsi _getCurrentDirectoryDelegateUtf8;
		private static Func_StringAnsi_Boolean _fileExistsDelegateUtf8;
		private static Func_StringAnsi_Boolean _isAbsolutePathDelegateUtf8;
		private static Func_StringAnsi_StringAnsi _toAbsolutePathDelegateUtf8;
		private static Func_StringAnsi_StringAnsi _readFileDelegateUtf8;

		private static Func_StringUni _getCurrentDirectoryDelegateUtf16;
		private static Func_StringUni_Boolean _fileExistsDelegateUtf16;
		private static Func_StringUni_Boolean _isAbsolutePathDelegateUtf16;
		private static Func_StringUni_StringUni _toAbsolutePathDelegateUtf16;
		private static Func_StringUni_StringUni _readFileDelegateUtf16;


		public static void SetFileManager(IFileManager fileManager)
		{
			if (fileManager != null)
			{
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					_getCurrentDirectoryDelegateUtf16 = fileManager.GetCurrentDirectory;
					_fileExistsDelegateUtf16 = fileManager.FileExists;
					_isAbsolutePathDelegateUtf16 = fileManager.IsAbsolutePath;
					_toAbsolutePathDelegateUtf16 = fileManager.ToAbsolutePath;
					_readFileDelegateUtf16 = fileManager.ReadFile;

					Sass_Api.sass_file_manager_set_get_current_directory_delegate_utf16(_getCurrentDirectoryDelegateUtf16);
					Sass_Api.sass_file_manager_set_file_exists_delegate_utf16(_fileExistsDelegateUtf16);
					Sass_Api.sass_file_manager_set_is_absolute_path_delegate_utf16(_isAbsolutePathDelegateUtf16);
					Sass_Api.sass_file_manager_set_to_absolute_path_delegate_utf16(_toAbsolutePathDelegateUtf16);
					Sass_Api.sass_file_manager_set_read_file_delegate_utf16(_readFileDelegateUtf16);
				}
				else
				{
					_getCurrentDirectoryDelegateUtf8 = fileManager.GetCurrentDirectory;
					_fileExistsDelegateUtf8 = fileManager.FileExists;
					_isAbsolutePathDelegateUtf8 = fileManager.IsAbsolutePath;
					_toAbsolutePathDelegateUtf8 = fileManager.ToAbsolutePath;
					_readFileDelegateUtf8 = fileManager.ReadFile;

					Sass_Api.sass_file_manager_set_get_current_directory_delegate_utf8(_getCurrentDirectoryDelegateUtf8);
					Sass_Api.sass_file_manager_set_file_exists_delegate_utf8(_fileExistsDelegateUtf8);
					Sass_Api.sass_file_manager_set_is_absolute_path_delegate_utf8(_isAbsolutePathDelegateUtf8);
					Sass_Api.sass_file_manager_set_to_absolute_path_delegate_utf8(_toAbsolutePathDelegateUtf8);
					Sass_Api.sass_file_manager_set_read_file_delegate_utf8(_readFileDelegateUtf8);
				}

				Sass_Api.sass_file_manager_set_supports_conversion_to_absolute_path(
					fileManager.SupportsConversionToAbsolutePath);
				Sass_Api.sass_file_manager_set_is_initialized(true);
			}
			else
			{
				UnsetFileManager();
			}
		}

		public static void UnsetFileManager()
		{
			if (!Sass_Api.sass_file_manager_get_is_initialized())
			{
				return;
			}

			Sass_Api.sass_file_manager_set_is_initialized(false);
			Sass_Api.sass_file_manager_set_supports_conversion_to_absolute_path(false);
			Sass_Api.sass_file_manager_unset_get_current_directory_delegate();
			Sass_Api.sass_file_manager_unset_file_exists_delegate();
			Sass_Api.sass_file_manager_unset_is_absolute_path_delegate();
			Sass_Api.sass_file_manager_unset_to_absolute_path_delegate();
			Sass_Api.sass_file_manager_unset_read_file_delegate();

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				_getCurrentDirectoryDelegateUtf16 = null;
				_fileExistsDelegateUtf16 = null;
				_isAbsolutePathDelegateUtf16 = null;
				_toAbsolutePathDelegateUtf16 = null;
				_readFileDelegateUtf16 = null;
			}
			else
			{
				_getCurrentDirectoryDelegateUtf8 = null;
				_fileExistsDelegateUtf8 = null;
				_isAbsolutePathDelegateUtf8 = null;
				_toAbsolutePathDelegateUtf8 = null;
				_readFileDelegateUtf8 = null;
			}
		}
	}
}