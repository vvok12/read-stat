#!/bin/bash
APP_NAME="./ReadStat.app"
PUBLISH_OUTPUT_DIRECTORY="./ReadStat/bin/Release/net8.0/osx-arm64/publish/."
INFO_PLIST="./Info.plist"

if [ -d "$APP_NAME" ]
then
    rm -rf "$APP_NAME"
fi

mkdir "$APP_NAME"

mkdir "$APP_NAME/Contents"
mkdir "$APP_NAME/Contents/MacOS"
mkdir "$APP_NAME/Contents/Resources"

cp "$INFO_PLIST" "$APP_NAME/Contents/Info.plist"
cp -a "$PUBLISH_OUTPUT_DIRECTORY" "$APP_NAME/Contents/MacOS"