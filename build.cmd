@echo off
REM powershell %0\..\build.ps1 %*
powershell -ExecutionPolicy ByPass -File ./build.ps1 -target AllCustom -configuration release
REM powershell -ExecutionPolicy ByPass -File ./build.ps1 -target compile -configuration release
