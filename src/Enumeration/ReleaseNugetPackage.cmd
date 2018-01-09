@echo off
REM Remember to update *.csproj package version .

REM IF NOT [%Microknights_Nuget_ApiKey%] == [] GOTO CheckArgs
REM echo [101;93m Enviroment "Microknights_Nuget_ApiKey" not set [0m
REM goto:eof

:CheckArgs
IF NOT [%1] == [] GOTO ArgumentsOk
echo [101;93m PLEASE: remember to correct package version in *.csproj and *.nuspec. Enter something in argument [0m
goto:eof

:ArgumentsOk

setlocal
set PATH="C:\Program Files (x86)\MSBuild\14.0\Bin\amd64\";%PATH%;
set MSBuildSDKsPath=c:\Program Files\dotnet\sdk\2.0.3\Sdks\

rmdir /q/s R:\NuGet_Release
mkdir R:\NuGet_Release

dotnet build -f netstandard1.3 -c Release -o r:\NuGet_Release\lib\netstandard1.3
dotnet build -f netstandard2.0 -c Release -o r:\NuGet_Release\lib\netstandard2.0
dotnet build -f net452 -c Release -o r:\NuGet_Release\lib\net452
dotnet build -f net46 -c Release -o r:\NuGet_Release\lib\net46

nuget pack MicroKnights.Collections.nuspec -OutputDirectory r:\
