

   -----------------------------------------------------------------------
                 README file for LibSass Host for .Net v0.5.0

   -----------------------------------------------------------------------

         Copyright (c) 2015-2016 Andrey Taritsyn - http://www.taritsyn.ru


   ===========
   DESCRIPTION
   ===========
   .NET wrapper around the libSass (http://sass-lang.com/libsass) version
   3.3.6 with the ability to support a virtual file system. For correct
   working of the LibSass Host require assemblies `msvcp120.dll` and
   `msvcr120.dll` from the Visual C++ Redistributable Packages for Visual
   Studio 2013.

   =============
   RELEASE NOTES
   =============
   1. In `IFileManager` interface was added `ToAbsolutePath` method;
   2. Added support of libSass version 3.3.6.

   ====================
   POST-INSTALL ACTIONS
   ====================
   If in your system does not assemblies `msvcp120.dll` and
   `msvcr120.dll`, then download and install the Visual C++
   Redistributable Packages for Visual Studio 2013
   (http://www.microsoft.com/en-us/download/details.aspx?id=40784).

   ============
   PROJECT SITE
   ============
   http://github.com/Taritsyn/LibSassHost