@echo off
echo ========================================
echo Building All Applications
echo ========================================
echo.

echo This will build both Desktop and Mobile applications.
echo.
pause

echo Building Desktop Application...
call build-desktop.bat
if %errorlevel% neq 0 (
    echo Desktop build failed!
    pause
    exit /b 1
)

echo.
echo Building Mobile Application...
call build-mobile.bat
if %errorlevel% neq 0 (
    echo Mobile build failed!
    pause
    exit /b 1
)

echo.
echo ========================================
echo All builds completed successfully!
echo ========================================
echo.
echo Applications created:
echo - Desktop: DesktopApp\bin\Release\net6.0-windows\win-x64\publish\ChildSafeSexEducation.Desktop.exe
echo - Mobile: MobileApp\bin\Release\net6.0-android\ChildSafeSexEducation.Mobile.dll
echo.
pause
