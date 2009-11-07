@echo off
%windir%\microsoft.net\framework\v3.5\msbuild Phantom.sln /v:minimal
src\phantom\bin\debug\phantom.exe %*