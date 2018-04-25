

   --------------------------------------------------------------------------------
              README file for LibSass Host Native for Windows x64 v1.1.9

   --------------------------------------------------------------------------------

           Copyright (c) 2015-2018 Andrey Taritsyn - http://www.taritsyn.ru


   ===========
   DESCRIPTION
   ===========
   This package complements the LibSassHost package and contains the native
   implementation of LibSass version 3.5.4 for Windows (x64).

   For correct working of the LibSass require the Microsoft Visual C++ 2015
   Redistributable.

   =============
   RELEASE NOTES
   =============
   1. Added support of LibSass version 3.5.4;
   2. The directory with `win7-x64` RID was renamed to `win-x64`.

   ====================
   POST-INSTALL ACTIONS
   ====================
   If in your system does not `api-ms-win-core-*.dll`, `api-ms-win-crt-*.dll`,
   `concrt140.dll`, `msvcp140.dll`, `ucrtbase.dll` and `vcruntime140.dll`
   assemblies, then download and install the Microsoft Visual C++ 2015
   Redistributable (https://www.microsoft.com/en-us/download/details.aspx?id=53840).

   ============
   PROJECT SITE
   ============
   http://github.com/Taritsyn/LibSassHost