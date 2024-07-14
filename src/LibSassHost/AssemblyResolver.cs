﻿#if NETFRAMEWORK
using System;
using System.IO;
using System.Runtime.InteropServices;
#if NET40

using PolyfillsForOldDotNet.System.Runtime.InteropServices;
#endif

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

			Architecture architecture = RuntimeInformation.OSArchitecture;
			string platform;

			if (architecture == Architecture.X64 || architecture == Architecture.X86)
			{
				platform = Utils.Is64BitProcess() ? "x64" : "x86";
			}
			else if (architecture == Architecture.Arm64 || architecture == Architecture.Arm)
			{
				platform = Utils.Is64BitProcess() ? "arm64" : "arm";
			}
			else
			{
				return;
			}

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
					string.Format(Strings.Compiler_AddingDirectoryToDllSearchPathFailed, assemblyDirectoryPath));
			}
		}
	}
}
#endif