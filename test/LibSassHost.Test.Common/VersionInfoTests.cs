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
		private static readonly Regex _versionRegex = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)$");

		/// <summary>
		/// Regular expression for working with the version of Sass language
		/// </summary>
		private static readonly Regex _languageVersionRegex = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)$");


		[Fact]
		public void VersionFormatIsCorrect()
		{
			// Arrange
			bool formatIsCorrect = false;
			int major = -1;
			int minor = -1;
			int patch = -1;

			// Act
			string version = SassCompiler.Version;
			Match match = _versionRegex.Match(version);

			if (match.Success)
			{
				formatIsCorrect = true;

				GroupCollection groups = match.Groups;
				major = int.Parse(groups["major"].Value);
				minor = int.Parse(groups["minor"].Value);
				patch = int.Parse(groups["patch"].Value);
			}

			// Assert
			Assert.True(formatIsCorrect);
			Assert.True(major > 0);
			Assert.True(minor >= 0);
			Assert.True(patch >= 0);
		}

		[Fact]
		public void LanguageVersionFormatIsCorrect()
		{
			// Arrange
			bool formatIsCorrect = false;
			int major = -1;
			int minor = -1;

			// Act
			string languageVersion = SassCompiler.LanguageVersion;
			Match match = _languageVersionRegex.Match(languageVersion);

			if (match.Success)
			{
				formatIsCorrect = true;

				GroupCollection groups = match.Groups;
				major = int.Parse(groups["major"].Value);
				minor = int.Parse(groups["minor"].Value);
			}

			// Assert
			Assert.True(formatIsCorrect);
			Assert.True(major > 0);
			Assert.True(minor >= 0);
		}
	}
}