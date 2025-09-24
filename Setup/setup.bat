@echo off
echo ========================================
echo Safe Learning Chatbot Setup
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

echo Restoring NuGet packages...
cd Backend
dotnet restore
if %errorlevel% neq 0 (
    echo ERROR: Failed to restore packages
    pause
    exit /b 1
)

echo.
echo Building the application...
dotnet build
if %errorlevel% neq 0 (
    echo ERROR: Build failed
    pause
    exit /b 1
)

echo.
echo ========================================
echo Setup completed successfully!
echo ========================================
echo.
echo To start the application:
echo 1. Run: dotnet run
echo 2. Open your browser to: http://localhost:5000
echo 3. Or open Desktop/wrapper.html for desktop version
echo.
echo To build executable:
echo Run: dotnet publish -c Release -r win-x64 --self-contained true
echo.
pause
