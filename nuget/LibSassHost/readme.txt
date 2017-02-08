

   --------------------------------------------------------------------------------
                 README file for LibSass Host for .Net v1.0.0 Alpha 2

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
   1. `SassСompilationException` class was made serializable;
   2. Added a packages, that contains a native assemblies for Debian-based Linux
      (x64) and OS X (x64).

   ============
   PROJECT SITE
   ============
   http://github.com/Taritsyn/LibSassHost