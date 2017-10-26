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
msbuild /t:pack /p:Configuration=Release /p:OutputPath=r:\NuGet_Release
nuget setApiKey %Microknights_Nuget_ApiKey%
nuget push R:\NuGet_Release\*.nupkg -Source https://www.nuget.org/api/v2/package

