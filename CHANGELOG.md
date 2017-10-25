Change log
==========

## v1.1.1 - October 25, 2017
 * Fixed a “Arithmetic operation resulted in an overflow” error, that occurred during compilation of non-existent file

## v1.1.0 - October 24, 2017
 * Added support of LibSass version 3.4.6
 * LibSassHost.Native.debian-x64 package has been replaced by the LibSassHost.Native.linux-x64 package
 * Added support of .NET Standard 2.0

## v1.0.4 - May 23, 2017
 * Added support of LibSass version 3.4.5

## v1.0.3 - March 31, 2017
 * Added support of LibSass version 3.4.4

## v1.0.2 - March 26, 2017
 * Added support of .NET Core 1.0.4

## v1.0.1 - March 9, 2017
 * Fixed a [error #14](https://github.com/Taritsyn/LibSassHost/issues/14) “Unable to debug ASP.NET MVC project”

## v1.0.0 - March 3, 2017
 * Added support of .NET Core 1.0.3 and .NET Framework 4.5
 * Native assemblies for Windows have been moved to separate packages: LibSassHost.Native.win-x86 and LibSassHost.Native.win-x64
 * Now the LibSass for Windows requires `msvcp140.dll` assembly from the [Visual C++ Redistributable for Visual Studio 2015](https://www.microsoft.com/en-us/download/details.aspx?id=53840)
 * Added a packages, that contains a native assemblies for Debian-based Linux (x64) and OS X (x64)
 * `SassCompiler` class was converted to a static class
 * In `SassCompiler` class was added two static properties: `Version` and `LanguageVersion`
 * `IndentedSyntax` and `SourceMapFilePath` compilation options was converted into parameters of compilation methods
 * `SassСompilationException` class was made serializable
 * Added a two new exception classes: `SassException` and `SassCompilerLoadException`
 * Now it is possible to use the Sass compiler without the file manager
 * In `IFileManager` interface was added `SupportsConversionToAbsolutePath` property
 * In `FileManager` class the `Current` property was renamed to `Instance`

## v1.0.0 Beta 1 - March 1, 2017
 * `SassCompiler` class was converted to a static class
 * In `SassCompiler` class was added two static properties: `Version` and `LanguageVersion`
 * `IndentedSyntax` and `SourceMapFilePath` compilation options was converted into parameters of compilation methods
 * Added a two new exception classes: `SassException` and `SassCompilerLoadException`
 * Now it is possible to use the Sass compiler without the file manager
 * In `IFileManager` interface was added `SupportsConversionToAbsolutePath` property
 * In `FileManager` class the `Current` property was renamed to `Instance`

## v1.0.0 Alpha 2 - February 8, 2017
 * `SassСompilationException` class was made serializable
 * Added a packages, that contains a native assemblies for Debian-based Linux (x64) and OS X (x64)

## v1.0.0 Alpha 1 - January 30, 2017
 * Added support of .NET Core 1.0.3 and .NET Framework 4.5
 * Native assemblies have been moved to separate packages: LibSassHost.Native.win-x86 and LibSassHost.Native.win-x64
 * Now the LibSass for Windows requires `msvcp140.dll` assembly from the [Visual C++ Redistributable for Visual Studio 2015](https://www.microsoft.com/en-us/download/details.aspx?id=53840)

## v0.6.4 - January 8, 2017
 * Added support of LibSass version 3.4.3

## v0.6.3 - January 1, 2017
 * Added support of LibSass version 3.4.2

## v0.6.2 - December 22, 2016
 * Added support of LibSass version 3.4.1

## v0.6.1 - December 10, 2016
 * Added support of LibSass version 3.4.0

## v0.6.0 - October 24, 2016
 * Added support of LibSass version 3.4.0 RC 1
 * In compilation options was added one new property - `SourceMapFileUrls` (default `false`)

## v0.5.2 - July 11, 2016
 * Added support of LibSass version of July 4, 2016

## v0.5.1 - April 25, 2016
 * Improved a “hooks” for processing of paths in CSS `url()` functions by using `ToAbsolutePath` method

## v0.5.0 - April 24, 2016
 * In `IFileManager` interface was added `ToAbsolutePath` method
 * Added support of LibSass version 3.3.6

## v0.4.6 - April 19, 2016
 * Added support of LibSass version 3.3.5

## v0.4.5 - March 20, 2016
 * Fixed a [error #6](https://github.com/Taritsyn/LibSassHost/issues/6) “Mixin rendering problem”

## v0.4.4 - March 19, 2016
 * Added support of LibSass version 3.3.4

## v0.4.3 - January 20, 2016
 * Added support of LibSass version 3.3.3

## v0.4.2 - January 19, 2016
 * Fixed a error [“Sass compilation sometimes fails with native exception in 32 bit mode”](https://bundletransformer.codeplex.com/discussions/649789)

## v0.4.2 Alpha 3 - January 18, 2016
 * Fixed a problem with untimely garbage collection

## v0.4.2 Alpha 2 - January 13, 2016
 * Solved a problem with lack of unmanaged memory

## v0.4.2 Alpha 1 - January 10, 2016
 * LibSass was updated to commit [ed6a057](https://github.com/sass/libsass/tree/ed6a057874b2c9651dcac92844bf7f45517f3eda)

## v0.4.1 - November 11, 2015
 * Added support of LibSass version 3.3.2
 * In NuGet package solved the problem with restoring native assemblies

## v0.4.0 - November 10, 2015
 * From `IFileManager` interface were removed redundant methods and properties

## v0.3.1 - November 2, 2015
 * Added support of LibSass version 3.3.1

## v0.3.0 - November 2, 2015
 * Added support of LibSass version 3.3.0
 * Fixed a problem with processing of contents and paths, that contain Unicode characters

## v0.2.0 - October 24, 2015
 * Initial version uploaded