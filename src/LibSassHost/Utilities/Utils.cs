using System;
#if !NETSTANDARD1_3
using System.Linq;
#endif
using System.Runtime.CompilerServices;
#if NETSTANDARD1_3
using System.Runtime.InteropServices;
#endif

namespace LibSassHost.Utilities
{
	internal static class Utils
	{
#if !NETSTANDARD1_3
		/// <summary>
		/// List of Windows platform identifiers
		/// </summary>
		private static readonly PlatformID[] _windowsPlatformIDs =
		{
			PlatformID.Win32NT,
			PlatformID.Win32S,
			PlatformID.Win32Windows,
			PlatformID.WinCE
		};
#endif


		/// <summary>
		/// Determines whether the current operating system is Windows
		/// </summary>
		/// <returns>true if the operating system is Windows; otherwise, false</returns>
		public static bool IsWindows()
		{
#if NETSTANDARD1_3
			bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
			bool isWindows = _windowsPlatformIDs.Contains(Environment.OSVersion.Platform);
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