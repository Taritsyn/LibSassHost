

   --------------------------------------------------------------------------------
                 README file for LibSass Host for .Net v1.0.0 Beta 1

   --------------------------------------------------------------------------------

           Copyright (c) 2015-2017 Andrey Taritsyn - http://www.taritsyn.ru


   ===========
   DESCRIPTION
   ===========
   .NET wrapper around the libSass (http://sass-lang.com/libsass) with the ability
   to support a virtual file system.

   This package does not contain the native implementations of libSass. Therefore,
   you need to choose and install the most appropriate package(s) for your platform.
   The following packages are available:

    * LibSassHost.Native.win-x86
    * LibSassHost.Native.win-x64
    * LibSassHost.Native.debian-x64
    * LibSassHost.Native.osx-x64

   =============
   RELEASE NOTES
   =============
   1. `SassCompiler` class was converted to a static class;
   2. In `SassCompiler` class was added two static properties: `Version` and
      `LanguageVersion`;
   3. `IndentedSyntax` and `SourceMapFilePath` compilation options was converted
      into parameters of compilation methods;
   4. Added a two new exception classes: `SassException` and
      `SassCompilerLoadException`;
   5. Now it is possible to use the Sass compiler without the file manager;
   6. In `IFileManager` interface was added `SupportsConversionToAbsolutePath`
      property;
   7. In `FileManager` class the `Current` property was renamed to `Instance`.

   ============
   PROJECT SITE
   ============
   http://github.com/Taritsyn/LibSassHost