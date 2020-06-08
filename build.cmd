@echo off
cls

echo.
echo ***** MSBUILD *****
msbuild src\cppacker\cppacker.csproj /p:Configuration=Release /p:OutputPath=..\..\Build\Release

