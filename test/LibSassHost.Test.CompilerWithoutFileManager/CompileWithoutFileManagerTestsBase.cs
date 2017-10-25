using System.IO;

using Xunit;

using LibSassHost.Test.Common;

namespace LibSassHost.Test.CompilerWithoutFileManager
{
	public abstract class CompileWithoutFileManagerTestsBase : CompileTestsBase
	{
		static CompileWithoutFileManagerTestsBase()
		{
			SassCompiler.FileManager = null;
		}

		protected CompileWithoutFileManagerTestsBase(SyntaxType syntaxType)
			: base(syntaxType)
		{ }


		#region Mapping errors

		[Fact]
		public override void MappingFileNotFoundErrorDuringCompilationOfFileIsCorrect()
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

		#endregion
	}
}