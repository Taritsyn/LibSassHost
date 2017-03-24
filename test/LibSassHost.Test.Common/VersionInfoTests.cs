using System.Text.RegularExpressions;

using Xunit;

namespace LibSassHost.Test.Common
{
	/// <summary>
	/// Version information tests
	/// </summary>
	public class VersionInfoTests
	{
		/// <summary>
		/// Regular expression for working with the version of the LibSass library
		/// </summary>
		private static readonly Regex _versionRegex = new Regex(@"\d+(?:\.\d+){2}");

		/// <summary>
		/// Regular expression for working with the version of Sass language
		/// </summary>
		private static readonly Regex _languageVersionRegex = new Regex(@"\d+\.\d+");


		[Fact]
		public void VersionFormatIsCorrect()
		{
			// Arrange

			// Act
			string version = SassCompiler.Version;
			bool formatIsCorrect = _versionRegex.IsMatch(version);

			// Assert
			Assert.True(formatIsCorrect);
		}

		[Fact]
		public void LanguageVersionFormatIsCorrect()
		{
			// Arrange

			// Act
			string languageVersion = SassCompiler.LanguageVersion;
			bool formatIsCorrect = _languageVersionRegex.IsMatch(languageVersion);

			// Assert
			Assert.True(formatIsCorrect);
		}
	}
}