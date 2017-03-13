@echo off
setlocal

::--------------------------------------------------------------------------------
:: Process arguments
::--------------------------------------------------------------------------------

:process-args

set _CONFIGURATION=Release
set _IS_64_BIT_OS=true
set _LOG_FILE_NAME=build-libsass.log
set _VERBOSE=false

:process-arg
if "%1"=="" goto process-args-done
if "%1"=="/?" goto print-usage
if "%1"=="-h" goto print-usage
if "%1"=="--help" goto print-usage
if /i "%1"=="-d" goto set-debug-configuration
if /i "%1"=="--debug" goto set-debug-configuration
if /i "%1"=="-v" goto set-verbose-output
if /i "%1"=="--verbose" goto set-verbose-output

:print-usage
echo [LibSass Build Script Help]
echo.
echo build-libsass.cmd [options]
echo.
echo options:
echo   -d, --debug          Debug build (by default Release build)
echo   -h, --help           Show help
echo   -v, --verbose        Display verbose output
echo.
echo example:
echo   build-libsass.cmd
echo debug build:
echo   build-libsass.cmd --debug
echo.
goto exit

:set-debug-configuration
set _CONFIGURATION=Debug
goto next-arg

:set-verbose-output
set _VERBOSE=true
goto next-arg

:next-arg
shift
goto process-arg

:process-args-done

::--------------------------------------------------------------------------------
:: Check environment
::--------------------------------------------------------------------------------

:check-os
if /i "%PROCESSOR_ARCHITECTURE%"=="AMD64" goto check-os-done
if defined PROCESSOR_ARCHITEW6432 goto check-os-done
set _IS_64_BIT_OS=false
echo *** Warning: Your operating system does not support 64-bit build. 
:check-os-done

:check-vs
if "%VisualStudioVersion%"=="12.0" goto check-vs-done
if "%VisualStudioVersion%"=="14.0" goto check-vs-done
if "%VisualStudioVersion%"=="15.0" goto check-vs-done
echo *** Error: This script requires a Developer Command Prompt for VS2013, VS2015 or VS2017.
goto exit
:check-vs-done

::--------------------------------------------------------------------------------
:: Build
::--------------------------------------------------------------------------------

:build

set _MSBUILD_ARGS=libsass.sln /p:Configuration=%_CONFIGURATION%

:build-32-bit
echo Building 32-bit LibSass ...
set _MSBUILD_ARGS=%_MSBUILD_ARGS% /p:Platform=Win32
if "%_VERBOSE%"=="true" (msbuild %_MSBUILD_ARGS%) else (msbuild %_MSBUILD_ARGS% >%_LOG_FILE_NAME%)
if errorlevel 1 goto error
:build-32-bit-done

:build-64-bit
if "%_IS_64_BIT_OS%"=="false" goto exit
echo Building 64-bit LibSass ...
set _MSBUILD_ARGS=%_MSBUILD_ARGS% /p:Platform=x64
if "%_VERBOSE%"=="true" (msbuild %_MSBUILD_ARGS%) else (msbuild %_MSBUILD_ARGS% >%_LOG_FILE_NAME%)
if errorlevel 1 goto error
:build-64-bit-done

:build-done

::--------------------------------------------------------------------------------
:: Exit
::--------------------------------------------------------------------------------

echo Succeeded!
goto exit

:error
echo *** Error: The previous step failed!

:exit
endlocal