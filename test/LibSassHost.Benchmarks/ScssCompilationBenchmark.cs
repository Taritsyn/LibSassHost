using System.Collections.Generic;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace LibSassHost.Benchmarks
{
	[MemoryDiagnoser]
	public class ScssCompilationBenchmark
	{
		private static readonly Dictionary<string, Document> s_documents = new Dictionary<string, Document>
		{
			{ "@angular/material", new Document("@angular/material/_theming.scss") },
			{ "bootstrap", new Document("bootstrap/scss/bootstrap.scss") },
			{ "foundation-sites", new Document("foundation-sites/scss/foundation.scss") }
		};

		[ParamsSource(nameof(DocumentNames))]
		public string DocumentName { get; set; }


		static ScssCompilationBenchmark()
		{
			Utils.PopulateTestData("../../../node_modules", s_documents);
		}


		private static void SetFileManager(bool withFileManager)
		{
			IFileManager fileManager = withFileManager ? FileManager.Instance : null;
			SassCompiler.FileManager = fileManager;
		}

		public IEnumerable<string> DocumentNames()
		{
			foreach (string key in s_documents.Keys)
			{
				yield return key;
			}
		}

		[Benchmark]
		[Arguments(false)]
		[Arguments(true)]
		public void CompileContent(bool withFileManager)
		{
			SetFileManager(withFileManager);
			Document document = s_documents[DocumentName];

			string compiledContent = SassCompiler.Compile(document.Content, document.AbsolutePath).CompiledContent;
		}

		[Benchmark]
		[Arguments(false)]
		[Arguments(true)]
		public void CompileFile(bool withFileManager)
		{
			SetFileManager(withFileManager);
			Document document = s_documents[DocumentName];

			string compiledContent = SassCompiler.CompileFile(document.AbsolutePath).CompiledContent;
		}
	}
}