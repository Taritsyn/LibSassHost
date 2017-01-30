

   --------------------------------------------------------------------------------
                 README file for LibSass Host for .Net v1.0.0 Alpha 1

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

   =============
   RELEASE NOTES
   =============
   1. Added support of .NET Core 1.0.3 and .NET Framework 4.5;
   2. Native assemblies have been moved to separate packages:
      LibSassHost.Native.win-x86 and LibSassHost.Native.win-x64;
   3. Now the libSass for Windows requires `msvcp140.dll` assembly from the Visual
      C++ Redistributable for Visual Studio 2015.

   ============
   PROJECT SITE
   ============
   http://github.com/Taritsyn/LibSassHost