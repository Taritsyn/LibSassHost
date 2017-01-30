#if !NETSTANDARD1_3
using System;
using System.IO;
using System.Runtime.InteropServices;

using LibSassHost.Helpers;
using LibSassHost.Resources;
using LibSassHost.Utilities;

namespace LibSassHost
{
	/// <summary>
	/// Assembly resolver
	/// </summary>
	internal static class AssemblyResolver
	{
		/// <summary>
		/// Name of the LibSass assembly
		/// </summary>
		private const string ASSEMBLY_NAME = "libsass";

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool SetDllDirectory(string lpPathName);


		/// <summary>
		/// Initialize a assembly resolver
		/// </summary>
		public static void Initialize()
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			string baseDirectoryPath = currentDomain.SetupInformation.PrivateBinPath;
			if (string.IsNullOrEmpty(baseDirectoryPath))
			{
				// `PrivateBinPath` property is empty in test scenarios, so
				// need to use the `BaseDirectory` property
				baseDirectoryPath = currentDomain.BaseDirectory;
			}

			if (!PathHelpers.ContainsDirectory(baseDirectoryPath, "bin"))
			{
				return;
			}

			string platform = Utils.Is64BitProcess() ? "x64" : "x86";

			string assemblyFileName = ASSEMBLY_NAME + ".dll";
			string assemblyDirectoryPath = Path.Combine(baseDirectoryPath, platform);
			string assemblyFilePath = Path.Combine(assemblyDirectoryPath, assemblyFileName);
			bool assemblyFileExists = File.Exists(assemblyFilePath);

			if (!assemblyFileExists)
			{
				return;
			}

			if (!SetDllDirectory(assemblyDirectoryPath))
			{
				throw new InvalidOperationException(
					string.Format(Strings.Runtime_AddingDirectoryToDllSearchPathFailed, assemblyDirectoryPath));
			}
		}
	}
}
#endif