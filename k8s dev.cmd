@echo off
REM powershell %0\..\build.ps1 %*
cls && powershell -ExecutionPolicy ByPass -File ./build.ps1 -target RunTemplate -k8snamespace dev -configuration release

