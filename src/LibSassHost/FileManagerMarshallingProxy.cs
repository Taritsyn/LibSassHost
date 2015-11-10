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


		public static void SetFileManager(IFileManager fileManager)
		{
			if (fileManager != null)
			{
				var delegates = new FileManagerDelegates
				{
					GetCurrentDirectoryDelegate = Marshal.GetFunctionPointerForDelegate(
						(FuncString)fileManager.GetCurrentDirectory),
					FileExistsDelegate = Marshal.GetFunctionPointerForDelegate(
						(FuncStringBoolean)fileManager.FileExists),
					IsAbsolutePathDelegate = Marshal.GetFunctionPointerForDelegate(
						(FuncStringBoolean)fileManager.IsAbsolutePath),
					ReadFileDelegate = Marshal.GetFunctionPointerForDelegate(
						(FuncStringString)fileManager.ReadFile)
				};
				FileManagerMarshaller.SetDelegates(delegates);
			}
			else
			{
				FileManagerMarshaller.UnsetDelegates();
			}
		}

		public static void UnsetFileManager()
		{
			FileManagerMarshaller.UnsetDelegates();
		}
	}
}