@echo off
REM Remember to update *.csproj package version .

IF NOT [%Microknights_Nuget_ApiKey%] == [] GOTO CheckArgs
echo [101;93m Enviroment "Microknights_Nuget_ApiKey" not set [0m
goto:eof

:CheckArgs
IF NOT [%1] == [] GOTO ArgumentsOk
echo [101;93m Please remember to correct package version in *.csproj and enter something in argument [0m
goto:eof

:ArgumentsOk

setlocal
set PATH="C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\amd64\";%PATH%;
set MSBuildSDKsPath=c:\Program Files\dotnet\sdk\2.0.2\Sdks\

rmdir /q/s R:\NuGet_Release
mkdir R:\NuGet_Release

dotnet build -f netstandard1.3 -c Release -o r:\NuGet_Release\lib\netstandard1.3
dotnet build -f netstandard2.0 -c Release -o r:\NuGet_Release\lib\netstandard2.0

nuget pack MicroKnights.Collections.nuspec -OutputDirectory r:\
