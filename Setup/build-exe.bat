@echo off
echo ========================================
echo Building Safe Learning Chatbot Executable
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

echo Building executable...
cd Backend
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
if %errorlevel% neq 0 (
    echo ERROR: Build failed
    pause
    exit /b 1
)

echo.
echo ========================================
echo Build completed successfully!
echo ========================================
echo.
echo Executable location:
echo Backend\bin\Release\net6.0\win-x64\publish\ChildSafeSexEducation.exe
echo.
echo You can now distribute this .exe file to other computers.
echo The executable includes all dependencies and doesn't require .NET to be installed.
echo.
pause
