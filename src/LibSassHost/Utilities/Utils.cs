using System;
#if !NETSTANDARD1_3 && !NETSTANDARD2_0
using System.Linq;
#endif
using System.Runtime.CompilerServices;
#if NETSTANDARD1_3 || NETSTANDARD2_0
using System.Runtime.InteropServices;
#endif

namespace LibSassHost.Utilities
{
	internal static class Utils
	{
		/// <summary>
		/// Flag indicating whether the current operating system is Windows
		/// </summary>
		private static readonly bool _isWindows;


		/// <summary>
		/// Static constructor
		/// </summary>
		static Utils()
		{
			_isWindows = InnerIsWindows();
		}


		/// <summary>
		/// Determines whether the current operating system is Windows
		/// </summary>
		/// <returns>true if the operating system is Windows; otherwise, false</returns>
		public static bool IsWindows()
		{
			return _isWindows;
		}

		private static bool InnerIsWindows()
		{
#if NETSTANDARD1_3 || NETSTANDARD2_0
			bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
			PlatformID[] windowsPlatformIDs =
			{
				PlatformID.Win32NT,
				PlatformID.Win32S,
				PlatformID.Win32Windows,
				PlatformID.WinCE
			};
			bool isWindows = windowsPlatformIDs.Contains(Environment.OSVersion.Platform);
#endif

			return isWindows;
		}

		/// <summary>
		/// Determines whether the current process is a 64-bit process
		/// </summary>
		/// <returns>true if the process is 64-bit; otherwise, false</returns>
		[MethodImpl((MethodImplOptions)256 /* AggressiveInlining */)]
		public static bool Is64BitProcess()
		{
#if NETSTANDARD1_3
			bool is64Bit = IntPtr.Size == 8;
#else
			bool is64Bit = Environment.Is64BitProcess;
#endif

			return is64Bit;
		}
	}
}