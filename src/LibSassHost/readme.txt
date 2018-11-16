

   --------------------------------------------------------------------------------
                     README file for LibSass Host for .Net v1.2.0

   --------------------------------------------------------------------------------

           Copyright (c) 2015-2018 Andrey Taritsyn - http://www.taritsyn.ru


   ===========
   DESCRIPTION
   ===========
   .NET wrapper around the LibSass (http://sass-lang.com/libsass) with the ability
   to support a virtual file system.

   This package does not contain the native implementations of LibSass. Therefore,
   you need to choose and install the most appropriate package(s) for your platform.
   The following packages are available:

    * LibSassHost.Native.win-x86
    * LibSassHost.Native.win-x64
    * LibSassHost.Native.linux-x64
    * LibSassHost.Native.osx-x64

   =============
   RELEASE NOTES
   =============
   1. Added support of the LibSass version 3.5.5;
   2. From compilation options was removed the `AdditionalImportExtensions`
      property;
   3. Fixed a error #30 “Conflict of using multiple sites in one application pool”;
   4. Error messages have become more informative;
   5. In the `SassException` class was added one new property - `Description`;
   6. In the `SassСompilationException` class was added two new properties:
      `ErrorCode` and `SourceFragment`;
   7. In the `SassСompilationException` class the following properties have been
      deprecated: `Status` and `Text`;
   8. `Format` method of the `SassErrorHelpers` class was renamed to the
      `GenerateErrorDetails`.

   ============
   PROJECT SITE
   ============
   http://github.com/Taritsyn/LibSassHost