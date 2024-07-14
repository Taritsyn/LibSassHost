@echo off
setlocal

::--------------------------------------------------------------------------------
:: Process arguments
::--------------------------------------------------------------------------------

:process-args

set _CONFIGURATION=Release
set _EMBED_MSVC_RUNTIME=false
set _EMBED_MFC_LIBRARY=false
set _LOG_FILE_NAME=build-libsass.log
set _VERBOSE=false

:process-arg
if "%1"=="" goto process-args-done
if "%1"=="/?" goto print-usage
if "%1"=="-h" goto print-usage
if "%1"=="--help" goto print-usage
if /i "%1"=="-d" goto set-debug-configuration
if /i "%1"=="--debug" goto set-debug-configuration
if /i "%1"=="--embed-msvc-runtime" goto set-msvc-runtime-embedding
if /i "%1"=="--embed-mfc-library" goto set-mfc-library-embedding
if /i "%1"=="-v" goto set-verbose-output
if /i "%1"=="--verbose" goto set-verbose-output

:print-usage
echo [LibSass Build Script Help]
echo.
echo build-libsass.cmd [options]
echo.
echo options:
echo   -d, --debug             Debug build (by default Release build)
echo   --embed-msvc-runtime    Link a assembly with MSVC libraries statically
echo   --embed-mfc-library     Link a assembly with MFC DLL statically
echo   -h, --help              Show help
echo   -v, --verbose           Display verbose output
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

:set-msvc-runtime-embedding
set _EMBED_MSVC_RUNTIME=true
goto next-arg

:set-mfc-library-embedding
set _EMBED_MFC_LIBRARY=true
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
echo *** Error: This script requires a 64-bit operating system.
goto exit
:check-os-done

:check-vs
if "%VisualStudioVersion%"=="16.0" goto check-vs-done
if "%VisualStudioVersion%"=="17.0" goto check-vs-done
echo *** Error: This script requires a Developer Command Prompt for VS2019 or VS2022.
echo Browse to http://www.visualstudio.com for more information.
goto exit
:check-vs-done

::--------------------------------------------------------------------------------
:: Build
::--------------------------------------------------------------------------------

:build

set _MSBUILD_ARGS=libsass.sln /p:Configuration=%_CONFIGURATION% /p:EmbedMsvcRuntime=%_EMBED_MSVC_RUNTIME% /p:EmbedMfcLibrary=%_EMBED_MFC_LIBRARY%

:build-x86
echo Building LibSass (x86) ...
set _MSBUILD_ARGS=%_MSBUILD_ARGS% /p:Platform=Win32
if "%_VERBOSE%"=="true" (msbuild %_MSBUILD_ARGS%) else (msbuild %_MSBUILD_ARGS% >%_LOG_FILE_NAME%)
if errorlevel 1 goto error
:build-x86-done

:build-x64
echo Building LibSass (x64) ...
set _MSBUILD_ARGS=%_MSBUILD_ARGS% /p:Platform=x64
if "%_VERBOSE%"=="true" (msbuild %_MSBUILD_ARGS%) else (msbuild %_MSBUILD_ARGS% >%_LOG_FILE_NAME%)
if errorlevel 1 goto error
:build-x64-done

:build-arm
echo Building LibSass (ARM) ...
set _MSBUILD_ARGS=%_MSBUILD_ARGS% /p:Platform=ARM
if "%_VERBOSE%"=="true" (msbuild %_MSBUILD_ARGS%) else (msbuild %_MSBUILD_ARGS% >%_LOG_FILE_NAME%)
if errorlevel 1 goto error
:build-arm-done

:build-arm64
echo Building LibSass (ARM64) ...
set _MSBUILD_ARGS=%_MSBUILD_ARGS% /p:Platform=ARM64
if "%_VERBOSE%"=="true" (msbuild %_MSBUILD_ARGS%) else (msbuild %_MSBUILD_ARGS% >%_LOG_FILE_NAME%)
if errorlevel 1 goto error
:build-arm64-done

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