@echo off
cls && cd .\src\ngApp && ng build --verbose --aot && cd ../../ && set /p temp="Hit enter to continue"

