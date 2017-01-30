@echo off
setlocal

::--------------------------------------------------------------------------------
:: Process arguments
::--------------------------------------------------------------------------------

:ProcessArgs

set configuration=Release

:ProcessArg
if "%1"=="" goto ProcessArgsDone
if /i "%1"=="--debug" goto SetDebugConfiguration

:SetDebugConfiguration
set configuration=Debug
goto NextArg

:NextArg
shift
goto ProcessArg

:ProcessArgsDone

::--------------------------------------------------------------------------------
:: Check environment
::--------------------------------------------------------------------------------

:CheckMSVS
if "%VisualStudioVersion%"=="12.0" goto CheckMSVSDone
if "%VisualStudioVersion%"=="14.0" goto CheckMSVSDone
echo Error: This script requires a Visual Studio 2013 or 2015 Developer Command
echo Prompt. Browse to http://www.visualstudio.com for more information.
goto Exit
:CheckMSVSDone

::--------------------------------------------------------------------------------
:: Build
::--------------------------------------------------------------------------------

:Build

:Build32Bit
echo Building 32-bit LibSass ...
msbuild libsass.sln /p:Platform=Win32 /p:Configuration=%configuration% >build.log
if errorlevel 1 goto Error
:Build32BitDone

:Build64Bit
echo Building 64-bit LibSass ...
msbuild libsass.sln /p:Platform=x64 /p:Configuration=%configuration% >build.log
if errorlevel 1 goto Error
:Build64BitDone

:BuildDone

::--------------------------------------------------------------------------------
:: Exit
::--------------------------------------------------------------------------------

echo Succeeded!
goto Exit

:Error
echo *** THE PREVIOUS STEP FAILED ***

:Exit
endlocal