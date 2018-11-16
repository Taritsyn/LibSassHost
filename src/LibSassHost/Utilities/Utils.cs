using System;
#if NET40
using System.Diagnostics;
#endif
using System.Runtime.CompilerServices;
#if NET40

using LibSassHost.Resources;
#endif

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
#if NET40

		public static string ReadProcessOutput(string fileName)
		{
			return ReadProcessOutput(fileName, string.Empty);
		}

		public static string ReadProcessOutput(string fileName, string args)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException(
					nameof(fileName),
					string.Format(Strings.Common_ArgumentIsNull, nameof(fileName))
				);
			}

			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentException(
					string.Format(Strings.Common_ArgumentIsEmpty, nameof(fileName)),
					nameof(fileName)
				);
			}

			string output;
			var processInfo = new ProcessStartInfo
			{
				FileName = fileName,
				Arguments = args ?? string.Empty,
				UseShellExecute = false,
				RedirectStandardOutput = true
			};

			using (Process process = Process.Start(processInfo))
			{
				output = process.StandardOutput.ReadToEnd();
				process.WaitForExit();
			}

			return output;
		}
#endif
	}
}