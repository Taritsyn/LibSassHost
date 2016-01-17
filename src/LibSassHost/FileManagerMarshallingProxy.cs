using System.Runtime.InteropServices;

using LibSassHost.Native;

namespace LibSassHost
{
	internal static class FileManagerMarshallingProxy
	{
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool FuncStringBoolean([MarshalAs(UnmanagedType.LPWStr)]string p);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.LPWStr)]
		private delegate string FuncString();

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.LPWStr)]
		private delegate string FuncStringString([MarshalAs(UnmanagedType.LPWStr)]string p);

		private static FuncString _getCurrentDirectoryDelegate;

		private static FuncStringBoolean _fileExistsDelegate;

		private static FuncStringBoolean _isAbsolutePathDelegate;

		private static FuncStringString _readFileDelegate;


		public static void SetFileManager(IFileManager fileManager)
		{
			if (fileManager != null)
			{
				_getCurrentDirectoryDelegate = fileManager.GetCurrentDirectory;
				_fileExistsDelegate = fileManager.FileExists;
				_isAbsolutePathDelegate = fileManager.IsAbsolutePath;
				_readFileDelegate = fileManager.ReadFile;

				var delegates = new FileManagerDelegates
				{
					GetCurrentDirectoryDelegate =
						Marshal.GetFunctionPointerForDelegate(_getCurrentDirectoryDelegate),
					FileExistsDelegate =
						Marshal.GetFunctionPointerForDelegate(_fileExistsDelegate),
					IsAbsolutePathDelegate =
						Marshal.GetFunctionPointerForDelegate(_isAbsolutePathDelegate),
					ReadFileDelegate =
						Marshal.GetFunctionPointerForDelegate(_readFileDelegate)
				};
				FileManagerMarshaller.SetDelegates(delegates);
			}
			else
			{
				UnsetFileManager();
			}
		}

		public static void UnsetFileManager()
		{
			FileManagerMarshaller.UnsetDelegates();

			_getCurrentDirectoryDelegate = null;
			_fileExistsDelegate = null;
			_isAbsolutePathDelegate = null;
			_readFileDelegate = null;
		}
	}
}