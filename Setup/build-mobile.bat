@echo off
echo ========================================
echo Building Mobile Application
echo ========================================
echo.

echo Checking for .NET MAUI workload...
dotnet workload list | findstr maui >nul 2>&1
if %errorlevel% neq 0 (
    echo Installing .NET MAUI workload...
    dotnet workload install maui
    if %errorlevel% neq 0 (
        echo ERROR: Failed to install MAUI workload
        pause
        exit /b 1
    )
)

echo .NET MAUI workload found!
echo.

echo Building mobile application...
cd MobileApp
dotnet build -c Release
if %errorlevel% neq 0 (
    echo ERROR: Mobile build failed
    pause
    exit /b 1
)

echo.
echo ========================================
echo Mobile Build completed successfully!
echo ========================================
echo.
echo Mobile app location:
echo MobileApp\bin\Release\net6.0-android\ChildSafeSexEducation.Mobile.dll
echo.
echo To deploy to device:
echo 1. Connect your Android device via USB
echo 2. Enable Developer Options and USB Debugging
echo 3. Run: dotnet build -t:Run -f net6.0-android
echo.
pause
