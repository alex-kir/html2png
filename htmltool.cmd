
@echo off

set htmltool_exe=%0\..\htmltool\bin\x64\Release\net9.0-windows\htmltool.exe
set htmltool_proj=%0\..\htmltool\htmltool.csproj

if exist "%htmltool_exe%" goto exe_found

dotnet restore "%htmltool_proj%"
dotnet publish "%htmltool_proj%" --configuration=Release /p:Platform=x64

if exist %htmltool_exe% goto exe_found

goto end_app

:exe_found

%htmltool_exe% %*

:end_app