

   --------------------------------------------------------------------------------
                     README file for LibSass Host for .Net v1.2.2

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
   1. An attempt was made to fix a error #26 “"Unable to find an entry point named
      'libsass_version' in DLL 'libsass'." on Azure Web App”;
   2. Part of the auxiliary code was moved to external libraries:
      PolyfillsForOldDotNet and AdvancedStringBuilder.

   ============
   PROJECT SITE
   ============
   http://github.com/Taritsyn/LibSassHost