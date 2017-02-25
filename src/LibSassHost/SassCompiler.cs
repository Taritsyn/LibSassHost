using System;
using System.IO;
using System.Linq;

using LibSassHost.Internal;
using LibSassHost.Resources;
using LibSassHost.Utilities;

namespace LibSassHost
{
	/// <summary>
	/// Sass-compiler
	/// </summary>
	public sealed class SassCompiler : IDisposable
	{
		/// <summary>
		/// Version of the libSass library
		/// </summary>
		private static readonly string _version;

		/// <summary>
		/// Version of Sass language
		/// </summary>
		private static readonly string _languageVersion;

		/// <summary>
		/// Synchronizer of compilation
		/// </summary>
		private static readonly object _compilationSynchronizer = new object();

		/// <summary>
		/// Instance of file manager
		/// </summary>
		private IFileManager _fileManager;

		/// <summary>
		/// Flag that object is destroyed
		/// </summary>
		private InterlockedStatedFlag _disposedFlag = new InterlockedStatedFlag();

		/// <summary>
		/// Gets a version of the libSass library
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
		/// Static constructor
		/// </summary>
		/// <exception cref="SassCompilerLoadException">Failed to load a Sass-compiler.</exception>
		static SassCompiler()
		{
#if !NETSTANDARD1_3
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
				throw new SassCompilerLoadException(
					Strings.Runtime_SassCompilerNotLoaded, e);
			}
		}

		/// <summary>
		/// Constructs an instance of Sass-compiler
		/// </summary>
		public SassCompiler()
			: this(FileManager.Current)
		{ }

		/// <summary>
		/// Constructs an instance of Sass-compiler
		/// </summary>
		/// <param name="fileManager">File manager</param>
		/// <exception cref="ArgumentNullException" />
		public SassCompiler(IFileManager fileManager)
		{
			if (fileManager == null)
			{
				throw new ArgumentNullException(
					"fileManager", string.Format(Strings.Common_ArgumentIsNull, "fileManager"));
			}

			_fileManager = fileManager;
		}


		private void VerifyNotDisposed()
		{
			if (_disposedFlag.IsSet())
			{
				throw new ObjectDisposedException(ToString());
			}
		}

		/// <summary>
		/// "Compiles" a Sass-code to CSS-code
		/// </summary>
		/// <param name="content">Text content written on Sass</param>
		/// <param name="inputPath">Path to input file</param>
		/// <param name="outputPath">Path to output file</param>
		/// <param name="options">Compilation options</param>
		/// <returns>Compilation result</returns>
		/// <exception cref="ArgumentException"/>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="ObjectDisposedException">Operation is performed on a disposed Sass-compiler.</exception>
		/// <exception cref="SassСompilationException">Sass compilation error.</exception>
		public CompilationResult Compile(string content, string inputPath = null, string outputPath = null,
			CompilationOptions options = null)
		{
			VerifyNotDisposed();

			if (content == null)
			{
				throw new ArgumentNullException(
					"content", string.Format(Strings.Common_ArgumentIsNull, "content"));
			}

			if (string.IsNullOrWhiteSpace(content))
			{
				throw new ArgumentException(
					string.Format(Strings.Common_ArgumentIsEmpty, "content"), "content");
			}

			var dataContext = new SassDataContext
			{
				SourceString = content
			};

			BeginCompile(dataContext, inputPath, outputPath, options);

			lock (_compilationSynchronizer)
			{
				FileManagerMarshaler.SetFileManager(_fileManager);
				SassCompilerProxy.Compile(dataContext);
				FileManagerMarshaler.UnsetFileManager();
			}

			CompilationResult result = EndCompile(dataContext);

			return result;
		}

		/// <summary>
		/// "Compiles" a Sass-file to CSS-code
		/// </summary>
		/// <param name="inputPath">Path to input file</param>
		/// <param name="outputPath">Path to output file</param>
		/// <param name="options">Compilation options</param>
		/// <returns>Compilation result</returns>
		/// <exception cref="ArgumentException"/>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="FileNotFoundException">Input file not exist.</exception>
		/// <exception cref="ObjectDisposedException">Operation is performed on a disposed Sass-compiler.</exception>
		/// <exception cref="SassСompilationException">Sass compilation error.</exception>
		public CompilationResult CompileFile(string inputPath, string outputPath = null,
			CompilationOptions options = null)
		{
			VerifyNotDisposed();

			if (inputPath == null)
			{
				throw new ArgumentNullException(
					"inputPath", string.Format(Strings.Common_ArgumentIsNull, "inputPath"));
			}

			if (string.IsNullOrWhiteSpace(inputPath))
			{
				throw new ArgumentException(
					string.Format(Strings.Common_ArgumentIsEmpty, "inputPath"), "inputPath");
			}

			if (!_fileManager.FileExists(inputPath))
			{
				throw new FileNotFoundException(
					string.Format(Strings.Common_FileNotExist, inputPath), inputPath);
			}

			var fileContext = new SassFileContext();

			BeginCompile(fileContext, inputPath, outputPath, options);

			lock (_compilationSynchronizer)
			{
				FileManagerMarshaler.SetFileManager(_fileManager);
				SassCompilerProxy.CompileFile(fileContext);
				FileManagerMarshaler.UnsetFileManager();
			}

			CompilationResult result = EndCompile(fileContext);

			return result;
		}

		private void BeginCompile(SassContextBase context, string inputPath, string outputPath, CompilationOptions options)
		{
			options = options ?? new CompilationOptions();

			string inputFilePath = !string.IsNullOrWhiteSpace(inputPath) ? inputPath : string.Empty;
			string outputFilePath = !string.IsNullOrWhiteSpace(outputPath) ? outputPath : string.Empty;
			string sourceMapFilePath = !string.IsNullOrWhiteSpace(options.SourceMapFilePath) ?
				options.SourceMapFilePath : string.Empty;

			if (inputFilePath.Length > 0 || outputFilePath.Length > 0)
			{
				outputFilePath = outputFilePath.Length > 0 ?
					outputPath : Path.ChangeExtension(inputFilePath, ".css");
				sourceMapFilePath = sourceMapFilePath.Length > 0 ?
					sourceMapFilePath : Path.ChangeExtension(outputFilePath, ".css.map");
			}

			context.InputPath = inputFilePath;
			context.Options = new SassOptions
			{
				IncludePath = string.Join(";", options.IncludePaths),
				Indent = GetIndentString(options.IndentType, options.IndentWidth),
				IsIndentedSyntaxSource = options.IndentedSyntax,
				LineFeed = GetLineFeedString(options.LineFeedType),
				OutputStyle = options.OutputStyle,
				Precision = options.Precision,
				OmitSourceMapUrl = options.OmitSourceMapUrl,
				SourceMapContents = options.SourceMapIncludeContents,
				SourceMapEmbed = options.InlineSourceMap,
				SourceMapFile = options.SourceMap ? sourceMapFilePath : string.Empty,
				SourceMapFileUrls = options.SourceMapFileUrls,
				SourceMapRoot = options.SourceMapRootPath,
				SourceComments = options.SourceComments
			};
			context.OutputPath = outputFilePath;
		}

		private CompilationResult EndCompile(SassContextBase context)
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

		#region IDisposable implementation

		/// <summary>
		/// Destroys object
		/// </summary>
		public void Dispose()
		{
			if (_disposedFlag.Set())
			{
				_fileManager = null;
			}
		}

		#endregion
	}
}