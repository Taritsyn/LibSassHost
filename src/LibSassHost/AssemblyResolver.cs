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
		/// Prefix of the native Sass-compiler assembly names
		/// </summary>
		private const string ASSEMBLY_NAME_PREFIX = "LibSassHost.Native-";

		/// <summary>
		/// Name of the native Sass-compiler proxy assembly
		/// </summary>
		private const string PROXY_ASSEMBLY_NAME = ASSEMBLY_NAME_PREFIX + "Proxy";

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
						AppDomain currentDomain = AppDomain.CurrentDomain;
						string binDirectoryPath = GetBinDirectoryPath(currentDomain);

						string proxyAssemblyFilePath = Path.Combine(binDirectoryPath, PROXY_ASSEMBLY_NAME + ".dll");
						if (File.Exists(proxyAssemblyFilePath))
						{
							throw new InvalidOperationException(
								string.Format(Strings.ProxyAssemblyFileFound, PROXY_ASSEMBLY_NAME, ASSEMBLY_NAME_PREFIX));
						}

						currentDomain.AssemblyResolve += AssemblyResolveHandler;

						_initialized = true;
					}
				}
			}
		}

		private static Assembly AssemblyResolveHandler(object sender, ResolveEventArgs args)
		{
			if (args.Name.StartsWith(PROXY_ASSEMBLY_NAME, StringComparison.OrdinalIgnoreCase))
			{
				var currentDomain = (AppDomain)sender;
				string platform = Environment.Is64BitProcess ? "64" : "32";
				string binDirectoryPath = GetBinDirectoryPath(currentDomain);

				string assemblyDirectoryPath = Path.Combine(binDirectoryPath, ASSEMBLY_DIRECTORY_NAME);
				string assemblyFileName = string.Format("{0}{1}.dll", ASSEMBLY_NAME_PREFIX, platform);
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
								string.Format(Strings.AssembliesDirectoryNotFound, ASSEMBLY_NAME_PREFIX, assemblyDirectoryPath));
						}

						assemblyFilePath = Path.Combine(assemblyDirectoryPath, assemblyFileName);
					}
					else
					{
						throw new DirectoryNotFoundException(
							string.Format(Strings.AssembliesDirectoryNotFound, ASSEMBLY_NAME_PREFIX, assemblyDirectoryPath));
					}
				}

				if (!File.Exists(assemblyFilePath))
				{
					throw new FileNotFoundException(
						string.Format(Strings.AssemblyFileNotFound, ASSEMBLY_NAME_PREFIX, assemblyFilePath));
				}

				return Assembly.LoadFile(assemblyFilePath);
			}

			return null;
		}

		private static string GetBinDirectoryPath(AppDomain currentDomain)
		{
			string binDirectoryPath = currentDomain.SetupInformation.PrivateBinPath;
			if (string.IsNullOrEmpty(binDirectoryPath))
			{
				// `PrivateBinPath` property is empty in test scenarios, so
				// need to use the `BaseDirectory` property
				binDirectoryPath = currentDomain.BaseDirectory;
			}

			return binDirectoryPath;
		}
	}
}