using System.IO;

using LibSassHost.Test.Common;

using Xunit;

namespace LibSassHost.Test.CompilerWithFileManager
{
	public abstract class CompileWithFileManagerTestsBase : CompileTestsBase
	{
		static CompileWithFileManagerTestsBase()
		{
			SassCompiler.FileManager = FileManager.Instance;
		}

		protected CompileWithFileManagerTestsBase(SyntaxType syntaxType)
			: base(syntaxType)
		{ }


		#region Compilation

		[Fact]
		public virtual void CodeWithUtf16CharactersCompilationIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_filesDirectoryPath,
				string.Format("ютф-16/{0}/символы{1}", _subfolderName, _fileExtension));
			string outputFilePath = Path.Combine(_filesDirectoryPath, "ютф-16/символы.css");

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
		public virtual void FileWithUtf16CharactersCompilationIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_filesDirectoryPath,
				string.Format("ютф-16/{0}/символы{1}", _subfolderName, _fileExtension));
			string outputFilePath = Path.Combine(_filesDirectoryPath, "ютф-16/символы.css");

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
		public override void MappingFileNotFoundErrorDuringCompilationOfFileIsCorrect()
		{
			// Arrange
			string inputFilePath = Path.Combine(_filesDirectoryPath,
				string.Format("non-existing-files/{0}/style{1}", _subfolderName, _fileExtension));

			FileNotFoundException exception = null;

			// Act
			try
			{
				CompilationResult result = SassCompiler.CompileFile(inputFilePath);
			}
			catch (FileNotFoundException e)
			{
				exception = e;
			}

			// Assert
			Assert.NotNull(exception);
			Assert.NotEmpty(exception.Message);
		}

		#endregion
	}
}