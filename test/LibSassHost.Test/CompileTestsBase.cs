using System;
using System.IO;

using Xunit;

namespace LibSassHost.Test
{
	public abstract class CompileTestsBase
	{
		private readonly string _resourcesDirectoryPath;

		private readonly string _fileExtension;

		private readonly string _subfolderName;

		private readonly bool _indentedSyntax;


		protected CompileTestsBase(SyntaxType syntaxType)
		{
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

			_resourcesDirectoryPath = Path.GetFullPath(
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../Resources/"));
		}


		private static string GetCanonicalPath(string path)
		{
			return Path.GetFullPath(path).Replace("\\", "/");
		}

		[Fact]
		public virtual void CodeCompilationIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_resourcesDirectoryPath,
				string.Format("variables/{0}/style{1}", _subfolderName, _fileExtension));
			string outputFilePath = Path.Combine(_resourcesDirectoryPath, "variables/style.css");

			string inputCode = File.ReadAllText(inputFilePath);
			string targetOutputCode = File.ReadAllText(outputFilePath);

			var options = new CompilationOptions
			{
				IndentedSyntax = _indentedSyntax
			};

			// Act
			CompilationResult result;

			using (var compiler = new SassCompiler())
			{
				result = compiler.Compile(inputCode, options: options);
			}

			// Assert
			Assert.Equal(targetOutputCode, result.CompiledContent);
			Assert.True(result.SourceMap.Length == 0);
			Assert.Empty(result.IncludedFilePaths);
		}

		[Fact]
		public virtual void CodeWithImportCompilationIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_resourcesDirectoryPath,
				string.Format("import/{0}/base{1}", _subfolderName, _fileExtension));
			string importedFilePath = Path.Combine(_resourcesDirectoryPath,
				string.Format("import/{0}/_reset{1}", _subfolderName, _fileExtension));
			string outputFilePath = Path.Combine(_resourcesDirectoryPath, "import/base.css");

			string inputCode = File.ReadAllText(inputFilePath);
			string targetOutputCode = File.ReadAllText(outputFilePath);

			var options = new CompilationOptions
			{
				IndentedSyntax = _indentedSyntax,
				SourceMap = true
			};

			// Act
			CompilationResult result;

			using (var compiler = new SassCompiler())
			{
				result = compiler.Compile(inputCode, inputFilePath, options: options);
			}

			// Assert
			Assert.Equal(targetOutputCode, result.CompiledContent);
			Assert.True(result.SourceMap.Length > 0);

			var includedFilePaths = result.IncludedFilePaths;
			Assert.Equal(1, includedFilePaths.Count);
			Assert.Equal(GetCanonicalPath(importedFilePath), includedFilePaths[0]);
		}

		[Fact]
		public virtual void FileCompilationIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_resourcesDirectoryPath,
				string.Format("variables/{0}/style{1}", _subfolderName, _fileExtension));
			string outputFilePath = Path.Combine(_resourcesDirectoryPath, "variables/style.css");

			string targetOutputCode = File.ReadAllText(outputFilePath);

			// Act
			CompilationResult result;

			using (var compiler = new SassCompiler())
			{
				result = compiler.CompileFile(inputFilePath);
			}

			// Assert
			Assert.Equal(targetOutputCode, result.CompiledContent);
			Assert.True(result.SourceMap.Length == 0);

			var includedFilePaths = result.IncludedFilePaths;
			Assert.Equal(1, includedFilePaths.Count);
			Assert.Equal(GetCanonicalPath(inputFilePath), includedFilePaths[0]);
		}

		[Fact]
		public virtual void FileWithImportCompilationIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_resourcesDirectoryPath,
				string.Format("import/{0}/base{1}", _subfolderName, _fileExtension));
			string importedFilePath = Path.Combine(_resourcesDirectoryPath,
				string.Format("import/{0}/_reset{1}", _subfolderName, _fileExtension));
			string outputFilePath = Path.Combine(_resourcesDirectoryPath, "import/base.css");

			string targetOutputCode = File.ReadAllText(outputFilePath);

			var options = new CompilationOptions
			{
				SourceMap = true
			};

			// Act
			CompilationResult result;

			using (var compiler = new SassCompiler())
			{
				result = compiler.CompileFile(inputFilePath, options: options);
			}

			// Assert
			Assert.Equal(targetOutputCode, result.CompiledContent);
			Assert.True(result.SourceMap.Length > 0);

			var includedFilePaths = result.IncludedFilePaths;
			Assert.Equal(2, includedFilePaths.Count);
			Assert.Equal(GetCanonicalPath(importedFilePath), includedFilePaths[0]);
			Assert.Equal(GetCanonicalPath(inputFilePath), includedFilePaths[1]);
		}
	}
}