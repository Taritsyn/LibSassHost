using System;
using System.IO;
using System.Linq;

using LibSassHost.Native;
using LibSassHost.Resources;

namespace LibSassHost
{
	/// <summary>
	/// Sass-compiler
	/// </summary>
	public sealed class SassCompiler : IDisposable
	{
		/// <summary>
		/// Additional amount of unmanaged memory that has been allocated
		/// </summary>
		private const long ADDITIONAL_BYTES_ALLOCATED = 128 * 1024;

		/// <summary>
		/// Instance of file manager
		/// </summary>
		private IFileManager _fileManager;

		/// <summary>
		/// Instance of native Sass-compiler
		/// </summary>
		private SassNativeCompiler _sassNativeCompiler;

		/// <summary>
		/// Flag that object is destroyed
		/// </summary>
		private bool _disposed;

		/// <summary>
		/// Synchronizer of compilation
		/// </summary>
		private static readonly object _compilationSynchronizer = new object();


		/// <summary>
		/// Static constructor
		/// </summary>
		static SassCompiler()
		{
			AssemblyResolver.Initialize();
		}

		/// <summary>
		/// Constructs a instance of Sass-compiler
		/// </summary>
		public SassCompiler()
			: this(FileManager.Current)
		{ }

		/// <summary>
		/// Constructs a instance of Sass-compiler
		/// </summary>
		/// <param name="fileManager">File manager</param>
		public SassCompiler(IFileManager fileManager)
		{
			_fileManager = fileManager;
			_sassNativeCompiler = new SassNativeCompiler();
		}

		/// <summary>
		/// Destructs a instance of Sass-compiler
		/// </summary>
		~SassCompiler()
		{
			Dispose(false);
		}



		/// <summary>
		/// "Compiles" a Sass-code to CSS-code
		/// </summary>
		/// <param name="content">Text content written on Sass</param>
		/// <param name="inputPath">Path to input file</param>
		/// <param name="outputPath">Path to output file</param>
		/// <param name="options">Compilation options</param>
		/// <returns>Compilation result</returns>
		public CompilationResult Compile(string content, string inputPath = null, string outputPath = null, CompilationOptions options = null)
		{
			if (string.IsNullOrWhiteSpace(content))
			{
				throw new ArgumentException(string.Format(Strings.Common_ArgumentIsEmpty, "content"), "content");
			}

			CompilationResult result;
			var dataContext = new SassDataContext
			{
				SourceString = content
			};

			lock (_compilationSynchronizer)
			{
				BeginCompile(dataContext, inputPath, outputPath, options);

				_sassNativeCompiler.Compile(dataContext);

				result = EndCompile(dataContext);
			}

			return result;
		}

		/// <summary>
		/// "Compiles" a Sass-file to CSS-code
		/// </summary>
		/// <param name="inputPath">Path to input file</param>
		/// <param name="outputPath">Path to output file</param>
		/// <param name="options">Compilation options</param>
		/// <returns>Compilation result</returns>
		public CompilationResult CompileFile(string inputPath, string outputPath = null, CompilationOptions options = null)
		{
			if (string.IsNullOrWhiteSpace(inputPath))
			{
				throw new ArgumentException(string.Format(Strings.Common_ArgumentIsEmpty, "inputPath"), "inputPath");
			}

			CompilationResult result;
			var fileContext = new SassFileContext();

			lock (_compilationSynchronizer)
			{
				if (!_fileManager.FileExists(inputPath))
				{
					throw new FileNotFoundException(string.Format(Strings.Common_FileNotExist, inputPath), inputPath);
				}

				BeginCompile(fileContext, inputPath, outputPath, options);

				_sassNativeCompiler.CompileFile(fileContext);

				result = EndCompile(fileContext);
			}

			return result;
		}

		private void BeginCompile(SassContext context, string inputPath, string outputPath, CompilationOptions options)
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
				OutputStyle = (int)options.OutputStyle,
				Precision = options.Precision,
				OmitSourceMapUrl = options.OmitSourceMapUrl,
				SourceMapContents = options.SourceMapIncludeContents,
				SourceMapEmbed = options.InlineSourceMap,
				SourceMapFile = options.SourceMap ? sourceMapFilePath : string.Empty,
				SourceMapRoot = options.SourceMapRootPath,
				SourceComments = options.SourceComments
			};
			context.OutputPath = outputFilePath;

			GC.AddMemoryPressure(ADDITIONAL_BYTES_ALLOCATED);
			FileManagerMarshallingProxy.SetFileManager(_fileManager);
		}

		private CompilationResult EndCompile(SassContext context)
		{
			FileManagerMarshallingProxy.UnsetFileManager();
			GC.RemoveMemoryPressure(ADDITIONAL_BYTES_ALLOCATED);

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
			char character = (type == IndentType.Tab) ? '\t' : ' ';
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

		/// <summary>
		/// Destroys object
		/// </summary>
		/// <param name="disposing">Flag, allowing destruction of
		/// managed objects contained in fields of class</param>
		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				_disposed = true;

				lock (_compilationSynchronizer)
				{
					_sassNativeCompiler = null;
					_fileManager = null;
				}
			}
		}

		#region IDisposable implementation

		/// <summary>
		/// Destroys object
		/// </summary>
		public void Dispose()
		{
			Dispose(true /* disposing */);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}