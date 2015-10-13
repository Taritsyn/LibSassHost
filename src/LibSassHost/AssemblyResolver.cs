using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

using LibSassHost.Resources;

namespace LibSassHost
{
	/// <summary>
	/// Assembly resolver
	/// </summary>
	internal static class AssemblyResolver
	{
		/// <summary>
		/// Name of directory, that contains the native Sass-compiler assemblies
		/// </summary>
		private const string ASSEMBLY_DIRECTORY_NAME = "LibSassHost.Native";

		/// <summary>
		/// Name of the native Sass-compiler assembly
		/// </summary>
		private const string ASSEMBLY_NAME = "LibSassHost.Native";

		/// <summary>
		/// Regular expression for working with the `bin` directory path
		/// </summary>
		private static readonly Regex _binDirectoryRegex = new Regex(@"\\bin\\?$", RegexOptions.IgnoreCase);

		/// <summary>
		/// Synchronizer of initialization
		/// </summary>
		private static readonly object _initializationSynchronizer = new object();

		/// <summary>
		/// Flag that assembly resolver is initialized
		/// </summary>
		private static bool _initialized;


		/// <summary>
		/// Initialize a assembly resolver
		/// </summary>
		public static void Initialize()
		{
			if (!_initialized)
			{
				lock (_initializationSynchronizer)
				{
					if (!_initialized)
					{
						AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolveHandler;

						_initialized = true;
					}
				}
			}
		}

		private static Assembly AssemblyResolveHandler(object sender, ResolveEventArgs args)
		{
			if (args.Name.StartsWith(ASSEMBLY_NAME, StringComparison.OrdinalIgnoreCase))
			{
				var currentDomain = (AppDomain)sender;
				string platform = Environment.Is64BitProcess ? "64" : "32";

				string binDirectoryPath = currentDomain.SetupInformation.PrivateBinPath;
				if (string.IsNullOrEmpty(binDirectoryPath))
				{
					// `PrivateBinPath` property is empty in test scenarios, so
					// need to use the `BaseDirectory` property
					binDirectoryPath = currentDomain.BaseDirectory;
				}

				string assemblyDirectoryPath = Path.Combine(binDirectoryPath, ASSEMBLY_DIRECTORY_NAME);
				string assemblyFileName = string.Format("{0}-{1}.dll", ASSEMBLY_NAME, platform);
				string assemblyFilePath = Path.Combine(assemblyDirectoryPath, assemblyFileName);

				if (!Directory.Exists(assemblyDirectoryPath))
				{
					if (_binDirectoryRegex.IsMatch(binDirectoryPath))
					{
						string applicationRootPath = _binDirectoryRegex.Replace(binDirectoryPath, string.Empty);
						assemblyDirectoryPath = Path.Combine(applicationRootPath, ASSEMBLY_DIRECTORY_NAME);

						if (!Directory.Exists(assemblyDirectoryPath))
						{
							throw new DirectoryNotFoundException(
								string.Format(Strings.AssembliesDirectoryNotFound, ASSEMBLY_NAME, assemblyDirectoryPath));
						}

						assemblyFilePath = Path.Combine(assemblyDirectoryPath, assemblyFileName);
					}
					else
					{
						throw new DirectoryNotFoundException(
							string.Format(Strings.AssembliesDirectoryNotFound, ASSEMBLY_NAME, assemblyDirectoryPath));
					}
				}

				if (!File.Exists(assemblyFilePath))
				{
					throw new FileNotFoundException(
						string.Format(Strings.AssemblyFileNotFound, ASSEMBLY_NAME, assemblyFilePath));
				}

				return Assembly.LoadFile(assemblyFilePath);
			}

			return null;
		}
	}
}