#!/usr/bin/env bash
PRINT_USAGE() {
    echo ""
    echo "[LibSass Build Script Help]"
    echo ""
    echo "build-libsass.sh [options]"
    echo ""
    echo "options:"
    echo "  -d, --debug          Debug build (by default Release build)"
    echo "  -h, --help           Show help"
    echo "  -j [N], --jobs[=N]   Multicore build, allow N jobs at once"
    echo "  -v, --verbose        Display verbose output"
    echo ""
    echo "example:"
    echo "  ./build-libsass.sh"
    echo "debug build:"
    echo "  ./build-libsass.sh --debug"
    echo ""
}

_LOG_FILE_NAME="build-libsass.log"
_MULTICORE_BUILD=""
_VERBOSE=0

export DEBUG=0

while [[ $# -gt 0 ]]; do
    case "$1" in
    -h | --help)
        PRINT_USAGE
        exit
        ;;

    -d | --debug)
        DEBUG=1
        ;;

    -v | --verbose)
        _VERBOSE=1
        ;;

    -j | --jobs)
        if [[ "$1" == "-j" && "$2" =~ ^[^-] ]]; then
            _MULTICORE_BUILD="-j $2"
            shift
        else
            _MULTICORE_BUILD="-j $(nproc)"
        fi
        ;;

    -j=* | --jobs=*)
        _MULTICORE_BUILD=$1
        if [[ "$1" =~ ^-j= ]]; then
            _MULTICORE_BUILD="-j ${_MULTICORE_BUILD:3}"
        else
            _MULTICORE_BUILD="-j ${_MULTICORE_BUILD:7}"
        fi
        ;;

    *)
        echo "Unknown option $1"
        PRINT_USAGE
        exit -1
        ;;
    esac

    shift
done

echo "Building LibSass ..."

_MAKE_ARGS="-C src/libsass ${_MULTICORE_BUILD}"
if [[ $_VERBOSE == 1 ]]; then
    make $_MAKE_ARGS
else
    make $_MAKE_ARGS >$_LOG_FILE_NAME 2>&1
fi

_RET=${PIPESTATUS[0]}
if [[ $_RET == 0 ]]; then
    echo "Succeeded!"
else
    echo "*** Error: Build failed!"
fi

exit $_RET