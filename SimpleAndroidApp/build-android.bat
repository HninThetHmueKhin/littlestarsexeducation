@echo off
echo Building Little Star Android App...
echo.

echo Cleaning previous build...
call gradlew clean

echo.
echo Building APK...
call gradlew assembleDebug

echo.
echo Build complete! APK location:
echo app\build\outputs\apk\debug\app-debug.apk
echo.
echo To install on device:
echo adb install app\build\outputs\apk\debug\app-debug.apk
echo.
pause
