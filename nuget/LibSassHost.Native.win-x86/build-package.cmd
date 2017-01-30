set project_name=LibSassHost.Native.win-x86
set lib_dir=..\..\lib
set nuget_package_manager=..\..\.nuget\nuget.exe

call "..\setup.cmd"

rmdir runtimes /Q/S
del libsass-license.txt /Q/S

xcopy "%lib_dir%\win-x86\libsass.dll" runtimes\win7-x86\native\
copy "..\..\src\libsass\LICENSE" libsass-license.txt /Y

%nuget_package_manager% pack "..\%project_name%\%project_name%.nuspec"