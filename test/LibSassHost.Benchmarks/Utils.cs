using System;
using System.Collections.Generic;
using System.IO;

namespace LibSassHost.Benchmarks
{
	internal static class Utils
	{
		public static void PopulateTestData(string directoryPath, Dictionary<string, Document> documents)
		{
			string absoluteDirectoryPath = Path.GetFullPath(
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryPath));

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