using System.Runtime.InteropServices;

using LibSassHost.Native;

namespace LibSassHost
{
	internal static class FileManagerMarshallingProxy
	{
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool FuncStringBoolean([MarshalAs(UnmanagedType.LPStr)]string p);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		private delegate string FuncString();

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		private delegate string FuncStringString([MarshalAs(UnmanagedType.LPStr)]string p);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		private delegate string FuncStringStringString(
			[MarshalAs(UnmanagedType.LPStr)]string p1,
			[MarshalAs(UnmanagedType.LPStr)]string p2);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		private delegate string FuncStringStringStringString(
			[MarshalAs(UnmanagedType.LPStr)]string p1,
			[MarshalAs(UnmanagedType.LPStr)]string p2,
			[MarshalAs(UnmanagedType.LPStr)]string p3);


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
					GetDirectoryNameDelegate = Marshal.GetFunctionPointerForDelegate(
						(FuncStringString)fileManager.GetDirectoryName),
					GetFileNameDelegate = Marshal.GetFunctionPointerForDelegate(
						(FuncStringString)fileManager.GetFileName),
					GetCanonicalPathDelegate = Marshal.GetFunctionPointerForDelegate(
						(FuncStringString)fileManager.GetCanonicalPath),
					CombinePathsDelegate = Marshal.GetFunctionPointerForDelegate(
						(FuncStringStringString)fileManager.CombinePaths),
					ToAbsolutePathDelegate = Marshal.GetFunctionPointerForDelegate(
						(FuncStringStringString)fileManager.ToAbsolutePath),
					MakeRelativePathDelegate = Marshal.GetFunctionPointerForDelegate(
						(FuncStringStringStringString)fileManager.MakeRelativePath),
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