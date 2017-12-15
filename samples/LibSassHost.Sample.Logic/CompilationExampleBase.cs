using System;
using System.IO;
#if NET451 || NETSTANDARD

using Microsoft.Extensions.PlatformAbstractions;
#endif

using LibSassHost.Helpers;

namespace LibSassHost.Sample.Logic
{
	public abstract class CompilationExampleBase
	{
		private static readonly string _filesDirectoryPath;


		/// <summary>
		/// Static constructor
		/// </summary>
		static CompilationExampleBase()
		{
#if NET451 || NETSTANDARD
			var appEnv = PlatformServices.Default.Application;
			string baseDirectoryPath = appEnv.ApplicationBasePath;
#elif NET40
			string baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
#else
#error No implementation for this target
#endif
			_filesDirectoryPath = Path.GetFullPath(Path.Combine(baseDirectoryPath, "Files"));
		}


		protected static void CompileContent()
		{
			WriteHeader("Compilation of SCSS code");

			const string inputContent = @"$font-stack: Helvetica, sans-serif;
$primary-color: #333;

body {
  font: 100% $font-stack;
  color: $primary-color;
}

/* Стрелка вниз */
.down-arrow:before {
  content: ""▼"";
}";

			try
			{
				var options = new CompilationOptions { SourceMap = true };
				CompilationResult result = SassCompiler.Compile(inputContent, "input.scss", "output.css",
					options: options);
				WriteOutput(result);
			}
			catch (SassСompilationException e)
			{
				WriteError("During compilation of SCSS code an error occurred.", e);
			}
		}

		protected static void CompileFile()
		{
			WriteHeader("Compilation of SCSS file");

			string inputFilePath = Path.Combine(_filesDirectoryPath, "style.scss");
			string outputFilePath = Path.Combine(_filesDirectoryPath, "style.css");

			try
			{
				var options = new CompilationOptions { SourceMap = true, SourceMapFileUrls = true };
				CompilationResult result = SassCompiler.CompileFile(inputFilePath, outputFilePath, options: options);
				WriteOutput(result);
			}
			catch (SassСompilationException e)
			{
				WriteError("During compilation of SCSS file an error occurred.", e);
			}
		}

		private static void WriteHeader(string text)
		{
			string separator = new string('-', 80);

			Console.WriteLine(separator);
			Console.WriteLine(text);
			Console.WriteLine(separator);
			Console.WriteLine();
		}

		private static void WriteOutput(CompilationResult result)
		{
			Console.WriteLine("Version: {0}", SassCompiler.Version);
			Console.WriteLine("Language version: {0}", SassCompiler.LanguageVersion);
			Console.WriteLine("Compiled content:{1}{1}{0}{1}", result.CompiledContent, Environment.NewLine);
			Console.WriteLine("Source map:{1}{1}{0}{1}", result.SourceMap, Environment.NewLine);
			Console.WriteLine("Included file paths: {0}", string.Join(", ", result.IncludedFilePaths));
			Console.WriteLine();
		}

		private static void WriteError(string title, SassСompilationException exception)
		{
			Console.WriteLine("{0} See details:", title);
			Console.WriteLine();
			Console.WriteLine(SassErrorHelpers.Format(exception));
			Console.WriteLine();
		}
	}
}