using System.Collections.Generic;
using System.IO;

namespace LibSassHost.Benchmarks
{
	internal static class Utils
	{
		public static void PopulateTestData(string directoryPath, Dictionary<string, Document> documents)
		{
			string currentDirectory = Path.GetDirectoryName(typeof(Program).Assembly.Location);
			string absoluteDirectoryPath = Path.GetFullPath(Path.Combine(currentDirectory, directoryPath));

			foreach (string documentName in documents.Keys)
			{
				Document document = documents[documentName];
				string relativePath = document.RelativePath;
				string absolutePath = Path.Combine(absoluteDirectoryPath, relativePath);
				string content = File.ReadAllText(absolutePath);

				document.AbsolutePath = absolutePath;
				document.Content = content;
			}
		}
	}
}