set msbuild=C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe
set nuget_package_manager=..\.nuget\nuget.exe

%msbuild% ..\src\LibSassHost.Native\LibSassHost.Native-32.vcxproj /p:Configuration=Release
%msbuild% ..\src\LibSassHost.Native\LibSassHost.Native-64.vcxproj /p:Configuration=Release
%msbuild% ..\src\LibSassHost\LibSassHost.csproj /p:Configuration=Release

rmdir lib /Q/S
rmdir content\LibSassHost.Native /Q/S

xcopy ..\src\LibSassHost\bin\Release\LibSassHost.dll lib\net40-client\
xcopy ..\src\LibSassHost\LibSassHost.Native\*.dll content\LibSassHost.Native\
copy ..\src\libsass\license libsass-license.txt /Y

%nuget_package_manager% pack LibSassHost.nuspec