set project_name=LibSassHost.Native.linux-x64
set lib_dir=..\..\lib
set nuget_package_manager=..\..\.nuget\nuget.exe

call "..\setup.cmd"

rmdir runtimes /Q/S
del libsass-license.txt /Q/S

xcopy "%lib_dir%\linux-x64\libsass.so" runtimes\linux-x64\native\
copy "..\..\src\libsass\LICENSE" libsass-license.txt /Y

%nuget_package_manager% pack "..\%project_name%\%project_name%.nuspec"