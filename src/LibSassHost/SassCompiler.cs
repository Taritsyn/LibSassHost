using System;
using System.IO;
using System.Linq;
#if NET45_OR_GREATER || NETSTANDARD
using System.Runtime.InteropServices;
#endif
using System.Text;
using System.Threading;

using AdvancedStringBuilder;
#if NET40
using PolyfillsForOldDotNet.System.Runtime.InteropServices;
#endif

using LibSassHost.Constants;
using LibSassHost.Helpers;
using LibSassHost.Internal;
using LibSassHost.Resources;
using LibSassHost.Utilities;

namespace LibSassHost
{
	/// <summary>
	/// Sass compiler
	/// </summary>
	public static class SassCompiler
	{
		/// <summary>
		/// Version of the LibSass library
		/// </summary>
		private static string _version;

		/// <summary>
		/// Version of Sass language
		/// </summary>
		private static string _languageVersion;

		/// <summary>
		/// Default compilation options
		/// </summary>
		private static readonly CompilationOptions _defaultOptions = new CompilationOptions();

		/// <summary>
		/// Instance of file manager
		/// </summary>
		private static IFileManager _fileManager;

		/// <summary>
		/// Synchronizer of Sass compiler initialization
		/// </summary>
		private static readonly object _initializationSynchronizer = new object();

		/// <summary>
		/// Flag indicating whether the Sass compiler is initialized
		/// </summary>
		private static bool _initialized;

		/// <summary>
		/// Instance of mutex
		/// </summary>
		private static Mutex _mutex = new Mutex();

		/// <summary>
		/// Gets a version of the LibSass library
		/// </summary>
		public static string Version
		{
			get
			{
				Initialize();

				return _version;
			}
		}

		/// <summary>
		/// Gets a version of Sass language
		/// </summary>
		public static string LanguageVersion
		{
			get
			{
				Initialize();

				return _languageVersion;
			}
		}

		/// <summary>
		/// Gets or sets a file manager
		/// </summary>
		public static IFileManager FileManager
		{
			get { return _fileManager; }
			set { _fileManager = value; }
		}
#if NETFRAMEWORK

		/// <summary>
		/// Static constructor
		/// </summary>
		/// <exception cref="SassCompilerLoadException">Failed to load a Sass-compiler.</exception>
		static SassCompiler()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				try
				{
					AssemblyResolver.Initialize();
				}
				catch (InvalidOperationException e)
				{
					throw SassErrorHelpers.WrapCompilerLoadException(e);
				}
			}
		}
#endif


		/// <summary>
		/// Initializes a Sass compiler
		/// </summary>
		private static void Initialize()
		{
			if (_initialized)
			{
				return;
			}

			lock (_initializationSynchronizer)
			{
				if (_initialized)
				{
					return;
				}

				try
				{
					_version = SassCompilerProxy.GetVersion();
					_languageVersion = SassCompilerProxy.GetLanguageVersion();
				}
				catch (DllNotFoundException e)
				{
					throw WrapDllNotFoundException(e);
				}
#if NETSTANDARD1_3
				catch (TypeLoadException e)
#else
				catch (EntryPointNotFoundException e)
#endif
				{
					string message = e.Message;
					if (message.ContainsQuotedValue(DllName.Universal)
						&& (message.ContainsQuotedValue("libsass_version") || message.ContainsQuotedValue("libsass_language_version")))
					{
						_version = "0.0.0";
						_languageVersion = "0.0";
					}
					else
					{
						throw SassErrorHelpers.WrapCompilerLoadException(e, true);
					}
				}
				catch (Exception e)
				{
					throw SassErrorHelpers.WrapCompilerLoadException(e, true);
				}

				_initialized = true;
			}
		}

		/// <summary>
		/// "Compiles" a Sass code to CSS code
		/// </summary>
		/// <param name="content">Text content written on Sass</param>
		/// <param name="options">Compilation options</param>
		/// <returns>Compilation result</returns>
		/// <exception cref="ArgumentException"/>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="SassCompilationException">Sass compilation error.</exception>
		public static CompilationResult Compile(string content, CompilationOptions options = null)
		{
			if (content == null)
			{
				throw new ArgumentNullException(
					nameof(content),
					string.Format(Strings.Common_ArgumentIsNull, nameof(content))
				);
			}

			if (string.IsNullOrWhiteSpace(content))
			{
				throw new ArgumentException(
					string.Format(Strings.Common_ArgumentIsEmpty, nameof(content)),
					nameof(content)
				);
			}

			return InnerCompile(content, false, null, null, null, options);
		}

		/// <summary>
		/// "Compiles" a Sass code to CSS code
		/// </summary>
		/// <param name="content">Text content written on Sass</param>
		/// <param name="indentedSyntax">Flag for whether to enable Sass Indented Syntax
		/// for parsing the data string</param>
		/// <param name="options">Compilation options</param>
		/// <returns>Compilation result</returns>
		/// <exception cref="ArgumentException"/>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="SassCompilationException">Sass compilation error.</exception>
		public static CompilationResult Compile(string content, bool indentedSyntax, CompilationOptions options = null)
		{
			if (content == null)
			{
				throw new ArgumentNullException(
					nameof(content),
					string.Format(Strings.Common_ArgumentIsNull, nameof(content))
				);
			}

			if (string.IsNullOrWhiteSpace(content))
			{
				throw new ArgumentException(
					string.Format(Strings.Common_ArgumentIsEmpty, nameof(content)),
					nameof(content)
				);
			}

			return InnerCompile(content, indentedSyntax, null, null, null, options);
		}

		/// <summary>
		/// "Compiles" a Sass code to CSS code
		/// </summary>
		/// <param name="content">Text content written on Sass</param>
		/// <param name="inputPath">Path to input file</param>
		/// <param name="outputPath">Path to output file</param>
		/// <param name="sourceMapPath">Path to source map file</param>
		/// <param name="options">Compilation options</param>
		/// <returns>Compilation result</returns>
		/// <exception cref="ArgumentException"/>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="SassCompilationException">Sass compilation error.</exception>
		public static CompilationResult Compile(string content, string inputPath, string outputPath = null,
			string sourceMapPath = null, CompilationOptions options = null)
		{
			if (content == null)
			{
				throw new ArgumentNullException(
					nameof(content),
					string.Format(Strings.Common_ArgumentIsNull, nameof(content))
				);
			}

			if (inputPath == null)
			{
				throw new ArgumentNullException(
					nameof(inputPath),
					string.Format(Strings.Common_ArgumentIsNull, nameof(inputPath))
				);
			}

			if (string.IsNullOrWhiteSpace(content))
			{
				throw new ArgumentException(
					string.Format(Strings.Common_ArgumentIsEmpty, nameof(content)),
					nameof(content)
				);
			}

			if (string.IsNullOrWhiteSpace(inputPath))
			{
				throw new ArgumentException(
					string.Format(Strings.Common_ArgumentIsEmpty, nameof(inputPath)),
					nameof(inputPath)
				);
			}

			bool indentedSyntax = GetIndentedSyntax(inputPath);

			return InnerCompile(content, indentedSyntax, inputPath, outputPath, sourceMapPath, options);
		}

		private static CompilationResult InnerCompile(string content, bool indentedSyntax, string inputPath,
			string outputPath, string sourceMapPath, CompilationOptions options)
		{
			Initialize();

			CompilationResult result;
			var dataContext = new SassDataContext
			{
				SourceString = content
			};

			BeginCompile(dataContext, indentedSyntax, inputPath, outputPath, sourceMapPath, options);

			try
			{
				_mutex.WaitOne();
				FileManagerMarshaler.SetFileManager(_fileManager);
				SassCompilerProxy.Compile(dataContext);
			}
			finally
			{
				FileManagerMarshaler.UnsetFileManager();
				_mutex.ReleaseMutex();
			}

			result = EndCompile(dataContext);

			return result;
		}

		/// <summary>
		/// "Compiles" a Sass file to CSS code
		/// </summary>
		/// <param name="inputPath">Path to input file</param>
		/// <param name="outputPath">Path to output file</param>
		/// <param name="sourceMapPath">Path to source map file</param>
		/// <param name="options">Compilation options</param>
		/// <returns>Compilation result</returns>
		/// <exception cref="ArgumentException"/>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="SassCompilationException">Sass compilation error.</exception>
		public static CompilationResult CompileFile(string inputPath, string outputPath = null,
			string sourceMapPath = null, CompilationOptions options = null)
		{
			if (inputPath == null)
			{
				throw new ArgumentNullException(
					nameof(inputPath),
					string.Format(Strings.Common_ArgumentIsNull, nameof(inputPath))
				);
			}

			if (string.IsNullOrWhiteSpace(inputPath))
			{
				throw new ArgumentException(
					string.Format(Strings.Common_ArgumentIsEmpty, nameof(inputPath)),
					nameof(inputPath)
				);
			}

			Initialize();

			IFileManager fileManager = _fileManager;
			if (fileManager != null && !fileManager.FileExists(inputPath))
			{
				string description = string.Format("File to read not found or unreadable: {0}", inputPath);
				string message = string.Format("Internal Error: {0}", description);

				throw new SassCompilationException(message)
				{
					ErrorCode = 3,
					Description = description,
					File = null,
					LineNumber = -1,
					ColumnNumber = -1,
					SourceFragment = null
				};
			}

			CompilationResult result;
			var fileContext = new SassFileContext();
			bool indentedSyntax = GetIndentedSyntax(inputPath);

			BeginCompile(fileContext, indentedSyntax, inputPath, outputPath, sourceMapPath, options);

			try
			{
				_mutex.WaitOne();
				FileManagerMarshaler.SetFileManager(fileManager);
				SassCompilerProxy.CompileFile(fileContext);
			}
			finally
			{
				FileManagerMarshaler.UnsetFileManager();
				_mutex.ReleaseMutex();
			}

			GC.KeepAlive(fileManager);

			result = EndCompile(fileContext);

			return result;
		}

		private static void BeginCompile(SassContextBase context, bool indentedSyntax, string inputPath,
			string outputPath, string sourceMapPath, CompilationOptions options)
		{
			options = options ?? _defaultOptions;

			string inputFilePath = !string.IsNullOrWhiteSpace(inputPath) ? inputPath : string.Empty;
			string outputFilePath = !string.IsNullOrWhiteSpace(outputPath) ? outputPath : string.Empty;
			string sourceMapFilePath = !string.IsNullOrWhiteSpace(sourceMapPath) ? sourceMapPath : string.Empty;

			if (inputFilePath.Length > 0 || outputFilePath.Length > 0)
			{
				outputFilePath = outputFilePath.Length > 0 ?
					outputPath : Path.ChangeExtension(inputFilePath, ".css");
				sourceMapFilePath = sourceMapFilePath.Length > 0 ?
					sourceMapFilePath : Path.ChangeExtension(outputFilePath, ".css.map");
			}

			context.InputPath = inputFilePath;
			context.IsIndentedSyntaxSource = indentedSyntax;
			context.Options = new SassOptions
			{
				IncludePath = string.Join(";", options.IncludePaths),
				Indent = GetIndentString(options.IndentType, options.IndentWidth),
				LineFeed = GetLineFeedString(options.LineFeedType),
				OutputStyle = options.OutputStyle,
				Precision = options.Precision,
				OmitSourceMapUrl = options.OmitSourceMapUrl,
				SourceMapContents = options.SourceMapIncludeContents,
				SourceMapEmbed = options.InlineSourceMap,
				SourceMapFileUrls = options.SourceMapFileUrls,
				SourceMapRoot = options.SourceMapRootPath,
				SourceComments = options.SourceComments
			};
			context.OutputPath = outputFilePath;
			context.SourceMapFile = options.SourceMap ? sourceMapFilePath : string.Empty;
		}

		private static CompilationResult EndCompile(SassContextBase context)
		{
			CompilationResult result;
			SassErrorInfo error = context.Error;

			if (error == null)
			{
				result = new CompilationResult
				{
					CompiledContent = context.OutputString,
					IncludedFilePaths = context.IncludedFiles.ToList(),
					SourceMap = context.SourceMapString
				};
			}
			else
			{
				string message = error.Message.TrimEnd();
				string sourceCode = error.Source;
				int lineNumber = error.Line;
				int columnNumber = error.Column;
				string sourceFragment = SourceCodeNavigator.GetSourceFragment(sourceCode,
					new SourceCodeNodeCoordinates(lineNumber, columnNumber));

				throw new SassCompilationException(message)
				{
					ErrorCode = error.Status,
					Description = error.Text,
					File = error.File,
					LineNumber = lineNumber,
					ColumnNumber = columnNumber,
					SourceFragment = sourceFragment
				};
			}

			return result;
		}

		private static bool GetIndentedSyntax(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}

			string fileExtension = Path.GetExtension(path);
			bool indentedSyntax = string.Equals(fileExtension, ".SASS", StringComparison.OrdinalIgnoreCase);

			return indentedSyntax;
		}

		private static string GetIndentString(IndentType type, int width)
		{
			char character = type == IndentType.Tab ? '\t' : ' ';
			string indent = new string(character, width);

			return indent;
		}

		private static string GetLineFeedString(LineFeedType type)
		{
			string lineFeed;

			switch (type)
			{
				case LineFeedType.Cr:
					lineFeed = "\r";
					break;
				case LineFeedType.CrLf:
					lineFeed = "\r\n";
					break;
				case LineFeedType.Lf:
					lineFeed = "\n";
					break;
				case LineFeedType.LfCr:
					lineFeed = "\n\r";
					break;
				default:
					throw new NotSupportedException();
			}

			return lineFeed;
		}

		private static SassCompilerLoadException WrapDllNotFoundException(
			DllNotFoundException originalDllNotFoundException)
		{
			string originalMessage = originalDllNotFoundException.Message;
			string description;
			string message;
			bool isMonoRuntime = Utils.IsMonoRuntime();

			if ((isMonoRuntime && originalMessage == DllName.Universal)
				|| originalMessage.ContainsQuotedValue(DllName.Universal))
			{
				const string buildInstructionsUrl = "https://github.com/Taritsyn/LibSassHost#{0}";
				const string manualInstallationInstructionsUrl = "https://github.com/Taritsyn/LibSassHost#{0}";
				Architecture osArchitecture = RuntimeInformation.OSArchitecture;

				var stringBuilderPool = StringBuilderPool.Shared;
				StringBuilder descriptionBuilder = stringBuilderPool.Rent();
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					descriptionBuilder.AppendFormat(Strings.Compiler_AssemblyNotFound, DllName.ForWindows);
					descriptionBuilder.Append(" ");
					if (osArchitecture == Architecture.X64 || osArchitecture == Architecture.X86)
					{
						descriptionBuilder.AppendFormat(Strings.Compiler_NuGetPackageInstallationRequired,
							Utils.Is64BitProcess() ?
								"LibSassHost.Native.win-x64"
								:
								"LibSassHost.Native.win-x86"
						);
					}
					else
					{
						descriptionBuilder.AppendFormat(Strings.Compiler_NoNuGetPackageForProcessorArchitecture,
							"LibSassHost.Native.win-*",
							osArchitecture.ToString().ToLowerInvariant()
						);
						descriptionBuilder.Append(" ");
						descriptionBuilder.AppendFormat(Strings.Compiler_BuildNativeAssemblyForCurrentProcessorArchitecture,
							DllName.ForWindows,
							string.Format(buildInstructionsUrl, "windows")
						);
					}
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				{
					descriptionBuilder.AppendFormat(Strings.Compiler_AssemblyNotFound, DllName.ForLinux);
					descriptionBuilder.Append(" ");
					if (isMonoRuntime)
					{
						descriptionBuilder.AppendFormat(Strings.Compiler_ManualInstallationUnderMonoRequired,
							"LibSassHost.Native.linux-*",
							string.Format(manualInstallationInstructionsUrl, "linux")
						);
					}
					else
					{
						if (osArchitecture == Architecture.X64)
						{
							descriptionBuilder.AppendFormat(Strings.Compiler_NuGetPackageInstallationRequired,
								"LibSassHost.Native.linux-x64");
						}
						else
						{
							descriptionBuilder.AppendFormat(Strings.Compiler_NoNuGetPackageForProcessorArchitecture,
								"LibSassHost.Native.linux-*",
								osArchitecture.ToString().ToLowerInvariant()
							);
							descriptionBuilder.Append(" ");
							descriptionBuilder.AppendFormat(Strings.Compiler_BuildNativeAssemblyForCurrentProcessorArchitecture,
								DllName.ForLinux,
								string.Format(buildInstructionsUrl, "linux-1")
							);
						}
					}
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					descriptionBuilder.AppendFormat(Strings.Compiler_AssemblyNotFound, DllName.ForOsx);
					descriptionBuilder.Append(" ");
					if (isMonoRuntime)
					{
						descriptionBuilder.AppendFormat(Strings.Compiler_ManualInstallationUnderMonoRequired,
							"LibSassHost.Native.osx-*",
							string.Format(manualInstallationInstructionsUrl, "os-x")
						);
					}
					else
					{
						if (osArchitecture == Architecture.X64)
						{
							descriptionBuilder.AppendFormat(Strings.Compiler_NuGetPackageInstallationRequired,
								"LibSassHost.Native.osx-x64");
						}
						else
						{
							descriptionBuilder.AppendFormat(Strings.Compiler_NoNuGetPackageForProcessorArchitecture,
								"LibSassHost.Native.osx-*",
								osArchitecture.ToString().ToLowerInvariant()
							);
							descriptionBuilder.Append(" ");
							descriptionBuilder.AppendFormat(Strings.Compiler_BuildNativeAssemblyForCurrentProcessorArchitecture,
								DllName.ForOsx,
								string.Format(buildInstructionsUrl, "os-x-1")
							);
						}
					}
				}
				else
				{
					descriptionBuilder.Append(Strings.Compiler_OperatingSystemNotSupported);
				}

				description = descriptionBuilder.ToString();
				stringBuilderPool.Return(descriptionBuilder);

				message = SassErrorHelpers.GenerateCompilerLoadErrorMessage(description);
			}
			else
			{
				description = originalMessage;
				message = SassErrorHelpers.GenerateCompilerLoadErrorMessage(description, true);
			}

			var wrapperEngineLoadException = new SassCompilerLoadException(message, originalDllNotFoundException)
			{
				Description = description
			};

			return wrapperEngineLoadException;
		}
	}
}