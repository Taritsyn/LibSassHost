set project_name=LibSassHost.Native.debian-x64
set lib_dir=..\..\lib
set nuget_package_manager=..\..\.nuget\nuget.exe

call "..\setup.cmd"

rmdir runtimes /Q/S
del libsass-license.txt /Q/S

xcopy "%lib_dir%\debian-x64\libsass.so" runtimes\debian-x64\native\
copy "..\..\src\libsass\LICENSE" libsass-license.txt /Y

%nuget_package_manager% pack "..\%project_name%\%project_name%.nuspec"