#!/usr/bin/env bash
if [ "`uname`" == 'Darwin' ]; then
    LIB_TYPE='dynamic'
else
    LIB_TYPE='shared'
fi

BUILD="$LIB_TYPE" make -C src/libsass -j5