@echo off
REM powershell %0\..\build.ps1 %*
powershell -ExecutionPolicy ByPass -File ./build.ps1 -target AllCustom -configuration release && set /p temp="Hit enter to continue"
REM powershell -ExecutionPolicy ByPass -File ./build.ps1 -target compile -configuration release
