@echo off
echo ========================================
echo Building Desktop Application
echo ========================================
echo.

echo Checking for .NET SDK...
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET SDK not found!
    echo Please install .NET 6.0 SDK or later from:
    echo https://dotnet.microsoft.com/download
    echo.
    pause
    exit /b 1
)

echo .NET SDK found!
echo.

echo Building desktop application...
cd DesktopApp
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
if %errorlevel% neq 0 (
    echo ERROR: Desktop build failed
    pause
    exit /b 1
)

echo.
echo ========================================
echo Desktop Build completed successfully!
echo ========================================
echo.
echo Desktop executable location:
echo DesktopApp\bin\Release\net6.0-windows\win-x64\publish\ChildSafeSexEducation.Desktop.exe
echo.
echo You can now run the desktop application directly.
echo.
pause
