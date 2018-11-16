using System;
using System.IO;
#if NET452 || NET471 || NETCOREAPP

using Microsoft.Extensions.PlatformAbstractions;
#endif

using Xunit;

using LibSassHost.Helpers;

namespace LibSassHost.Test.Common
{
	public abstract class CompileTestsBase
	{
		protected readonly SyntaxType _syntaxType;

		protected readonly string _filesDirectoryPath;

		protected readonly string _fileExtension;

		protected readonly string _subfolderName;

		protected readonly bool _indentedSyntax;


		protected CompileTestsBase(SyntaxType syntaxType)
		{
#if NETCOREAPP
			TestsInitializer.Initialize();

#endif
#if NET452 || NET471 || NETCOREAPP
			var appEnv = PlatformServices.Default.Application;
			string baseDirectoryPath = Path.Combine(appEnv.ApplicationBasePath, "../../../");
#elif NET40
			string baseDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../");
#else
#error No implementation for this target
#endif
			_filesDirectoryPath = Path.GetFullPath(Path.Combine(baseDirectoryPath, "../SharedFiles"));
			_syntaxType = syntaxType;

			if (syntaxType == SyntaxType.Sass)
			{
				_fileExtension = ".sass";
				_subfolderName = "sass";
				_indentedSyntax = true;
			}
			else if (syntaxType == SyntaxType.Scss)
			{
				_fileExtension = ".scss";
				_subfolderName = "scss";
				_indentedSyntax = false;
			}
			else
			{
				throw new NotSupportedException();
			}
		}


		protected static string GetCanonicalPath(string path)
		{
			return PathHelpers.ProcessBackSlashes(Path.GetFullPath(path));
		}

		#region Compilation

		[Fact]
		public virtual void CodeCompilationIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_filesDirectoryPath,
				string.Format("variables/{0}/style{1}", _subfolderName, _fileExtension));
			string outputFilePath = Path.Combine(_filesDirectoryPath, "variables/style.css");

			string inputCode = File.ReadAllText(inputFilePath);
			string targetOutputCode = File.ReadAllText(outputFilePath);

			// Act
			CompilationResult result = SassCompiler.Compile(inputCode, _indentedSyntax);

			// Assert
			Assert.Equal(targetOutputCode, result.CompiledContent);
			Assert.True(string.IsNullOrEmpty(result.SourceMap));
			Assert.Empty(result.IncludedFilePaths);
		}

		[Fact]
		public virtual void CodeWithImportCompilationIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_filesDirectoryPath,
				string.Format("import/{0}/base{1}", _subfolderName, _fileExtension));
			string importedFilePath = Path.Combine(_filesDirectoryPath,
				string.Format("import/{0}/_reset{1}", _subfolderName, _fileExtension));
			string outputFilePath = Path.Combine(_filesDirectoryPath, "import/base.css");

			string inputCode = File.ReadAllText(inputFilePath);
			string targetOutputCode = File.ReadAllText(outputFilePath);

			var options = new CompilationOptions
			{
				SourceMap = true
			};

			// Act
			CompilationResult result = SassCompiler.Compile(inputCode, inputFilePath, options: options);

			// Assert
			Assert.Equal(targetOutputCode, result.CompiledContent);
			Assert.False(string.IsNullOrEmpty(result.SourceMap));

			var includedFilePaths = result.IncludedFilePaths;
			Assert.Equal(1, includedFilePaths.Count);
			Assert.Equal(GetCanonicalPath(importedFilePath), includedFilePaths[0]);
		}

		[Fact]
		public virtual void CodeWithUtf8CharactersCompilationIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_filesDirectoryPath,
				string.Format("ютф-8/{0}/символы{1}", _subfolderName, _fileExtension));
			string outputFilePath = Path.Combine(_filesDirectoryPath, "ютф-8/символы.css");

			string inputCode = File.ReadAllText(inputFilePath);
			string targetOutputCode = File.ReadAllText(outputFilePath);

			// Act
			CompilationResult result = SassCompiler.Compile(inputCode, _indentedSyntax);

			// Assert
			Assert.Equal(targetOutputCode, result.CompiledContent);
			Assert.True(string.IsNullOrEmpty(result.SourceMap));
			Assert.Empty(result.IncludedFilePaths);
		}

		[Fact]
		public virtual void FileCompilationIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_filesDirectoryPath,
				string.Format("variables/{0}/style{1}", _subfolderName, _fileExtension));
			string outputFilePath = Path.Combine(_filesDirectoryPath, "variables/style.css");

			string targetOutputCode = File.ReadAllText(outputFilePath);

			// Act
			CompilationResult result = SassCompiler.CompileFile(inputFilePath);

			// Assert
			Assert.Equal(targetOutputCode, result.CompiledContent);
			Assert.True(string.IsNullOrEmpty(result.SourceMap));

			var includedFilePaths = result.IncludedFilePaths;
			Assert.Equal(1, includedFilePaths.Count);
			Assert.Equal(GetCanonicalPath(inputFilePath), includedFilePaths[0]);
		}

		[Fact]
		public virtual void FileWithImportCompilationIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_filesDirectoryPath,
				string.Format("import/{0}/base{1}", _subfolderName, _fileExtension));
			string importedFilePath = Path.Combine(_filesDirectoryPath,
				string.Format("import/{0}/_reset{1}", _subfolderName, _fileExtension));
			string outputFilePath = Path.Combine(_filesDirectoryPath, "import/base.css");

			string targetOutputCode = File.ReadAllText(outputFilePath);

			var options = new CompilationOptions
			{
				SourceMap = true
			};

			// Act
			CompilationResult result = SassCompiler.CompileFile(inputFilePath, options: options);

			// Assert
			Assert.Equal(targetOutputCode, result.CompiledContent);
			Assert.False(string.IsNullOrEmpty(result.SourceMap));

			var includedFilePaths = result.IncludedFilePaths;
			Assert.Equal(2, includedFilePaths.Count);
			Assert.Equal(GetCanonicalPath(inputFilePath), includedFilePaths[0]);
			Assert.Equal(GetCanonicalPath(importedFilePath), includedFilePaths[1]);
		}

		[Fact]
		public virtual void FileWithUtf8CharactersCompilationIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_filesDirectoryPath,
				string.Format("ютф-8/{0}/символы{1}", _subfolderName, _fileExtension));
			string outputFilePath = Path.Combine(_filesDirectoryPath, "ютф-8/символы.css");

			string targetOutputCode = File.ReadAllText(outputFilePath);

			// Act
			CompilationResult result = SassCompiler.CompileFile(inputFilePath);

			// Assert
			Assert.Equal(targetOutputCode, result.CompiledContent);
			Assert.True(string.IsNullOrEmpty(result.SourceMap));

			var includedFilePaths = result.IncludedFilePaths;
			Assert.Equal(1, includedFilePaths.Count);
			Assert.Equal(GetCanonicalPath(inputFilePath), includedFilePaths[0]);
		}

		#endregion

		#region Mapping errors

		[Fact]
		public virtual void MappingFileNotFoundErrorDuringCompilationOfCodeIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_filesDirectoryPath,
				string.Format("non-existing-files/{0}/base{1}", _subfolderName, _fileExtension));
			string inputCode = File.ReadAllText(inputFilePath);
			var options = new CompilationOptions
			{
				SourceMap = true
			};

			SassСompilationException exception = null;

			// Act
			try
			{
				CompilationResult result = SassCompiler.Compile(inputCode, inputFilePath, options: options);
			}
			catch (SassСompilationException e)
			{
				exception = e;
			}

			// Assert
			Assert.NotNull(exception);
			Assert.NotEmpty(exception.Message);
			Assert.Equal(GetCanonicalPath(inputFilePath), exception.File);
			Assert.Equal(_syntaxType == SyntaxType.Sass ? 5 : 6, exception.LineNumber);
			Assert.Equal(1, exception.ColumnNumber);
		}

		[Fact]
		public virtual void MappingSyntaxErrorDuringCompilationOfCodeIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_filesDirectoryPath,
				string.Format("invalid-syntax/{0}/style{1}", _subfolderName, _fileExtension));
			string inputCode = File.ReadAllText(inputFilePath);
			var options = new CompilationOptions
			{
				SourceMap = true
			};

			SassСompilationException exception = null;

			// Act
			try
			{
				CompilationResult result = SassCompiler.Compile(inputCode, inputFilePath, options: options);
			}
			catch (SassСompilationException e)
			{
				exception = e;
			}

			// Assert
			Assert.NotNull(exception);
			Assert.NotEmpty(exception.Message);
			Assert.Equal(GetCanonicalPath(inputFilePath), exception.File);
			Assert.Equal(3, exception.LineNumber);
			Assert.Equal(13, exception.ColumnNumber);
		}

		[Fact]
		public virtual void MappingFileNotFoundErrorDuringCompilationOfFileIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_filesDirectoryPath,
				string.Format("non-existing-files/{0}/style{1}", _subfolderName, _fileExtension));

			SassСompilationException exception = null;

			// Act
			try
			{
				CompilationResult result = SassCompiler.CompileFile(inputFilePath);
			}
			catch (SassСompilationException e)
			{
				exception = e;
			}

			// Assert
			Assert.NotNull(exception);
			Assert.NotEmpty(exception.Message);
			Assert.Null(exception.File);
			Assert.Equal(-1, exception.LineNumber);
			Assert.Equal(-1, exception.ColumnNumber);
		}

		[Fact]
		public virtual void MappingSyntaxErrorDuringCompilationOfFileIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_filesDirectoryPath,
				string.Format("invalid-syntax/{0}/style{1}", _subfolderName, _fileExtension));

			SassСompilationException exception = null;

			// Act
			try
			{
				CompilationResult result = SassCompiler.CompileFile(inputFilePath);
			}
			catch (SassСompilationException e)
			{
				exception = e;
			}

			// Assert
			Assert.NotNull(exception);
			Assert.NotEmpty(exception.Message);
			Assert.Equal(GetCanonicalPath(inputFilePath), exception.File);
			Assert.Equal(3, exception.LineNumber);
			Assert.Equal(13, exception.ColumnNumber);
		}

		#endregion
	}
}