using System.Collections.Generic;

using BenchmarkDotNet.Attributes;

namespace LibSassHost.Benchmarks
{
	public abstract class ScssCompilationBenchmarkBase
	{
		private static readonly Dictionary<string, Document> s_documents = new Dictionary<string, Document>
		{
			{ "angular-material", new Document("angular/material/_theming.scss") },
			{ "bootstrap", new Document("bootstrap/bootstrap.scss") },
			{ "foundation", new Document("foundation/scss/foundation.scss") }
		};

		public static Dictionary<string, Document> Documents
		{
			get { return s_documents; }
		}

		[ParamsSource(nameof(GetDocumentNames))]
		public string DocumentName { get; set; }


		static ScssCompilationBenchmarkBase()
		{
			Utils.PopulateTestData("Files", s_documents);
		}


		public static void SetFileManager(bool withFileManager)
		{
			IFileManager fileManager = withFileManager ? FileManager.Instance : null;
			SassCompiler.FileManager = fileManager;
		}

		public static IEnumerable<string> GetDocumentNames()
		{
			foreach (string key in s_documents.Keys)
			{
				yield return key;
			}
		}
	}
}