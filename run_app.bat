@echo off
echo Starting Project2.3 Application...
cd /d "%~dp0"
echo Current directory: %CD%
echo.
echo Attempting to run the application...
start "" ".\bin\Debug\net8.0-windows10.0.19041.0\win10-x64\Project2.3.exe"
if %errorlevel% neq 0 (
    echo.
    echo Error: Application failed to start with exit code %errorlevel%
    echo.
    echo This might be due to missing Windows App SDK runtime.
    echo Try installing it manually from: https://aka.ms/windowsappsdk
    echo.
    pause
) else (
    echo Application should be starting...
)