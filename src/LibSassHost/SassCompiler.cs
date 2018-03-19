using System;
using System.IO;
using System.Linq;

using LibSassHost.Internal;
using LibSassHost.Resources;
#if !NETSTANDARD
using LibSassHost.Utilities;
#endif

namespace LibSassHost
{
	/// <summary>
	/// Sass-compiler
	/// </summary>
	public static class SassCompiler
	{
		/// <summary>
		/// Version of the LibSass library
		/// </summary>
		private static readonly string _version;

		/// <summary>
		/// Version of Sass language
		/// </summary>
		private static readonly string _languageVersion;

		/// <summary>
		/// Default compilation options
		/// </summary>
		private static readonly CompilationOptions _defaultOptions = new CompilationOptions();

		/// <summary>
		/// Instance of file manager
		/// </summary>
		private static IFileManager _fileManager;

		/// <summary>
		/// Synchronizer of file manager
		/// </summary>
		private static readonly object _fileManagerSynchronizer = new object();

		/// <summary>
		/// Gets a version of the LibSass library
		/// </summary>
		public static string Version
		{
			get { return _version; }
		}

		/// <summary>
		/// Gets a version of Sass language
		/// </summary>
		public static string LanguageVersion
		{
			get { return _languageVersion; }
		}

		/// <summary>
		/// Gets or sets a file manager
		/// </summary>
		public static IFileManager FileManager
		{
			get
			{
				lock (_fileManagerSynchronizer)
				{
					return _fileManager;
				}
			}
			set
			{
				lock (_fileManagerSynchronizer)
				{
					_fileManager = value;
					FileManagerMarshaler.SetFileManager(_fileManager);
				}
			}
		}


		/// <summary>
		/// Static constructor
		/// </summary>
		/// <exception cref="SassCompilerLoadException">Failed to load a Sass-compiler.</exception>
		static SassCompiler()
		{
#if !NETSTANDARD
			if (Utils.IsWindows())
			{
				AssemblyResolver.Initialize();
			}

#endif
			try
			{
				_version = SassCompilerProxy.GetVersion();
				_languageVersion = SassCompilerProxy.GetLanguageVersion();
			}
			catch (DllNotFoundException e)
			{
				throw new SassCompilerLoadException(Strings.Runtime_SassCompilerNotLoaded, e);
			}
		}


		/// <summary>
		/// "Compiles" a Sass-code to CSS-code
		/// </summary>
		/// <param name="content">Text content written on Sass</param>
		/// <param name="options">Compilation options</param>
		/// <returns>Compilation result</returns>
		/// <exception cref="ArgumentException"/>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="SassСompilationException">Sass compilation error.</exception>
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
		/// "Compiles" a Sass-code to CSS-code
		/// </summary>
		/// <param name="content">Text content written on Sass</param>
		/// <param name="indentedSyntax">Flag for whether to enable Sass Indented Syntax
		/// for parsing the data string</param>
		/// <param name="options">Compilation options</param>
		/// <returns>Compilation result</returns>
		/// <exception cref="ArgumentException"/>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="SassСompilationException">Sass compilation error.</exception>
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
		/// "Compiles" a Sass-code to CSS-code
		/// </summary>
		/// <param name="content">Text content written on Sass</param>
		/// <param name="inputPath">Path to input file</param>
		/// <param name="outputPath">Path to output file</param>
		/// <param name="sourceMapPath">Path to source map file</param>
		/// <param name="options">Compilation options</param>
		/// <returns>Compilation result</returns>
		/// <exception cref="ArgumentException"/>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="SassСompilationException">Sass compilation error.</exception>
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
			var dataContext = new SassDataContext
			{
				SourceString = content
			};

			BeginCompile(dataContext, indentedSyntax, inputPath, outputPath, sourceMapPath, options);
			SassCompilerProxy.Compile(dataContext);
			CompilationResult result = EndCompile(dataContext);

			return result;
		}

		/// <summary>
		/// "Compiles" a Sass-file to CSS-code
		/// </summary>
		/// <param name="inputPath">Path to input file</param>
		/// <param name="outputPath">Path to output file</param>
		/// <param name="sourceMapPath">Path to source map file</param>
		/// <param name="options">Compilation options</param>
		/// <returns>Compilation result</returns>
		/// <exception cref="ArgumentException"/>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="SassСompilationException">Sass compilation error.</exception>
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

			var fileContext = new SassFileContext();
			bool indentedSyntax = GetIndentedSyntax(inputPath);

			BeginCompile(fileContext, indentedSyntax, inputPath, outputPath, sourceMapPath, options);

			try
			{
				SassCompilerProxy.CompileFile(fileContext);
			}
			catch (FileNotFoundException e)
			{
				string filePath = e.FileName;
				string text = string.Format("File to read not found or unreadable: {0}", filePath);
				string message = string.Format("Internal Error: {0}", text);

				throw new SassСompilationException(message, e)
				{
					Status = 3,
					Text = text,
					File = null,
					LineNumber = -1,
					ColumnNumber = -1,
					Source = null
				};
			}

			CompilationResult result = EndCompile(fileContext);

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
				throw new SassСompilationException(error.Message)
				{
					Status = error.Status,
					Text = error.Text,
					File = error.File,
					LineNumber = error.Line,
					ColumnNumber = error.Column,
					Source = error.Source
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
	}
}