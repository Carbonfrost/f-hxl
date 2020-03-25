#!/bin/sh

#
# Shim to execute `tuxasm'
#
# Environment variables:
#
# DOTNET_OPTIONS    Specifies options to dotnet
#

set -e

app_name=APP_NAME
PREFIX="/usr/local"

if ! command -v dotnet > /dev/null; then
    echo "fatal: Unable to start $app_name: could not find \`dotnet' in path."
    exit 1
fi

BINARY="$PREFIX/opt/$app_name/$app_name.dll"
DOTNET_BINARY=dotnet

exec $DOTNET_BINARY $DOTNET_OPTIONS exec $BINARY $*
