using System;
using System.Runtime.CompilerServices;

namespace LibSassHost.Utilities
{
	internal static class Utils
	{
		/// <summary>
		/// Flag indicating whether the current runtime is Mono
		/// </summary>
		private static readonly bool _isMonoRuntime;


		/// <summary>
		/// Static constructor
		/// </summary>
		static Utils()
		{
			_isMonoRuntime = Type.GetType("Mono.Runtime") != null;
		}


		/// <summary>
		/// Determines whether the current runtime is Mono
		/// </summary>
		/// <returns>true if the runtime is Mono; otherwise, false</returns>
		public static bool IsMonoRuntime()
		{
			return _isMonoRuntime;
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